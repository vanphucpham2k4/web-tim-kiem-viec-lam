using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Unicareer.Models;
using Unicareer.Models.ViewModels;

namespace Unicareer.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        // GET: Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    HoTen = model.HoTen,
                    PhoneNumber = model.SoDienThoai,
                    LoaiTaiKhoan = model.LoaiTaiKhoan,
                    NgayDangKy = DateTime.Now
                };

                var result = await _userManager.CreateAsync(user, model.MatKhau);

                if (result.Succeeded)
                {
                    // Gán role cho user dựa trên loại tài khoản
                    string roleName = model.LoaiTaiKhoan == SD.Role_NhaTuyenDung 
                        ? SD.Role_NhaTuyenDung 
                        : SD.Role_UngVien;
                    
                    await _userManager.AddToRoleAsync(user, roleName);
                    
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    
                    // Chuyển hướng dựa trên loại tài khoản
                    if (model.LoaiTaiKhoan == SD.Role_NhaTuyenDung)
                    {
                        return RedirectToAction("Index", "Recruiter", new { area = "Recruiter" });
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        // GET: Account/Login
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(
                    model.Email, 
                    model.MatKhau, 
                    model.GhiNho, 
                    lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }

                    // Chuyển hướng dựa trên role
                    if (await _userManager.IsInRoleAsync(user, SD.Role_Admin))
                    {
                        return RedirectToAction("Index", "Admin", new { area = "Admin" });
                    }
                    else if (await _userManager.IsInRoleAsync(user, SD.Role_NhaTuyenDung))
                    {
                        return RedirectToAction("Index", "Recruiter", new { area = "Recruiter" });
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }

                ModelState.AddModelError(string.Empty, "Email hoặc mật khẩu không đúng");
            }

            return View(model);
        }

        // POST: Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        // GET: Account/AccessDenied
        public IActionResult AccessDenied()
        {
            return View();
        }

        // GET: Account/ExternalLogin
        [HttpGet]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            var redirectUrl = Url.Action("GoogleCallback", "Account", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        // GET: Account/GoogleCallback
        [HttpGet]
        public async Task<IActionResult> GoogleCallback(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Lỗi từ nhà cung cấp: {remoteError}");
                return RedirectToAction("Login", new { ReturnUrl = returnUrl });
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ModelState.AddModelError(string.Empty, "Không thể lấy thông tin từ Google.");
                return RedirectToAction("Login", new { ReturnUrl = returnUrl });
            }

            // Thử đăng nhập với external login
            var signInResult = await _signInManager.ExternalLoginSignInAsync(
                info.LoginProvider, 
                info.ProviderKey, 
                isPersistent: false, 
                bypassTwoFactor: true);

            if (signInResult.Succeeded)
            {
                // Đăng nhập thành công
                var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
                return await RedirectToHomePage(user);
            }

            // Nếu user chưa có tài khoản, lấy thông tin từ Google và yêu cầu chọn loại tài khoản
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var name = info.Principal.FindFirstValue(ClaimTypes.Name);

            if (string.IsNullOrEmpty(email))
            {
                ModelState.AddModelError(string.Empty, "Không thể lấy email từ Google.");
                return RedirectToAction("Login", new { ReturnUrl = returnUrl });
            }

            // Kiểm tra xem email đã tồn tại chưa
            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser != null)
            {
                // Thêm external login cho user hiện có
                var addLoginResult = await _userManager.AddLoginAsync(existingUser, info);
                if (addLoginResult.Succeeded)
                {
                    await _signInManager.SignInAsync(existingUser, isPersistent: false);
                    return await RedirectToHomePage(existingUser);
                }
            }

            // Lưu thông tin external login vào session để sử dụng sau khi chọn loại tài khoản
            TempData["ExternalLoginProvider"] = info.LoginProvider;
            TempData["ExternalLoginProviderKey"] = info.ProviderKey;
            TempData["ExternalEmail"] = email;
            TempData["ExternalName"] = name;
            TempData["ReturnUrl"] = returnUrl;

            return RedirectToAction("ChooseAccountType");
        }

        // GET: Account/ChooseAccountType
        [HttpGet]
        public IActionResult ChooseAccountType()
        {
            // Kiểm tra xem có thông tin external login không
            if (TempData["ExternalLoginProvider"] == null)
            {
                return RedirectToAction("Login");
            }

            ViewBag.Email = TempData["ExternalEmail"];
            ViewBag.Name = TempData["ExternalName"];
            
            // Giữ lại giá trị trong TempData cho lần request tiếp theo
            TempData.Keep("ExternalLoginProvider");
            TempData.Keep("ExternalLoginProviderKey");
            TempData.Keep("ExternalEmail");
            TempData.Keep("ExternalName");
            TempData.Keep("ReturnUrl");

            return View();
        }

        // POST: Account/ChooseAccountType
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChooseAccountType(string loaiTaiKhoan)
        {
            var provider = TempData["ExternalLoginProvider"]?.ToString();
            var providerKey = TempData["ExternalLoginProviderKey"]?.ToString();
            var email = TempData["ExternalEmail"]?.ToString();
            var name = TempData["ExternalName"]?.ToString();
            var returnUrl = TempData["ReturnUrl"]?.ToString() ?? Url.Content("~/");

            if (string.IsNullOrEmpty(provider) || string.IsNullOrEmpty(providerKey) || string.IsNullOrEmpty(email))
            {
                ModelState.AddModelError(string.Empty, "Thông tin đăng nhập không hợp lệ.");
                return RedirectToAction("Login");
            }

            if (string.IsNullOrEmpty(loaiTaiKhoan) || 
                (loaiTaiKhoan != SD.Role_UngVien && loaiTaiKhoan != SD.Role_NhaTuyenDung))
            {
                ModelState.AddModelError(string.Empty, "Vui lòng chọn loại tài khoản.");
                ViewBag.Email = email;
                ViewBag.Name = name;
                TempData.Keep();
                return View();
            }

            // Tạo user mới
            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                HoTen = name ?? "Người dùng",
                LoaiTaiKhoan = loaiTaiKhoan,
                NgayDangKy = DateTime.Now,
                EmailConfirmed = true // Email đã được xác nhận từ Google
            };

            var result = await _userManager.CreateAsync(user);
            if (result.Succeeded)
            {
                // Thêm external login
                var addLoginResult = await _userManager.AddLoginAsync(user, 
                    new UserLoginInfo(provider, providerKey, provider));
                
                if (addLoginResult.Succeeded)
                {
                    // Gán role
                    await _userManager.AddToRoleAsync(user, loaiTaiKhoan);
                    
                    // Đăng nhập
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    
                    return await RedirectToHomePage(user);
                }
                else
                {
                    foreach (var error in addLoginResult.Errors)
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

            ViewBag.Email = email;
            ViewBag.Name = name;
            TempData.Keep();
            return View();
        }

        private async Task<IActionResult> RedirectToHomePage(ApplicationUser user)
        {
            if (await _userManager.IsInRoleAsync(user, SD.Role_Admin))
            {
                return RedirectToAction("Index", "Admin", new { area = "Admin" });
            }
            else if (await _userManager.IsInRoleAsync(user, SD.Role_NhaTuyenDung))
            {
                return RedirectToAction("Index", "Recruiter", new { area = "Recruiter" });
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}

