namespace Unicareer.Models
{
    /// <summary>
    /// Model đánh giá công ty từ ứng viên
    /// </summary>
    public class DanhGiaCongTy
    {
        public int MaDanhGia { get; set; }
        
        /// <summary>
        /// Mã đơn ứng tuyển - để đảm bảo mỗi đơn chỉ được đánh giá 1 lần
        /// </summary>
        public int MaTinUngTuyen { get; set; }
        public TinUngTuyen? TinUngTuyen { get; set; }
        
        /// <summary>
        /// UserId của ứng viên đánh giá
        /// </summary>
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }
        
        /// <summary>
        /// Mã nhà tuyển dụng được đánh giá
        /// </summary>
        public int MaNhaTuyenDung { get; set; }
        public NhaTuyenDung? NhaTuyenDung { get; set; }
        
        /// <summary>
        /// Điểm minh bạch lương (1-5)
        /// </summary>
        public int DiemMinhBachLuong { get; set; } = 3;
        
        /// <summary>
        /// Điểm tốc độ phản hồi (1-5)
        /// </summary>
        public int DiemTocDoPhanHoi { get; set; } = 3;
        
        /// <summary>
        /// Điểm tôn trọng ứng viên (1-5)
        /// </summary>
        public int DiemTonTrongUngVien { get; set; } = 3;
        
        /// <summary>
        /// Tags được chọn (JSON string: ["tag1", "tag2"])
        /// </summary>
        public string? Tags { get; set; }
        
        /// <summary>
        /// Nội dung đánh giá
        /// </summary>
        public string? NoiDung { get; set; }
        
        /// <summary>
        /// Chế độ ẩn danh (mặc định true)
        /// </summary>
        public bool IsAnDanh { get; set; } = true;
        
        /// <summary>
        /// Trạng thái duyệt: "Cho duyet", "Da duyet", "Tu choi"
        /// </summary>
        public string TrangThai { get; set; } = "Cho duyet";
        
        /// <summary>
        /// Phản hồi từ nhà tuyển dụng
        /// </summary>
        public string? PhanHoi { get; set; }
        
        /// <summary>
        /// Ngày phản hồi từ nhà tuyển dụng
        /// </summary>
        public DateTime? NgayPhanHoi { get; set; }
        
        /// <summary>
        /// Số lượt thích (useful)
        /// </summary>
        public int SoLuotThich { get; set; } = 0;
        
        public DateTime NgayTao { get; set; } = DateTime.Now;
        public DateTime? NgayCapNhat { get; set; }
    }
}
