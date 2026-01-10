using Unicareer.Models;

namespace Unicareer.Models.ViewModels
{
    public class TinTuyenDungAdminViewModel
    {
        public List<TinTuyenDung> TinTuyenDungs { get; set; } = new List<TinTuyenDung>();
        
        // Phân trang
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        
        // Tìm kiếm và lọc
        public string? SearchTerm { get; set; }
        public string? TrangThaiFilter { get; set; }
        public string? NganhNgheFilter { get; set; }
        
        // Stats từ toàn bộ dữ liệu (không phân trang)
        public int TotalDangTuyen { get; set; }
        public int TotalHetHan { get; set; }
        public int TotalUngTuyen { get; set; }
        public int TotalSapHetHan { get; set; }
        public int TrungBinhUngTuyen { get; set; }
        public int TotalChoDuyet { get; set; }
        public int TotalDaDuyet { get; set; }
        public int TotalTuChoi { get; set; }
    }
}

