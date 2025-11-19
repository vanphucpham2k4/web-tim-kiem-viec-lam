using Unicareer.Models;

namespace Unicareer.Models.ViewModels
{
    public class DonUngTuyenViewModel
    {
        public List<TinUngTuyen> DonUngTuyens { get; set; } = new List<TinUngTuyen>();
        public List<TinTuyenDung> DanhSachTinTuyenDung { get; set; } = new List<TinTuyenDung>();
        
        // Phân trang
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        
        // Tìm kiếm và lọc
        public string? SearchTerm { get; set; }
        public string? TrangThaiFilter { get; set; }
        public string? TinTuyenDungFilter { get; set; }
        
        // Stats từ toàn bộ dữ liệu (không phân trang)
        public int TotalDangXemXet { get; set; }
        public int TotalChoPhongVan { get; set; }
        public int TotalTuyenDung { get; set; }
    }
}

