using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Unicareer.Models;
using Unicareer.Models.ViewModels;
using Unicareer.Services;

namespace Unicareer.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IEmailService _emailService;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<AccountController> logger,
            IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
            _emailService = emailService;
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
                // Kiểm tra email đã tồn tại chưa (kể cả khi tài khoản bị khóa)
                var existingUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    // Kiểm tra tài khoản có bị khóa không
                    var isLockedOut = await _userManager.IsLockedOutAsync(existingUser);
                    var lockoutEnd = await _userManager.GetLockoutEndDateAsync(existingUser);
                    
                    if (isLockedOut && lockoutEnd.HasValue && lockoutEnd.Value > DateTimeOffset.UtcNow)
                    {
                        ModelState.AddModelError(string.Empty, 
                            $"Tài khoản với email {model.Email} đã bị khóa. " +
                            $"Vui lòng liên hệ quản trị viên để được hỗ trợ hoặc thử lại sau khi tài khoản được mở khóa.");
                        _logger.LogWarning("REGISTER ATTEMPT ON LOCKED ACCOUNT: Email={Email}, IP={IP}, Time={Time}",
                            model.Email, HttpContext.Connection.RemoteIpAddress?.ToString(), DateTime.UtcNow);
                        return View(model);
                    }
                    else
                    {
                        // Email đã tồn tại nhưng không bị khóa
                        ModelState.AddModelError(string.Empty, 
                            $"Email {model.Email} đã được sử dụng. " +
                            $"Vui lòng đăng nhập hoặc sử dụng email khác.");
                        return View(model);
                    }
                }

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
            
            // Hiển thị thông báo lỗi từ TempData (nếu có)
            if (TempData["ErrorMessage"] != null)
            {
                ViewData["ErrorMessage"] = TempData["ErrorMessage"];
            }
            
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            // Validate CAPTCHA trước
            var sessionCaptcha = HttpContext.Session.GetString("CaptchaCode");
            if (string.IsNullOrEmpty(sessionCaptcha) || 
                string.IsNullOrEmpty(model.CaptchaCode) ||
                !sessionCaptcha.Equals(model.CaptchaCode, StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError("CaptchaCode", "Mã CAPTCHA không đúng. Vui lòng thử lại.");
                // Tạo lại CAPTCHA mới
                HttpContext.Session.Remove("CaptchaCode");
                return View(model);
            }

            if (ModelState.IsValid)
            {
                // Kiểm tra xem user có phải là Admin không - CHẶN admin đăng nhập qua route công khai
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null && await _userManager.IsInRoleAsync(user, SD.Role_Admin))
                {
                    ModelState.AddModelError(string.Empty, "Tài khoản quản trị phải đăng nhập qua đường link riêng.");
                    HttpContext.Session.Remove("CaptchaCode");
                    return View(model);
                }

                // Xóa CAPTCHA sau khi validate thành công
                HttpContext.Session.Remove("CaptchaCode");

                var result = await _signInManager.PasswordSignInAsync(
                    model.Email, 
                    model.MatKhau, 
                    model.GhiNho, 
                    lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }

                    // Chuyển hướng dựa trên role (không có Admin ở đây vì đã chặn ở trên)
                    if (await _userManager.IsInRoleAsync(user, SD.Role_NhaTuyenDung))
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

            // Nếu có lỗi, tạo lại CAPTCHA mới
            HttpContext.Session.Remove("CaptchaCode");
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
            // Dùng callback path mặc định /signin-google
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        // GET: Account/ExternalLoginCallback - Handle callback từ /signin-google
        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            // DEBUG: Log request info
            _logger.LogInformation("=== GOOGLE CALLBACK DEBUG START ===");
            _logger.LogInformation("URL: {Url}", $"{Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}");
            _logger.LogInformation("Path: {Path}", Request.Path);
            _logger.LogInformation("QueryString: {QueryString}", Request.QueryString);
            _logger.LogInformation("RemoteError: {RemoteError}", remoteError);
            
            // DEBUG: Log cookies
            _logger.LogInformation("=== COOKIES DEBUG ===");
            foreach (var cookie in Request.Cookies)
            {
                var cookieValue = cookie.Value ?? string.Empty;
                _logger.LogInformation("Cookie Name: {Name}, Value Length: {Length}", cookie.Key, cookieValue.Length);
                if (cookie.Key.Contains("Identity") || cookie.Key.Contains("Correlation") || cookie.Key.Contains("Google"))
                {
                    var preview = cookieValue.Length > 200 ? cookieValue.Substring(0, 200) : cookieValue;
                    _logger.LogInformation("  Full Value (first 200 chars): {Value}", preview);
                }
            }

            if (remoteError != null)
            {
                _logger.LogError("Remote error from Google: {Error}", remoteError);
                ModelState.AddModelError(string.Empty, $"Lỗi từ nhà cung cấp: {remoteError}");
                return RedirectToAction("Login", new { ReturnUrl = returnUrl });
            }

            // DEBUG: Try to get external login info
            _logger.LogInformation("Attempting to get external login info...");
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                _logger.LogError("GetExternalLoginInfoAsync returned NULL - OAuth state validation failed");
                _logger.LogError("This usually means the correlation cookie was missing, invalid, or could not be decrypted");
                
                // Try to log authentication result
                var authResult = await HttpContext.AuthenticateAsync("Identity.External");
                _logger.LogInformation("Authentication result: Success={Success}, Failure={Failure}", 
                    authResult?.Succeeded, authResult?.Failure?.Message);
                
                ModelState.AddModelError(string.Empty, "Không thể lấy thông tin từ Google.");
                return RedirectToAction("Login", new { ReturnUrl = returnUrl });
            }
            
            _logger.LogInformation("Successfully retrieved external login info. Provider: {Provider}, Key: {Key}", 
                info.LoginProvider, info.ProviderKey);
            _logger.LogInformation("=== GOOGLE CALLBACK DEBUG END ===");

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
                return await RedirectToHomePage(user, returnUrl);
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
                // Kiểm tra tài khoản có bị khóa không
                var isLockedOut = await _userManager.IsLockedOutAsync(existingUser);
                var lockoutEnd = await _userManager.GetLockoutEndDateAsync(existingUser);
                
                if (isLockedOut && lockoutEnd.HasValue && lockoutEnd.Value > DateTimeOffset.UtcNow)
                {
                    _logger.LogWarning("GOOGLE LOGIN ATTEMPT ON LOCKED ACCOUNT: Email={Email}, IP={IP}, Time={Time}",
                        email, HttpContext.Connection.RemoteIpAddress?.ToString(), DateTime.UtcNow);
                    TempData["ErrorMessage"] = 
                        $"Tài khoản với email {email} đã bị khóa. " +
                        $"Vui lòng liên hệ quản trị viên để được hỗ trợ.";
                    return RedirectToAction("Login");
                }

                // Thêm external login cho user hiện có
                var addLoginResult = await _userManager.AddLoginAsync(existingUser, info);
                if (addLoginResult.Succeeded)
                {
                    // Nếu user chưa có bất kỳ role nào, gán role mặc định là Ứng Viên
                    var currentRoles = await _userManager.GetRolesAsync(existingUser);
                    if (currentRoles == null || currentRoles.Count == 0)
                    {
                        await _userManager.AddToRoleAsync(existingUser, SD.Role_UngVien);
                    }

                    await _signInManager.SignInAsync(existingUser, isPersistent: false);
                    return await RedirectToHomePage(existingUser, returnUrl);
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

            // Kiểm tra email đã tồn tại chưa (kể cả khi tài khoản bị khóa)
            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser != null)
            {
                // Kiểm tra tài khoản có bị khóa không
                var isLockedOut = await _userManager.IsLockedOutAsync(existingUser);
                var lockoutEnd = await _userManager.GetLockoutEndDateAsync(existingUser);
                
                if (isLockedOut && lockoutEnd.HasValue && lockoutEnd.Value > DateTimeOffset.UtcNow)
                {
                    _logger.LogWarning("GOOGLE REGISTER ATTEMPT ON LOCKED ACCOUNT: Email={Email}, IP={IP}, Time={Time}",
                        email, HttpContext.Connection.RemoteIpAddress?.ToString(), DateTime.UtcNow);
                    ModelState.AddModelError(string.Empty, 
                        $"Tài khoản với email {email} đã bị khóa. " +
                        $"Vui lòng liên hệ quản trị viên để được hỗ trợ hoặc thử lại sau khi tài khoản được mở khóa.");
                    ViewBag.Email = email;
                    ViewBag.Name = name;
                    TempData.Keep();
                    return View();
                }
                else
                {
                    // Email đã tồn tại nhưng không bị khóa - thêm external login cho tài khoản hiện có
                    var addLoginResult = await _userManager.AddLoginAsync(existingUser, 
                        new UserLoginInfo(provider, providerKey, provider));
                    
                    if (addLoginResult.Succeeded)
                    {
                        // Nếu user chưa có bất kỳ role nào, gán role mặc định
                        var currentRoles = await _userManager.GetRolesAsync(existingUser);
                        if (currentRoles == null || currentRoles.Count == 0)
                        {
                            await _userManager.AddToRoleAsync(existingUser, loaiTaiKhoan);
                        }
                        
                        await _signInManager.SignInAsync(existingUser, isPersistent: false);
                        return await RedirectToHomePage(existingUser, returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, 
                            $"Email {email} đã được sử dụng. " +
                            $"Vui lòng đăng nhập hoặc sử dụng email khác.");
                        ViewBag.Email = email;
                        ViewBag.Name = name;
                        TempData.Keep();
                        return View();
                    }
                }
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
                    
                    return await RedirectToHomePage(user, returnUrl);
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

        private async Task<IActionResult> RedirectToHomePage(ApplicationUser user, string? returnUrl = null)
        {
            // Ưu tiên sử dụng returnUrl nếu có và là URL hợp lệ
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            // Nếu không có returnUrl hoặc không hợp lệ, redirect theo role
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

        // GET: Account/ForgotPassword
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        // POST: Account/ForgotPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _logger.LogInformation("ForgotPassword request for email: {Email}", model.Email);
                    
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    
                    if (user != null)
                    {
                        _logger.LogInformation("User found for email: {Email}", model.Email);
                        
                        // Tạo mã xác thực 6 chữ số
                        var random = new Random();
                        var verificationCode = random.Next(100000, 999999).ToString();
                        
                        _logger.LogInformation("Generated verification code for {Email}: {Code}", model.Email, verificationCode);
                        
                        // Lưu mã xác thực vào session với thời gian hết hạn 10 phút
                        HttpContext.Session.SetString($"VerificationCode_{model.Email}", verificationCode);
                        HttpContext.Session.SetString($"VerificationCodeTime_{model.Email}", DateTime.UtcNow.ToString("O"));
                        
                        _logger.LogInformation("Verification code saved to session for {Email}", model.Email);
                        
                        // Gửi email
                        _logger.LogInformation("Attempting to send verification email to {Email}", model.Email);
                        var emailSent = await _emailService.SendVerificationCodeAsync(model.Email, verificationCode);
                        
                        if (emailSent)
                        {
                            _logger.LogInformation("Verification code sent successfully to {Email}", model.Email);
                            TempData["SuccessMessage"] = "Mã xác thực đã được gửi đến email của bạn. Vui lòng kiểm tra hộp thư (bao gồm cả thư mục Spam).";
                            TempData["Email"] = model.Email;
                            return RedirectToAction("VerifyCode");
                        }
                        else
                        {
                            _logger.LogError("Failed to send verification email to {Email}. Check email configuration and logs.", model.Email);
                            ModelState.AddModelError(string.Empty, 
                                "Không thể gửi email xác thực. Vui lòng kiểm tra:\n" +
                                "- Cấu hình email trong hệ thống\n" +
                                "- Kết nối internet\n" +
                                "- Thư mục Spam trong hộp thư\n\n" +
                                "Nếu vấn đề vẫn tiếp tục, vui lòng liên hệ quản trị viên.");
                            // Xóa mã đã lưu trong session vì không gửi được email
                            HttpContext.Session.Remove($"VerificationCode_{model.Email}");
                            HttpContext.Session.Remove($"VerificationCodeTime_{model.Email}");
                        }
                    }
                    else
                    {
                        _logger.LogWarning("User not found for email: {Email}", model.Email);
                        // Vẫn hiển thị thông báo thành công để bảo mật
                        TempData["SuccessMessage"] = "Nếu email tồn tại trong hệ thống, mã xác thực đã được gửi đến email của bạn.";
                        return RedirectToAction("Login");
                    }
                }
                else
                {
                    _logger.LogWarning("ModelState is invalid for ForgotPassword. Errors: {Errors}", 
                        string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred in ForgotPassword for email: {Email}. Exception: {Message}", 
                    model.Email, ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Inner exception: {InnerMessage}", ex.InnerException.Message);
                }
                ModelState.AddModelError(string.Empty, 
                    $"Đã xảy ra lỗi khi xử lý yêu cầu: {ex.Message}. Vui lòng thử lại sau hoặc liên hệ quản trị viên.");
            }

            return View(model);
        }

        // GET: Account/VerifyCode
        [HttpGet]
        public IActionResult VerifyCode()
        {
            var email = TempData["Email"]?.ToString();
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("ForgotPassword");
            }

            var model = new VerifyCodeViewModel
            {
                Email = email
            };

            // Giữ lại email trong TempData
            TempData.Keep("Email");
            
            return View(model);
        }

        // POST: Account/VerifyCode
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult VerifyCode(VerifyCodeViewModel model)
        {
            // Đảm bảo email có trong model hoặc TempData
            if (string.IsNullOrEmpty(model.Email))
            {
                model.Email = TempData["Email"]?.ToString() ?? string.Empty;
            }

            if (string.IsNullOrEmpty(model.Email))
            {
                return RedirectToAction("ForgotPassword");
            }

            if (ModelState.IsValid)
            {
                // Lấy mã xác thực từ session
                var storedCode = HttpContext.Session.GetString($"VerificationCode_{model.Email}");
                var codeTimeStr = HttpContext.Session.GetString($"VerificationCodeTime_{model.Email}");

                if (string.IsNullOrEmpty(storedCode) || string.IsNullOrEmpty(codeTimeStr))
                {
                    ModelState.AddModelError(string.Empty, "Mã xác thực đã hết hạn. Vui lòng yêu cầu mã mới.");
                    TempData.Keep("Email");
                    return View(model);
                }

                // Kiểm tra thời gian hết hạn (10 phút)
                if (DateTime.TryParse(codeTimeStr, out var codeTime))
                {
                    var timeElapsed = DateTime.UtcNow - codeTime;
                    if (timeElapsed.TotalMinutes > 10)
                    {
                        // Xóa mã đã hết hạn
                        HttpContext.Session.Remove($"VerificationCode_{model.Email}");
                        HttpContext.Session.Remove($"VerificationCodeTime_{model.Email}");
                        
                        ModelState.AddModelError(string.Empty, "Mã xác thực đã hết hạn. Vui lòng yêu cầu mã mới.");
                        TempData.Keep("Email");
                        return View(model);
                    }
                }

                // Kiểm tra mã xác thực
                if (storedCode == model.VerificationCode)
                {
                    // Xóa mã xác thực sau khi xác nhận thành công
                    HttpContext.Session.Remove($"VerificationCode_{model.Email}");
                    HttpContext.Session.Remove($"VerificationCodeTime_{model.Email}");
                    
                    // Lưu email vào TempData để reset password
                    TempData["VerifiedEmail"] = model.Email;
                    return RedirectToAction("ResetPassword");
                }
                else
                {
                    ModelState.AddModelError("VerificationCode", "Mã xác thực không đúng. Vui lòng thử lại.");
                }
            }

            TempData.Keep("Email");
            return View(model);
        }

        // GET: Account/ResetPassword
        [HttpGet]
        public IActionResult ResetPassword()
        {
            var email = TempData["VerifiedEmail"]?.ToString();
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("ForgotPassword");
            }

            var model = new ResetPasswordViewModel
            {
                Email = email
            };

            // Giữ lại email trong TempData
            TempData.Keep("VerifiedEmail");
            
            return View(model);
        }

        // POST: Account/ResetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            // Đảm bảo email có trong model hoặc TempData
            if (string.IsNullOrEmpty(model.Email))
            {
                model.Email = TempData["VerifiedEmail"]?.ToString() ?? string.Empty;
            }

            if (string.IsNullOrEmpty(model.Email))
            {
                return RedirectToAction("ForgotPassword");
            }

            if (ModelState.IsValid)
            {
                var email = model.Email;
                var user = await _userManager.FindByEmailAsync(email);

                if (user != null)
                {
                    // Xóa mật khẩu cũ và đặt mật khẩu mới
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);

                    if (result.Succeeded)
                    {
                        _logger.LogInformation("Password reset successful for {Email}", email);
                        TempData["SuccessMessage"] = "Đặt lại mật khẩu thành công. Vui lòng đăng nhập với mật khẩu mới.";
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Không tìm thấy tài khoản.");
                }
            }

            // Giữ lại email trong TempData nếu có
            if (!string.IsNullOrEmpty(model.Email))
            {
                TempData["VerifiedEmail"] = model.Email;
            }
            else
            {
                TempData.Keep("VerifiedEmail");
            }
            return View(model);
        }
    }
}

