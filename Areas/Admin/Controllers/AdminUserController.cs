using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        private readonly ILogger<AdminUserController> _logger;

        public AdminUserController(
            UserManager<ApplicationUser> userManager,
            ILogger<AdminUserController> logger)
        {
            _userManager = userManager;
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
                return RedirectToAction("Index", "Admin");
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
    }
}

