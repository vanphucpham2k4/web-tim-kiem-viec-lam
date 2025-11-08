namespace Unicareer.Models
{
    public class UngVien
    {
        public int MaUngVien { get; set; }
        public string UserId { get; set; } = string.Empty; // Foreign key đến ApplicationUser
        public ApplicationUser? User { get; set; } // Navigation property
        public string HoTen { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string SoDienThoai { get; set; } = string.Empty;
        public DateTime NgaySinh { get; set; } = new DateTime(2000, 1, 1);
        public string? GioiTinh { get; set; }
        public string? DiaChi { get; set; }
        public string? HocVan { get; set; }
        public string? KinhNghiem { get; set; }
        public string? KyNang { get; set; }
        public string? NganhNghe { get; set; }
        public string? LinkCV { get; set; }
        public int SoLanUngTuyen { get; set; }
        public DateTime NgayDangKy { get; set; }
    }
}

