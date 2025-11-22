using Unicareer.Models;

namespace Unicareer.Models.ViewModels
{
    public class UngVienManagementViewModel
    {
        public List<UngVien> UngViens { get; set; } = new List<UngVien>();
        
        // Phân trang
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        
        // Tìm kiếm và lọc
        public string? SearchTerm { get; set; }
        public string? NganhNgheFilter { get; set; }
        public string? KinhNghiemFilter { get; set; }
        
        // Danh sách cho dropdown
        public List<NganhNghe> DanhSachNganhNghe { get; set; } = new List<NganhNghe>();
        public List<string> DanhSachKinhNghiem { get; set; } = new List<string>();
        
        // Stats từ toàn bộ dữ liệu (không phân trang)
        public int TotalUngVien { get; set; }
        public int TotalUngVienIT { get; set; }
        public int TotalUngTuyen { get; set; }
        public int TrungBinhUngTuyenPerUngVien { get; set; }
    }
}

