namespace Unicareer.Models
{
    public class NhaTuyenDung
    {
        public int MaNhaTuyenDung { get; set; }
        public string TenCongTy { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string SoDienThoai { get; set; } = string.Empty;
        public string DiaChi { get; set; } = string.Empty;
        public string TinhThanhPho { get; set; } = string.Empty;
        public string QuanHuyen { get; set; } = string.Empty;
        public string Website { get; set; } = string.Empty;
        public string NguoiDaiDien { get; set; } = string.Empty;
        public string ChucVu { get; set; } = string.Empty;
        public string LinhVuc { get; set; } = string.Empty;
        public string MoTa { get; set; } = string.Empty;
        public string Logo { get; set; } = string.Empty;
        public int SoTinDaDang { get; set; }
        public int SoUngVienNhan { get; set; }
        public DateTime NgayDangKy { get; set; }
    }
}

