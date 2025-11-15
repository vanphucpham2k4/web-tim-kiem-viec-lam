namespace Unicareer.Models
{
    public class TinTuyenDung
    {
        public int MaTinTuyenDung { get; set; }
        public string? TenViecLam { get; set; } = string.Empty; // Changed from TieuDe
        public string? CongTy { get; set; } = string.Empty; // Ten cong ty (lien ket voi NhaTuyenDung)
        
        // Foreign key đến NhaTuyenDung
        public int? MaNhaTuyenDung { get; set; }
        public NhaTuyenDung? NhaTuyenDung { get; set; } // Navigation property
        
        // Mo ta cong viec
        public string? NganhNghe { get; set; } = string.Empty;
        public string? NganhNgheChiTiet { get; set; } = string.Empty;
        public string? LoaiCongViec { get; set; } = string.Empty;
        public string? KinhNghiem { get; set; } = string.Empty;
        public string? ViTri { get; set; } = string.Empty;
        public string? NgoaiNgu { get; set; } = string.Empty;
        public string? TuKhoa { get; set; } = string.Empty;
        public string? KyNang { get; set; } = string.Empty;
        public string? MoTa { get; set; } = string.Empty;
        
        // Yeu cau cong viec
        public string? YeuCau { get; set; } = string.Empty;
        
        // Quyen loi
        public decimal? MucLuongThapNhat { get; set; }
        public decimal? MucLuongCaoNhat { get; set; }
        public string? QuyenLoi { get; set; } = string.Empty;
        
        // Thong tin lien he
        public string? NguoiLienHe { get; set; } = string.Empty;
        public string? EmailLienHe { get; set; } = string.Empty;
        public string? SDTLienHe { get; set; } = string.Empty;
        public string? TinhThanhPho { get; set; } = string.Empty;
        public string? PhuongXa { get; set; } = string.Empty;
        public string? DiaChiLamViec { get; set; } = string.Empty;
        
        // Anh van phong
        public string? AnhVanPhong { get; set; } = string.Empty;
        
        // Other fields
        public int? SoLuongUngTuyen { get; set; }
        public DateTime NgayDang { get; set; }
        public DateTime HanNop { get; set; }
        public string? TrangThai { get; set; } = "Dang tuyen"; // Trạng thái: "Dang tuyen", "Het han", "Da dong"
        
        // Geolocation
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}
