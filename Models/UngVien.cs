namespace Unicareer.Models
{
    public class UngVien
    {
        public int MaUngVien { get; set; }
        public string HoTen { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string SoDienThoai { get; set; } = string.Empty;
        public DateTime NgaySinh { get; set; }
        public string GioiTinh { get; set; } = string.Empty;
        public string DiaChi { get; set; } = string.Empty;
        public string HocVan { get; set; } = string.Empty;
        public string KinhNghiem { get; set; } = string.Empty;
        public string KyNang { get; set; } = string.Empty;
        public string NganhNghe { get; set; } = string.Empty;
        public string LinkCV { get; set; } = string.Empty;
        public int SoLanUngTuyen { get; set; }
        public DateTime NgayDangKy { get; set; }
    }
}

