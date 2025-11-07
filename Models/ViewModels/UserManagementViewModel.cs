using Microsoft.AspNetCore.Identity;
using Unicareer.Models;

namespace Unicareer.Models.ViewModels
{
    public class UserManagementViewModel
    {
        public List<UserInfo> Users { get; set; } = new List<UserInfo>();
        public string? SearchTerm { get; set; }
        public string? RoleFilter { get; set; }
        public List<string> AvailableRoles { get; set; } = new List<string>();
    }

    public class UserInfo
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? HoTen { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime NgayDangKy { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
        public bool IsLockedOut { get; set; }
        public DateTime? LockoutEnd { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool IsPrimaryAdmin { get; set; }
    }
}

