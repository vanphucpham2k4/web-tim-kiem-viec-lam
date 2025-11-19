using Unicareer.Models;

namespace Unicareer.Models.ViewModels
{
    public class TinDaDangViewModel
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
        
        // Stats từ toàn bộ dữ liệu (không phân trang)
        public int TotalDangTuyen { get; set; }
        public int TotalHetHan { get; set; }
        public int TotalUngTuyen { get; set; }
    }
}

