namespace Unicareer.Models
{
    public class TheLoaiBlog
    {
        public int MaTheLoai { get; set; }
        public string TenTheLoai { get; set; } = string.Empty;
        public bool HienThi { get; set; } = true;
        public DateTime NgayTao { get; set; }
        public int ThuTu { get; set; } = 0;
    }
}

