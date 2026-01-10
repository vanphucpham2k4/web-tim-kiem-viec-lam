using Unicareer.Models;

namespace Unicareer.Models.ViewModels
{
    public class TruongDaiHocAdminViewModel
    {
        public List<TruongDaiHoc> TruongDaiHocs { get; set; } = new List<TruongDaiHoc>();
        
        // Phân trang
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        
        // Tìm kiếm
        public string? SearchTerm { get; set; }
        
        // Stats từ toàn bộ dữ liệu (không phân trang)
        public int TotalTruongDaiHoc { get; set; }
    }
}

