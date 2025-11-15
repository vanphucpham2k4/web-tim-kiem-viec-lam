using Microsoft.AspNetCore.Identity;

namespace Unicareer.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? HoTen { get; set; }
        public string? LoaiTaiKhoan { get; set; } // "UngVien" hoặc "NhaTuyenDung"
        public string? Avatar { get; set; } // Đường dẫn đến file avatar
        public DateTime NgayDangKy { get; set; }
    }
}

