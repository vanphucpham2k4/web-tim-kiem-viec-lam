using Unicareer.Models;

namespace Unicareer.Models.ViewModels
{
    public class ProvinceManagementViewModel
    {
        public List<Province> Provinces { get; set; } = new List<Province>();
        public string? SearchTerm { get; set; }
        
        // Phân trang
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}

