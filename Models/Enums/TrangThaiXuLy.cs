namespace Unicareer.Models.Enums
{
    /// <summary>
    /// Enum trạng thái xử lý đơn ứng tuyển
    /// </summary>
    public enum TrangThaiXuLy
    {
        /// <summary>
        /// Đang xem xét
        /// </summary>
        DangXemXet = 1,

        /// <summary>
        /// Chờ phỏng vấn
        /// </summary>
        ChoPhongVan = 2,

        /// <summary>
        /// Đã phỏng vấn
        /// </summary>
        DaPhongVan = 3,

        /// <summary>
        /// Tuyển dụng
        /// </summary>
        TuyenDung = 4,

        /// <summary>
        /// Từ chối
        /// </summary>
        TuChoi = 5,

        /// <summary>
        /// Không phù hợp
        /// </summary>
        KhongPhuHop = 6
    }
}

