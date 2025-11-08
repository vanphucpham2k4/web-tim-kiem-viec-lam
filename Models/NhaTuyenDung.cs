namespace Unicareer.Models
{
    public class NhaTuyenDung
    {
        public int MaNhaTuyenDung { get; set; }
        public string UserId { get; set; } = string.Empty; // Foreign key đến ApplicationUser
        public ApplicationUser? User { get; set; } // Navigation property
        public string TenCongTy { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string SoDienThoai { get; set; } = string.Empty;
        public string? DiaChi { get; set; }
        public string? TinhThanhPho { get; set; }
        public string? QuanHuyen { get; set; }
        public string? Website { get; set; }
        public string? NguoiDaiDien { get; set; }
        public string? ChucVu { get; set; }
        public string? LinhVuc { get; set; }
        public string? MoTa { get; set; }
        public string? Logo { get; set; }
        public int SoTinDaDang { get; set; }
        public int SoUngVienNhan { get; set; }
        public DateTime NgayDangKy { get; set; }
    }
}

