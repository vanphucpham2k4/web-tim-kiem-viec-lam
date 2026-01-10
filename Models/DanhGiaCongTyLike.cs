namespace Unicareer.Models
{
    /// <summary>
    /// Model lưu trữ lượt thích của user cho review
    /// </summary>
    public class DanhGiaCongTyLike
    {
        public int MaLike { get; set; }
        
        /// <summary>
        /// Mã đánh giá được like
        /// </summary>
        public int MaDanhGia { get; set; }
        public DanhGiaCongTy? DanhGiaCongTy { get; set; }
        
        /// <summary>
        /// UserId của người like
        /// </summary>
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }
        
        /// <summary>
        /// Ngày like
        /// </summary>
        public DateTime NgayLike { get; set; } = DateTime.Now;
    }
}
