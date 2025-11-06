using Microsoft.AspNetCore.Identity;
using Unicareer.Models;

namespace Unicareer.Data
{
    public class DbInitializer
    {
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            // Tạo các role nếu chưa tồn tại
            if (!await roleManager.RoleExistsAsync(SD.Role_Admin))
            {
                await roleManager.CreateAsync(new IdentityRole(SD.Role_Admin));
            }

            if (!await roleManager.RoleExistsAsync(SD.Role_NhaTuyenDung))
            {
                await roleManager.CreateAsync(new IdentityRole(SD.Role_NhaTuyenDung));
            }

            if (!await roleManager.RoleExistsAsync(SD.Role_UngVien))
            {
                await roleManager.CreateAsync(new IdentityRole(SD.Role_UngVien));
            }
        }

        public static async Task SeedAdminUserAsync(UserManager<ApplicationUser> userManager)
        {
            // Tạo tài khoản admin mặc định nếu chưa có
            // Tài khoản này là tài khoản đầu tiên, không được xóa/sửa (trừ mật khẩu)
            var adminEmail = "admin@unicareer.vn";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var admin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    HoTen = "Administrator",
                    PhoneNumber = "0900000000",
                    LoaiTaiKhoan = SD.Role_Admin,
                    NgayDangKy = DateTime.Now,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(admin, "Admin@123");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, SD.Role_Admin);
                }
            }
            else
            {
                // Đảm bảo tài khoản admin luôn có role Admin
                if (!await userManager.IsInRoleAsync(adminUser, SD.Role_Admin))
                {
                    await userManager.AddToRoleAsync(adminUser, SD.Role_Admin);
                }
            }
        }

        // Helper method để kiểm tra xem user có phải là admin đầu tiên không
        public static bool IsPrimaryAdmin(ApplicationUser user)
        {
            return user != null && user.Email?.ToLower() == "admin@unicareer.vn";
        }
    }
}

