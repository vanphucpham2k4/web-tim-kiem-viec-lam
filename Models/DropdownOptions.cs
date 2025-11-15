namespace Unicareer.Models
{
    /// <summary>
    /// Static class chứa các giá trị dropdown cho form tuyển dụng
    /// </summary>
    public static class DropdownOptions
    {
        /// <summary>
        /// Danh sách các vị trí công việc
        /// </summary>
        public static readonly List<string> ViTri = new List<string>
        {
            "Nhân viên",
            "Trưởng nhóm",
            "Quản lý",
            "Phó giám đốc",
            "Giám đốc",
            "Trợ lý",
            "Thực tập sinh"
        };

        /// <summary>
        /// Danh sách các mức kinh nghiệm làm việc
        /// </summary>
        public static readonly List<string> KinhNghiem = new List<string>
        {
            "Không yêu cầu",
            "Dưới 1 năm",
            "1-2 năm",
            "2-5 năm",
            "Trên 5 năm"
        };

        /// <summary>
        /// Danh sách các ngoại ngữ
        /// </summary>
        public static readonly List<string> NgoaiNgu = new List<string>
        {
            "Không yêu cầu",
            "Tiếng Anh",
            "Tiếng Nhật",
            "Tiếng Hàn",
            "Tiếng Trung",
            "Tiếng Pháp",
            "Tiếng Đức"
        };
    }
}

