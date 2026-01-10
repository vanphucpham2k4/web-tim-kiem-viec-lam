namespace Unicareer.Models
{
    public class ChuyenNganh
    {
        public int MaChuyenNganh { get; set; }
        public string TenChuyenNganh { get; set; } = string.Empty;
        public string? MoTa { get; set; }
        public int? MaNganhNghe { get; set; } // Foreign key đến NganhNghe (nullable để cho phép xóa ngành nghề)
        public NganhNghe? NganhNghe { get; set; } // Navigation property
        public DateTime NgayTao { get; set; }
        public bool IsActive { get; set; } = true; // Để admin có thể ẩn/hiện chuyên ngành
    }
}

