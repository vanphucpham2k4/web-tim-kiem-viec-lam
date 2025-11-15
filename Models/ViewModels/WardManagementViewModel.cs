using Unicareer.Models;

namespace Unicareer.Models.ViewModels
{
    public class WardManagementViewModel
    {
        public List<Ward> Wards { get; set; } = new List<Ward>();
        public List<Province> Provinces { get; set; } = new List<Province>();
        public string? SearchTerm { get; set; }
        public string? SelectedProvinceCode { get; set; }
        
        // Phân trang
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 50;
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}

