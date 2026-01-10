using Unicareer.Models;

namespace Unicareer.Models.ViewModels
{
    public class NhaTuyenDungManagementViewModel
    {
        public List<NhaTuyenDung> NhaTuyenDungs { get; set; } = new List<NhaTuyenDung>();
        public List<TinTuyenDung> DanhSachTinTuyenDung { get; set; } = new List<TinTuyenDung>();
        public List<TinUngTuyen> DanhSachTinUngTuyen { get; set; } = new List<TinUngTuyen>();
        public List<Province> DanhSachTinhThanh { get; set; } = new List<Province>();
        
        // Phân trang
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        
        // Tìm kiếm và lọc
        public string? SearchTerm { get; set; }
        public string? TinhThanhPhoFilter { get; set; }
        public string? QuanHuyenFilter { get; set; }
        
        // Stats từ toàn bộ dữ liệu (không phân trang)
        public int TotalNhaTuyenDung { get; set; }
        public int TotalTinDang { get; set; }
        public int TotalUngVienNhan { get; set; }
        public int TrungBinhTinPerNTD { get; set; }
    }
}

