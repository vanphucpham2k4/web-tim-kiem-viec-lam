using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using Unicareer.Data;
using Unicareer.Models;
using Unicareer.Repository;
using Unicareer.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Đăng ký Repository với Dependency Injection
// Tự động chuyển đổi giữa Mock Data và Real Data dựa trên cấu hình Repository:UseMockData
// - Development: mặc định sử dụng Mock Data (có thể thay đổi trong appsettings.Development.json)
// - Production: mặc định sử dụng Real Data (có thể thay đổi trong appsettings.json)
var useMockData = builder.Configuration.GetValue<bool>("Repository:UseMockData", false);
builder.Services.AddRepositories(builder.Configuration);

// Đăng ký Background Service để tự động cập nhật trạng thái tin tuyển dụng hết hạn
builder.Services.AddHostedService<UpdateTrangThaiBackgroundService>();

// Đăng ký Email Service
builder.Services.AddScoped<IEmailService, EmailService>();

// Log trạng thái sử dụng repository (sẽ log sau khi app được build)

// Cấu hình Session để lưu CAPTCHA
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
});

// Cấu hình DataProtection để giữ khóa ổn định (tránh mất state sau redirect)
var keysDir = Path.Combine(builder.Environment.ContentRootPath, "App_Data", "keys");
Directory.CreateDirectory(keysDir);
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(keysDir))
    .SetApplicationName("Unicareer")
    .SetDefaultKeyLifetime(TimeSpan.FromDays(90)); // Tăng thời gian sống của key

// Cấu hình Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Cấu hình Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Cấu hình Password
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 0;

    // Cấu hình User
    options.User.RequireUniqueEmail = true;

    // Cấu hình SignIn
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
    
    // Cấu hình Lockout để bảo mật cao cho admin
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    options.Lockout.MaxFailedAccessAttempts = 5; // Khóa sau 5 lần thử sai
    options.Lockout.AllowedForNewUsers = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Cấu hình Cookie
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
    options.SlidingExpiration = true;
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Bắt buộc Secure trên HTTPS
});

// Cấu hình External cookie (dùng trong quá trình đăng nhập ngoài)
builder.Services.ConfigureExternalCookie(options =>
{
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Cấu hình Google Authentication
var googleClientId = builder.Configuration["Authentication:Google:ClientId"];
var googleClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];

if (!string.IsNullOrEmpty(googleClientId) && !string.IsNullOrEmpty(googleClientSecret) && 
    googleClientId != "YOUR_GOOGLE_CLIENT_ID" && googleClientSecret != "YOUR_GOOGLE_CLIENT_SECRET")
{
    builder.Services.AddAuthentication()
        .AddGoogle(options =>
        {
            options.ClientId = googleClientId;
            options.ClientSecret = googleClientSecret;
            options.CallbackPath = "/signin-google"; // Dùng callback path mặc định của ASP.NET Core
            // Lưu tokens để sử dụng sau này
            options.SaveTokens = true;
            
            // Cấu hình CorrelationCookie để lưu OAuth state
            // Dùng None cho SameSite vì redirect từ Google về localhost là cross-site
            if (builder.Environment.IsDevelopment())
            {
                options.CorrelationCookie.SameSite = SameSiteMode.Lax;
                options.CorrelationCookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            }
            else
            {
                options.CorrelationCookie.SameSite = SameSiteMode.None;
                options.CorrelationCookie.SecurePolicy = CookieSecurePolicy.Always;
            }
            options.CorrelationCookie.Path = "/";
            options.CorrelationCookie.HttpOnly = true;
            options.CorrelationCookie.IsEssential = true;
        });
}

var app = builder.Build();

// Log trạng thái sử dụng repository
var repositoryLogger = app.Services.GetRequiredService<ILogger<Program>>();
repositoryLogger.LogInformation("=== REPOSITORY CONFIGURATION ===");
repositoryLogger.LogInformation("UseMockData: {UseMockData}", useMockData);
repositoryLogger.LogInformation("Repository Mode: {Mode}", useMockData ? "MOCK DATA" : "REAL DATABASE");
repositoryLogger.LogInformation("================================");

// Seed roles và admin user
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        
        await Unicareer.Data.DbInitializer.SeedRolesAsync(roleManager);
        await Unicareer.Data.DbInitializer.SeedAdminUserAsync(userManager);
        
        // Đảm bảo tài khoản admin luôn tồn tại sau mỗi lần khởi động
        var logger = services.GetRequiredService<ILogger<Program>>();
        var adminUser = await userManager.FindByEmailAsync("admin@unicareer.vn");
        if (adminUser != null)
        {
            logger.LogInformation("Primary Admin account verified: {Email}", adminUser.Email);
        }
        else
        {
            logger.LogWarning("Primary Admin account not found! Attempting to recreate...");
            await Unicareer.Data.DbInitializer.SeedAdminUserAsync(userManager);
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Có lỗi xảy ra khi seed database");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

// Sử dụng Session (phải đặt trước UseRouting)
app.UseSession();

// Middleware bảo mật cho admin login - Log và rate limiting
app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value?.ToLower() ?? "";
    
    // Kiểm tra nếu là đường dẫn admin login
    if (path.Contains("/admin/login") || path.Contains("/admin/logout"))
    {
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
        var userAgent = context.Request.Headers["User-Agent"].ToString();
        
        // Log mọi lần truy cập admin login
        logger.LogWarning("=== ADMIN LOGIN ACCESS ATTEMPT ===");
        logger.LogWarning("IP: {IP}, Path: {Path}, Method: {Method}, UserAgent: {UserAgent}, Time: {Time}",
            ipAddress, path, context.Request.Method, userAgent, DateTime.UtcNow);
        
        // Rate limiting đơn giản bằng session
        var sessionKey = $"admin_login_attempt_{ipAddress}";
        var attempts = context.Session.GetInt32(sessionKey) ?? 0;
        
        if (context.Request.Method == "POST" && attempts >= 5)
        {
            logger.LogError("BLOCKED: Too many login attempts from IP: {IP}", ipAddress);
            context.Response.StatusCode = 429; // Too Many Requests
            await context.Response.WriteAsync("Quá nhiều lần thử đăng nhập. Vui lòng thử lại sau 15 phút.");
            return;
        }
        
        if (context.Request.Method == "POST")
        {
            context.Session.SetInt32(sessionKey, attempts + 1);
            context.Session.SetString($"{sessionKey}_time", DateTime.UtcNow.ToString());
        }
    }
    
    await next();
});

app.UseRouting();

// DEBUG: Middleware để log request và cookie trước khi vào authentication
app.Use(async (context, next) =>
{
    if (context.Request.Path.StartsWithSegments("/signin-google") || 
        context.Request.Path.StartsWithSegments("/Account/GoogleCallback") ||
        context.Request.Path.StartsWithSegments("/Account/ExternalLoginCallback"))
    {
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        logger.LogInformation("=== MIDDLEWARE: GoogleCallback Request (BEFORE AUTHENTICATION) ===");
        logger.LogInformation("Method: {Method}, Path: {Path}, QueryString: {QueryString}", 
            context.Request.Method, context.Request.Path, context.Request.QueryString);
        logger.LogInformation("Scheme: {Scheme}, Host: {Host}", context.Request.Scheme, context.Request.Host);
        logger.LogInformation("Cookies Count: {Count}", context.Request.Cookies.Count);
        
        // Log tất cả cookies
        foreach (var cookie in context.Request.Cookies)
        {
            var cookieValue = cookie.Value ?? string.Empty;
            if (cookie.Key.Contains("Identity") || cookie.Key.Contains("Correlation") || cookie.Key.Contains("Google") || cookie.Key.Contains("state"))
            {
                var preview = cookieValue.Length > 150 ? cookieValue.Substring(0, 150) : cookieValue;
                logger.LogInformation("OAuth Cookie Found: {Name} = {Value}... (length: {Length})", 
                    cookie.Key, 
                    preview,
                    cookieValue.Length);
            }
            else
            {
                logger.LogDebug("Other Cookie: {Name} (length: {Length})", cookie.Key, cookieValue.Length);
            }
        }
        
        // Log headers
        logger.LogInformation("Referer: {Referer}", context.Request.Headers["Referer"].ToString());
        logger.LogInformation("User-Agent: {UserAgent}", context.Request.Headers["User-Agent"].ToString());
    }
    
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        if (context.Request.Path.StartsWithSegments("/signin-google") || 
            context.Request.Path.StartsWithSegments("/Account/GoogleCallback") ||
            context.Request.Path.StartsWithSegments("/Account/ExternalLoginCallback"))
        {
            var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "Exception in OAuth callback middleware");
        }
        throw;
    }
});

app.UseHttpsRedirection();

// Cấu hình Cookie Policy - đặt TRƯỚC UseAuthentication
// Trong Development, cho phép Lax để hoạt động với localhost
// Trong Production, cần None với Secure
if (app.Environment.IsDevelopment())
{
    app.UseCookiePolicy(new CookiePolicyOptions
    {
        MinimumSameSitePolicy = SameSiteMode.Lax,
        Secure = CookieSecurePolicy.SameAsRequest
    });
}
else
{
    app.UseCookiePolicy(new CookiePolicyOptions
    {
        MinimumSameSitePolicy = SameSiteMode.None,
        Secure = CookieSecurePolicy.Always
    });
}

// DEBUG: Log authentication errors
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Microsoft.AspNetCore.Authentication.AuthenticationFailureException ex)
    {
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "=== AUTHENTICATION FAILURE EXCEPTION ===");
        logger.LogError("Path: {Path}, QueryString: {QueryString}", context.Request.Path, context.Request.QueryString);
        logger.LogError("Exception Message: {Message}", ex.Message);
        if (ex.InnerException != null)
        {
            logger.LogError("Inner Exception: {InnerMessage}", ex.InnerException.Message);
        }
        throw; // Re-throw để exception handler xử lý
    }
});

app.UseAuthentication();
app.UseAuthorization();

// Route riêng cho admin login (phải đặt trước route areas)
app.MapControllerRoute(
    name: "adminLogin",
    pattern: "admin/login",
    defaults: new { area = "Admin", controller = "AdminAccount", action = "Login" });

app.MapControllerRoute(
    name: "adminLogout",
    pattern: "admin/logout",
    defaults: new { area = "Admin", controller = "AdminAccount", action = "Logout" });

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Admin}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
