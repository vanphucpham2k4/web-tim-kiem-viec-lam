using Microsoft.AspNetCore.Identity;

namespace Unicareer.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? HoTen { get; set; }
        public string? LoaiTaiKhoan { get; set; } // "UngVien" hoặc "NhaTuyenDung"
        public DateTime NgayDangKy { get; set; }
    }
}

