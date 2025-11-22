namespace Unicareer.Models
{
    public class TheLoaiBlog
    {
        public int MaTheLoai { get; set; }
        public string TenTheLoai { get; set; } = string.Empty;
        public string? MoTa { get; set; }
        public string? Icon { get; set; } // Icon class (ví dụ: "bi bi-briefcase")
        public string? MauSac { get; set; } // Màu sắc cho badge (ví dụ: "#10b981")
        public int SoLuongBlog { get; set; } = 0;
        public bool HienThi { get; set; } = true;
        public DateTime NgayTao { get; set; }
        public int ThuTu { get; set; } = 0; // Thứ tự hiển thị
    }
}

