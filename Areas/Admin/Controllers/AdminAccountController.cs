using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Unicareer.Models;
using Unicareer.Models.ViewModels;

namespace Unicareer.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AllowAnonymous] // Cho phép truy cập mà không cần đăng nhập
    public class AdminAccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<AdminAccountController> _logger;

        public AdminAccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<AdminAccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        // GET: Admin/AdminAccount/Login
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            // Nếu đã đăng nhập và là admin, chuyển đến trang admin
            if (User.Identity?.IsAuthenticated == true && User.IsInRole(SD.Role_Admin))
            {
                return RedirectToAction("Index", "Admin", new { area = "Admin" });
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: Admin/AdminAccount/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AdminLoginViewModel model)
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
            
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                
                // Kiểm tra xem user có phải là Admin không
                if (user == null || !await _userManager.IsInRoleAsync(user, SD.Role_Admin))
                {
                    // Log lần thử đăng nhập không hợp lệ
                    _logger.LogWarning("ADMIN LOGIN FAILED - Invalid user or not admin: Email={Email}, IP={IP}, Time={Time}",
                        model.Email, ipAddress, DateTime.UtcNow);
                    
                    ModelState.AddModelError(string.Empty, "Tài khoản không có quyền truy cập.");
                    return View(model);
                }

                var result = await _signInManager.PasswordSignInAsync(
                    model.Email, 
                    model.MatKhau, 
                    model.GhiNho, 
                    lockoutOnFailure: true); // Bật lockout để bảo mật hơn

                if (result.Succeeded)
                {
                    // Log đăng nhập thành công
                    _logger.LogInformation("ADMIN LOGIN SUCCESS: Email={Email}, IP={IP}, UserAgent={UserAgent}, Time={Time}",
                        model.Email, ipAddress, userAgent, DateTime.UtcNow);
                    
                    // Xóa session rate limiting sau khi đăng nhập thành công
                    var sessionKey = $"admin_login_attempt_{ipAddress}";
                    HttpContext.Session.Remove(sessionKey);
                    
                    // Kiểm tra lại role sau khi đăng nhập thành công
                    if (await _userManager.IsInRoleAsync(user, SD.Role_Admin))
                    {
                        if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                        {
                            return Redirect(model.ReturnUrl);
                        }
                        return RedirectToAction("Index", "Admin", new { area = "Admin" });
                    }
                    else
                    {
                        // Nếu không phải admin, đăng xuất ngay
                        await _signInManager.SignOutAsync();
                        _logger.LogWarning("ADMIN LOGIN REVOKED - User is not admin after login: Email={Email}, IP={IP}",
                            model.Email, ipAddress);
                        ModelState.AddModelError(string.Empty, "Tài khoản không có quyền truy cập.");
                        return View(model);
                    }
                }
                else if (result.IsLockedOut)
                {
                    _logger.LogWarning("ADMIN LOGIN BLOCKED - Account locked out: Email={Email}, IP={IP}, Time={Time}",
                        model.Email, ipAddress, DateTime.UtcNow);
                    ModelState.AddModelError(string.Empty, "Tài khoản đã bị khóa do quá nhiều lần đăng nhập sai. Vui lòng thử lại sau.");
                }
                else
                {
                    // Log lần thử đăng nhập sai
                    _logger.LogWarning("ADMIN LOGIN FAILED - Invalid password: Email={Email}, IP={IP}, Time={Time}",
                        model.Email, ipAddress, DateTime.UtcNow);
                    ModelState.AddModelError(string.Empty, "Email hoặc mật khẩu không đúng");
                }
            }

            return View(model);
        }

        // POST: Admin/AdminAccount/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = SD.Role_Admin)]
        public async Task<IActionResult> Logout()
        {
            var user = await _userManager.GetUserAsync(User);
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            
            // Log đăng xuất
            if (user != null)
            {
                _logger.LogInformation("ADMIN LOGOUT: Email={Email}, IP={IP}, Time={Time}",
                    user.Email, ipAddress, DateTime.UtcNow);
            }
            
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "AdminAccount", new { area = "Admin" });
        }
    }
}

