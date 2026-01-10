using Unicareer.Models;

namespace Unicareer.Repository
{
    public interface IDanhGiaCongTyRepository
    {
        /// <summary>
        /// Lấy đánh giá theo ID
        /// </summary>
        DanhGiaCongTy? LayDanhGiaTheoId(int id);
        
        /// <summary>
        /// Lấy đánh giá theo mã đơn ứng tuyển (để kiểm tra đã đánh giá chưa)
        /// </summary>
        DanhGiaCongTy? LayDanhGiaTheoMaTinUngTuyen(int maTinUngTuyen);
        
        /// <summary>
        /// Lấy danh sách đánh giá theo mã nhà tuyển dụng (đã duyệt)
        /// </summary>
        List<DanhGiaCongTy> LayDanhSachDanhGiaTheoNhaTuyenDung(int maNhaTuyenDung, bool chiLayDaDuyet = true);
        
        /// <summary>
        /// Lấy danh sách đánh giá theo UserId (của ứng viên)
        /// </summary>
        List<DanhGiaCongTy> LayDanhSachDanhGiaTheoUserId(string userId);
        
        /// <summary>
        /// Tạo đánh giá mới
        /// </summary>
        DanhGiaCongTy ThemDanhGia(DanhGiaCongTy danhGia);
        
        /// <summary>
        /// Cập nhật đánh giá
        /// </summary>
        DanhGiaCongTy? CapNhatDanhGia(DanhGiaCongTy danhGia);
        
        /// <summary>
        /// Xóa đánh giá (chỉ cho phép trong 24h)
        /// </summary>
        bool XoaDanhGia(int id);
        
        /// <summary>
        /// Thêm phản hồi từ nhà tuyển dụng
        /// </summary>
        bool ThemPhanHoi(int maDanhGia, string phanHoi);
        
        /// <summary>
        /// Tăng số lượt thích (chỉ nếu user chưa like và không phải người đăng review)
        /// </summary>
        bool TangLuotThich(int maDanhGia, string userId);
        
        /// <summary>
        /// Kiểm tra user đã like review chưa
        /// </summary>
        bool DaLike(int maDanhGia, string userId);
        
        /// <summary>
        /// Tính điểm Trust Score trung bình của công ty
        /// </summary>
        double TinhTrustScore(int maNhaTuyenDung);
        
        /// <summary>
        /// Tính tỷ lệ phản hồi trung bình (ngày)
        /// </summary>
        double? TinhTyLePhanHoi(int maNhaTuyenDung);
        
        /// <summary>
        /// Kiểm tra xem đơn ứng tuyển đã được đánh giá chưa
        /// </summary>
        bool DaDanhGia(int maTinUngTuyen);
    }
}
