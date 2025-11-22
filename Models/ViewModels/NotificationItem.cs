namespace Unicareer.Models.ViewModels
{
    /// <summary>
    /// Class chung cho thông báo, được sử dụng bởi cả Nhà tuyển dụng và Ứng viên
    /// </summary>
    public class NotificationItem
    {
        /// <summary>
        /// Loại thông báo (NewApplication, ExpiringSoon, Expired, PendingReview, StatusChanged, InterviewScheduled, Accepted, Rejected, JobExpiring, JobExpired)
        /// </summary>
        public string Type { get; set; } = string.Empty;
        
        /// <summary>
        /// Tiêu đề thông báo
        /// </summary>
        public string Title { get; set; } = string.Empty;
        
        /// <summary>
        /// Nội dung thông báo
        /// </summary>
        public string Message { get; set; } = string.Empty;
        
        /// <summary>
        /// Icon Bootstrap Icons (ví dụ: bi-bell, bi-calendar-check-fill)
        /// </summary>
        public string Icon { get; set; } = string.Empty;
        
        /// <summary>
        /// Màu sắc CSS class (ví dụ: text-primary, text-warning, text-danger)
        /// </summary>
        public string Color { get; set; } = string.Empty;
        
        /// <summary>
        /// URL để điều hướng khi click vào thông báo
        /// </summary>
        public string Url { get; set; } = string.Empty;
        
        /// <summary>
        /// Thời gian tạo thông báo
        /// </summary>
        public DateTime CreatedAt { get; set; }
        
        /// <summary>
        /// ID liên quan (có thể là MaTinUngTuyen, MaTinTuyenDung, v.v.)
        /// </summary>
        public int? RelatedId { get; set; }
        
        /// <summary>
        /// Đánh dấu thông báo đã đọc hay chưa
        /// </summary>
        public bool IsRead { get; set; } = false;
        
        /// <summary>
        /// Key duy nhất để xác định thông báo (dùng cho localStorage)
        /// </summary>
        public string NotificationKey { get; set; } = string.Empty;
    }
}

