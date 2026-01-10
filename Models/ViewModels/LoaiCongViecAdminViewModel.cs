using Unicareer.Models;

namespace Unicareer.Models.ViewModels
{
    public class LoaiCongViecAdminViewModel
    {
        public List<LoaiCongViec> LoaiCongViecs { get; set; } = new List<LoaiCongViec>();
        
        // Phân trang
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        
        // Tìm kiếm
        public string? SearchTerm { get; set; }
        
        // Stats từ toàn bộ dữ liệu (không phân trang)
        public int TotalLoaiCongViec { get; set; }
        public int TotalViTri { get; set; }
        public int TrungBinhViTri { get; set; }
    }
}

