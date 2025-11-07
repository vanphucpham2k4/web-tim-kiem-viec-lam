using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Unicareer.Data;
using Unicareer.Models;
using Unicareer.Models.ViewModels;

namespace Unicareer.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class AdminUserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<AdminUserController> _logger;

        public AdminUserController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<AdminUserController> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        // GET: Admin/AdminUser/ChangePassword
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        // POST: Admin/AdminUser/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            // Kiểm tra mật khẩu cũ
            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            
            if (result.Succeeded)
            {
                _logger.LogInformation("ADMIN PASSWORD CHANGED: Email={Email}, IP={IP}, Time={Time}",
                    user.Email, HttpContext.Connection.RemoteIpAddress?.ToString(), DateTime.UtcNow);
                
                TempData["SuccessMessage"] = "Đổi mật khẩu thành công!";
                return RedirectToAction("Settings", "Admin");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        // POST: Admin/AdminUser/DeleteUser
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest();
            }

            var userToDelete = await _userManager.FindByIdAsync(userId);
            if (userToDelete == null)
            {
                return NotFound();
            }

            // BẢO VỆ: Không cho phép xóa tài khoản admin đầu tiên
            if (DbInitializer.IsPrimaryAdmin(userToDelete))
            {
                _logger.LogWarning("BLOCKED DELETE PRIMARY ADMIN: Attempted by {CurrentUser}, IP={IP}, Time={Time}",
                    User.Identity?.Name, HttpContext.Connection.RemoteIpAddress?.ToString(), DateTime.UtcNow);
                
                TempData["ErrorMessage"] = "Không thể xóa tài khoản quản trị viên đầu tiên!";
                return RedirectToAction("NhaTuyenDung", "Admin");
            }

            // Kiểm tra xem user có phải là admin không
            if (await _userManager.IsInRoleAsync(userToDelete, SD.Role_Admin))
            {
                // Không cho phép xóa admin khác (chỉ có thể xóa user thường)
                TempData["ErrorMessage"] = "Không thể xóa tài khoản quản trị viên!";
                return RedirectToAction("NhaTuyenDung", "Admin");
            }

            var result = await _userManager.DeleteAsync(userToDelete);
            
            if (result.Succeeded)
            {
                _logger.LogInformation("USER DELETED: UserId={UserId}, Email={Email}, DeletedBy={DeletedBy}, IP={IP}, Time={Time}",
                    userId, userToDelete.Email, User.Identity?.Name, HttpContext.Connection.RemoteIpAddress?.ToString(), DateTime.UtcNow);
                
                TempData["SuccessMessage"] = "Xóa người dùng thành công!";
            }
            else
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi xóa người dùng.";
            }

            return RedirectToAction("NhaTuyenDung", "Admin");
        }

        // POST: Admin/AdminUser/EditUser
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(string userId, string hoTen, string phoneNumber)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest();
            }

            var userToEdit = await _userManager.FindByIdAsync(userId);
            if (userToEdit == null)
            {
                return NotFound();
            }

            // BẢO VỆ: Không cho phép sửa tài khoản admin đầu tiên (trừ mật khẩu)
            if (DbInitializer.IsPrimaryAdmin(userToEdit))
            {
                _logger.LogWarning("BLOCKED EDIT PRIMARY ADMIN: Attempted by {CurrentUser}, IP={IP}, Time={Time}",
                    User.Identity?.Name, HttpContext.Connection.RemoteIpAddress?.ToString(), DateTime.UtcNow);
                
                TempData["ErrorMessage"] = "Không thể sửa thông tin tài khoản quản trị viên đầu tiên!";
                return RedirectToAction("NhaTuyenDung", "Admin");
            }

            // Cho phép sửa thông tin user thường
            userToEdit.HoTen = hoTen;
            userToEdit.PhoneNumber = phoneNumber;

            var result = await _userManager.UpdateAsync(userToEdit);
            
            if (result.Succeeded)
            {
                _logger.LogInformation("USER UPDATED: UserId={UserId}, Email={Email}, UpdatedBy={UpdatedBy}, IP={IP}, Time={Time}",
                    userId, userToEdit.Email, User.Identity?.Name, HttpContext.Connection.RemoteIpAddress?.ToString(), DateTime.UtcNow);
                
                TempData["SuccessMessage"] = "Cập nhật thông tin thành công!";
            }
            else
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi cập nhật thông tin.";
            }

            return RedirectToAction("NhaTuyenDung", "Admin");
        }

        // GET: Admin/AdminUser/UserManagement
        [HttpGet]
        public async Task<IActionResult> UserManagement(string? searchTerm, string? roleFilter)
        {
            ViewData["Title"] = "Quản lý người dùng";
            
            var allUsers = _userManager.Users.ToList();
            var model = new UserManagementViewModel
            {
                SearchTerm = searchTerm,
                RoleFilter = roleFilter,
                AvailableRoles = _roleManager.Roles.Select(r => r.Name!).ToList()
            };

            // Lọc theo search term
            if (!string.IsNullOrEmpty(searchTerm))
            {
                allUsers = allUsers.Where(u => 
                    (u.Email != null && u.Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                    (u.HoTen != null && u.HoTen.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                    (u.PhoneNumber != null && u.PhoneNumber.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                ).ToList();
            }

            // Lọc theo role
            if (!string.IsNullOrEmpty(roleFilter))
            {
                var usersInRole = await _userManager.GetUsersInRoleAsync(roleFilter);
                var userIdsInRole = usersInRole.Select(u => u.Id).ToHashSet();
                allUsers = allUsers.Where(u => userIdsInRole.Contains(u.Id)).ToList();
            }

            // Chuyển đổi sang UserInfo
            foreach (var user in allUsers)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var isLockedOut = await _userManager.IsLockedOutAsync(user);
                var lockoutEnd = await _userManager.GetLockoutEndDateAsync(user);

                model.Users.Add(new UserInfo
                {
                    Id = user.Id,
                    Email = user.Email ?? "",
                    HoTen = user.HoTen,
                    PhoneNumber = user.PhoneNumber,
                    NgayDangKy = user.NgayDangKy,
                    Roles = roles.ToList(),
                    IsLockedOut = isLockedOut,
                    LockoutEnd = lockoutEnd?.DateTime,
                    EmailConfirmed = user.EmailConfirmed,
                    IsPrimaryAdmin = DbInitializer.IsPrimaryAdmin(user)
                });
            }

            // Sắp xếp theo ngày đăng ký (mới nhất trước)
            model.Users = model.Users.OrderByDescending(u => u.NgayDangKy).ToList();

            return View(model);
        }

        // GET: Admin/AdminUser/ResetUserPassword
        [HttpGet]
        public async Task<IActionResult> ResetUserPassword(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var model = new ResetUserPasswordViewModel
            {
                UserId = user.Id,
                UserEmail = user.Email,
                UserName = user.HoTen ?? user.Email
            };

            return View(model);
        }

        // POST: Admin/AdminUser/ResetUserPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetUserPassword(ResetUserPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.UserId);
                if (user != null)
                {
                    model.UserEmail = user.Email;
                    model.UserName = user.HoTen ?? user.Email;
                }
                return View(model);
            }

            var userToReset = await _userManager.FindByIdAsync(model.UserId);
            if (userToReset == null)
            {
                return NotFound();
            }

            // BẢO VỆ: Không cho phép reset mật khẩu của admin đầu tiên (trừ khi chính admin đó)
            if (DbInitializer.IsPrimaryAdmin(userToReset) && 
                !DbInitializer.IsPrimaryAdmin(await _userManager.GetUserAsync(User)))
            {
                TempData["ErrorMessage"] = "Không thể reset mật khẩu của tài khoản quản trị viên đầu tiên!";
                return RedirectToAction("UserManagement");
            }

            // Reset mật khẩu (không cần mật khẩu cũ)
            var token = await _userManager.GeneratePasswordResetTokenAsync(userToReset);
            var result = await _userManager.ResetPasswordAsync(userToReset, token, model.NewPassword);

            if (result.Succeeded)
            {
                _logger.LogInformation("ADMIN RESET PASSWORD: UserId={UserId}, Email={Email}, ResetBy={ResetBy}, IP={IP}, Time={Time}",
                    userToReset.Id, userToReset.Email, User.Identity?.Name, HttpContext.Connection.RemoteIpAddress?.ToString(), DateTime.UtcNow);
                
                TempData["SuccessMessage"] = $"Đã reset mật khẩu thành công cho {userToReset.Email}!";
                return RedirectToAction("UserManagement");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            model.UserEmail = userToReset.Email;
            model.UserName = userToReset.HoTen ?? userToReset.Email;
            return View(model);
        }

        // POST: Admin/AdminUser/AssignRole
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignRole(string userId, string roleName)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(roleName))
            {
                return BadRequest();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            // BẢO VỆ: Không cho phép thay đổi role của admin đầu tiên
            if (DbInitializer.IsPrimaryAdmin(user))
            {
                TempData["ErrorMessage"] = "Không thể thay đổi role của tài khoản quản trị viên đầu tiên!";
                return RedirectToAction("UserManagement");
            }

            // Kiểm tra role có tồn tại không
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                TempData["ErrorMessage"] = "Role không tồn tại!";
                return RedirectToAction("UserManagement");
            }

            // Xóa tất cả role hiện tại
            var currentRoles = await _userManager.GetRolesAsync(user);
            if (currentRoles.Any())
            {
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
            }

            // Gán role mới
            var result = await _userManager.AddToRoleAsync(user, roleName);

            if (result.Succeeded)
            {
                _logger.LogInformation("ADMIN ASSIGN ROLE: UserId={UserId}, Email={Email}, Role={Role}, AssignedBy={AssignedBy}, IP={IP}, Time={Time}",
                    user.Id, user.Email, roleName, User.Identity?.Name, HttpContext.Connection.RemoteIpAddress?.ToString(), DateTime.UtcNow);
                
                TempData["SuccessMessage"] = $"Đã gán role '{roleName}' cho {user.Email}!";
            }
            else
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi gán role.";
            }

            return RedirectToAction("UserManagement");
        }

        // POST: Admin/AdminUser/LockUnlockUser
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LockUnlockUser(string userId, bool lockUser)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            // BẢO VỆ: Không cho phép khóa admin đầu tiên
            if (DbInitializer.IsPrimaryAdmin(user) && lockUser)
            {
                TempData["ErrorMessage"] = "Không thể khóa tài khoản quản trị viên đầu tiên!";
                return RedirectToAction("UserManagement");
            }

            if (lockUser)
            {
                // Khóa tài khoản trong 365 ngày
                var result = await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddDays(365));
                if (result.Succeeded)
                {
                    _logger.LogInformation("ADMIN LOCK USER: UserId={UserId}, Email={Email}, LockedBy={LockedBy}, IP={IP}, Time={Time}",
                        user.Id, user.Email, User.Identity?.Name, HttpContext.Connection.RemoteIpAddress?.ToString(), DateTime.UtcNow);
                    
                    TempData["SuccessMessage"] = $"Đã khóa tài khoản {user.Email}!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Có lỗi xảy ra khi khóa tài khoản.";
                }
            }
            else
            {
                // Mở khóa tài khoản
                var result = await _userManager.SetLockoutEndDateAsync(user, null);
                if (result.Succeeded)
                {
                    _logger.LogInformation("ADMIN UNLOCK USER: UserId={UserId}, Email={Email}, UnlockedBy={UnlockedBy}, IP={IP}, Time={Time}",
                        user.Id, user.Email, User.Identity?.Name, HttpContext.Connection.RemoteIpAddress?.ToString(), DateTime.UtcNow);
                    
                    TempData["SuccessMessage"] = $"Đã mở khóa tài khoản {user.Email}!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Có lỗi xảy ra khi mở khóa tài khoản.";
                }
            }

            return RedirectToAction("UserManagement");
        }

        // GET: Admin/AdminUser/UserDetails
        [HttpGet]
        public async Task<IActionResult> UserDetails(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);
            var isLockedOut = await _userManager.IsLockedOutAsync(user);
            var lockoutEnd = await _userManager.GetLockoutEndDateAsync(user);

            var userInfo = new UserInfo
            {
                Id = user.Id,
                Email = user.Email ?? "",
                HoTen = user.HoTen,
                PhoneNumber = user.PhoneNumber,
                NgayDangKy = user.NgayDangKy,
                Roles = roles.ToList(),
                IsLockedOut = isLockedOut,
                LockoutEnd = lockoutEnd?.DateTime,
                EmailConfirmed = user.EmailConfirmed,
                IsPrimaryAdmin = DbInitializer.IsPrimaryAdmin(user)
            };

            ViewData["Title"] = $"Chi tiết người dùng: {userInfo.Email}";
            return View(userInfo);
        }

        // GET: Admin/AdminUser/CreateUser
        [HttpGet]
        public IActionResult CreateUser()
        {
            ViewData["Title"] = "Tạo tài khoản mới";
            
            var model = new CreateUserViewModel();
            var roles = new List<SelectListItem>();
            foreach (var role in _roleManager.Roles)
            {
                string roleText = role.Name;
                if (role.Name == "Admin")
                    roleText = "Quản trị viên";
                else if (role.Name == "NhaTuyenDung")
                    roleText = "Nhà tuyển dụng";
                else if (role.Name == "UngVien")
                    roleText = "Ứng viên";

                roles.Add(new SelectListItem
                {
                    Value = role.Name,
                    Text = roleText
                });
            }
            ViewBag.AvailableRoles = roles;

            return View(model);
        }

        // POST: Admin/AdminUser/CreateUser
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(CreateUserViewModel model)
        {
            ViewData["Title"] = "Tạo tài khoản mới";
            var roles = new List<SelectListItem>();
            foreach (var role in _roleManager.Roles)
            {
                string roleText = role.Name;
                if (role.Name == "Admin")
                    roleText = "Quản trị viên";
                else if (role.Name == "NhaTuyenDung")
                    roleText = "Nhà tuyển dụng";
                else if (role.Name == "UngVien")
                    roleText = "Ứng viên";

                roles.Add(new SelectListItem
                {
                    Value = role.Name,
                    Text = roleText,
                    Selected = role.Name == model.Role
                });
            }
            ViewBag.AvailableRoles = roles;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Kiểm tra email đã tồn tại chưa
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", $"Email {model.Email} đã được sử dụng. Vui lòng sử dụng email khác.");
                return View(model);
            }

            // Kiểm tra role có tồn tại không
            if (!await _roleManager.RoleExistsAsync(model.Role))
            {
                ModelState.AddModelError("Role", "Role không tồn tại!");
                return View(model);
            }

            // Tạo user mới
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                HoTen = model.HoTen,
                PhoneNumber = model.SoDienThoai,
                LoaiTaiKhoan = model.Role,
                NgayDangKy = DateTime.Now,
                EmailConfirmed = true // Admin tạo tài khoản thì coi như đã xác nhận email
            };

            var result = await _userManager.CreateAsync(user, model.MatKhau);

            if (result.Succeeded)
            {
                // Gán role cho user
                var roleResult = await _userManager.AddToRoleAsync(user, model.Role);
                
                if (roleResult.Succeeded)
                {
                    _logger.LogInformation("ADMIN CREATED USER: UserId={UserId}, Email={Email}, Role={Role}, CreatedBy={CreatedBy}, IP={IP}, Time={Time}",
                        user.Id, user.Email, model.Role, User.Identity?.Name, HttpContext.Connection.RemoteIpAddress?.ToString(), DateTime.UtcNow);
                    
                    TempData["SuccessMessage"] = $"Đã tạo tài khoản thành công cho {user.Email}!";
                    return RedirectToAction("UserManagement");
                }
                else
                {
                    // Nếu gán role thất bại, xóa user vừa tạo
                    await _userManager.DeleteAsync(user);
                    foreach (var error in roleResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }
    }
}

