namespace Unicareer.Models
{
    public class LoaiCongViec
    {
        public int MaLoaiCongViec { get; set; }
        public string TenLoaiCongViec { get; set; } = string.Empty;
        public string MoTa { get; set; } = string.Empty;
        public int SoLuongViTri { get; set; }
        public DateTime NgayTao { get; set; }
    }
}

