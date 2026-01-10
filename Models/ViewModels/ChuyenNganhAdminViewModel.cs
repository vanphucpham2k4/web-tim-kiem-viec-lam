using Unicareer.Models;

namespace Unicareer.Models.ViewModels
{
    public class ChuyenNganhAdminViewModel
    {
        public List<ChuyenNganh> ChuyenNganhs { get; set; } = new List<ChuyenNganh>();
        public List<NganhNghe> DanhSachNganhNghe { get; set; } = new List<NganhNghe>();
        
        // Phân trang
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        
        // Tìm kiếm và lọc
        public string? SearchTerm { get; set; }
        public int? MaNganhNgheFilter { get; set; }
        
        // Stats từ toàn bộ dữ liệu (không phân trang)
        public int TotalChuyenNganh { get; set; }
    }
}

