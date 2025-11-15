namespace Unicareer.Models
{
    public class TinUngTuyen
    {
        public int MaTinUngTuyen { get; set; }
        public string? UserId { get; set; } // Foreign key đến ApplicationUser (nullable để tương thích với dữ liệu cũ)
        public ApplicationUser? User { get; set; } // Navigation property
        public string HoTen { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string SoDienThoai { get; set; } = string.Empty;
        public string ViTriUngTuyen { get; set; } = string.Empty;
        public string CongTy { get; set; } = string.Empty;
        public string MaTinTuyenDung { get; set; } = string.Empty;
        public string TrangThaiXuLy { get; set; } = string.Empty;
        public string LinkCV { get; set; } = string.Empty;
        public string GhiChu { get; set; } = string.Empty;
        public DateTime NgayUngTuyen { get; set; }
    }
}

