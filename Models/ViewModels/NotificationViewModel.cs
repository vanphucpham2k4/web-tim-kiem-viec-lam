namespace Unicareer.Models.ViewModels
{
    /// <summary>
    /// ViewModel chung cho thông báo, được sử dụng bởi cả Nhà tuyển dụng và Ứng viên
    /// </summary>
    public class NotificationViewModel
    {
        /// <summary>
        /// Tổng số thông báo
        /// </summary>
        public int TotalCount { get; set; }
        
        /// <summary>
        /// Danh sách thông báo
        /// </summary>
        public List<NotificationItem> Notifications { get; set; } = new List<NotificationItem>();
    }
}

