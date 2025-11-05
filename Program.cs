using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Extensions;
using Unicareer.Data;
using Unicareer.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

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

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Admin}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
