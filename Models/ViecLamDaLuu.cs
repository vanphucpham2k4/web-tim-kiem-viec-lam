namespace Unicareer.Models
{
    public class ViecLamDaLuu
    {
        public int MaViecLamDaLuu { get; set; }
        public string UserId { get; set; } = string.Empty; // Foreign key đến ApplicationUser
        public ApplicationUser? User { get; set; } // Navigation property
        public int MaTinTuyenDung { get; set; } // Foreign key đến TinTuyenDung
        public TinTuyenDung? TinTuyenDung { get; set; } // Navigation property
        public DateTime NgayLuu { get; set; } // Ngày lưu tin
    }
}

