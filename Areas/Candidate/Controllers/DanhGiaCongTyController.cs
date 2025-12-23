using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Unicareer.Models;
using Unicareer.Models.Enums;
using Unicareer.Repository;
using Unicareer.Data;

namespace Unicareer.Areas.Candidate.Controllers
{
    [Area("Candidate")]
    [Authorize(Roles = $"{SD.Role_UngVien}")]
    public class DanhGiaCongTyController : Controller
    {
        private readonly IDanhGiaCongTyRepository _danhGiaRepository;
        private readonly ITinUngTuyenRepository _tinUngTuyenRepository;
        private readonly INhaTuyenDungRepository _nhaTuyenDungRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public DanhGiaCongTyController(
            IDanhGiaCongTyRepository danhGiaRepository,
            ITinUngTuyenRepository tinUngTuyenRepository,
            INhaTuyenDungRepository nhaTuyenDungRepository,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            _danhGiaRepository = danhGiaRepository;
            _tinUngTuyenRepository = tinUngTuyenRepository;
            _nhaTuyenDungRepository = nhaTuyenDungRepository;
            _userManager = userManager;
            _context = context;
        }

        /// <summary>
        /// Kiểm tra xem có thể đánh giá đơn ứng tuyển này không
        /// </summary>
        [HttpGet]
        public IActionResult KiemTraCoTheDanhGia(int maTinUngTuyen)
        {
            var user = _userManager.GetUserAsync(User).Result;
            if (user == null)
                return Json(new { canReview = false, message = "Vui lòng đăng nhập" });

            var tinUngTuyen = _tinUngTuyenRepository.LayTinUngTuyenTheoId(maTinUngTuyen);
            if (tinUngTuyen == null)
                return Json(new { canReview = false, message = "Không tìm thấy đơn ứng tuyển" });

            // Kiểm tra quyền sở hữu
            if (tinUngTuyen.UserId != user.Id)
                return Json(new { canReview = false, message = "Bạn không có quyền đánh giá đơn này" });

            // Kiểm tra đã đánh giá chưa
            if (_danhGiaRepository.DaDanhGia(maTinUngTuyen))
            {
                var danhGia = _danhGiaRepository.LayDanhGiaTheoMaTinUngTuyen(maTinUngTuyen);
                return Json(new { 
                    canReview = false, 
                    alreadyReviewed = true,
                    maDanhGia = danhGia?.MaDanhGia,
                    message = "Bạn đã đánh giá đơn này rồi" 
                });
            }

            // Kiểm tra điều kiện hiển thị nút review
            var canReview = KiemTraDieuKienDanhGia(tinUngTuyen);
            return Json(new { 
                canReview = canReview, 
                message = canReview ? "Bạn có thể đánh giá" : "Chưa đủ điều kiện để đánh giá" 
            });
        }

        /// <summary>
        /// Lấy thông tin đơn ứng tuyển để hiển thị trong form đánh giá
        /// </summary>
        [HttpGet]
        public IActionResult LayThongTinDonUngTuyen(int maTinUngTuyen)
        {
            var user = _userManager.GetUserAsync(User).Result;
            if (user == null)
                return Json(new { success = false, message = "Vui lòng đăng nhập" });

            var tinUngTuyen = _tinUngTuyenRepository.LayTinUngTuyenTheoId(maTinUngTuyen);
            if (tinUngTuyen == null)
                return Json(new { success = false, message = "Không tìm thấy đơn ứng tuyển" });

            if (tinUngTuyen.UserId != user.Id)
                return Json(new { success = false, message = "Bạn không có quyền" });

            // Lấy thông tin nhà tuyển dụng
            var nhaTuyenDung = _context.NhaTuyenDungs
                .FirstOrDefault(n => n.TenCongTy == tinUngTuyen.CongTy);
            
            return Json(new { 
                success = true,
                maTinUngTuyen = tinUngTuyen.MaTinUngTuyen,
                viTriUngTuyen = tinUngTuyen.ViTriUngTuyen,
                congTy = tinUngTuyen.CongTy,
                maNhaTuyenDung = nhaTuyenDung?.MaNhaTuyenDung ?? 0
            });
        }

        /// <summary>
        /// Tạo đánh giá mới
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TaoDanhGia(
            int maTinUngTuyen,
            int diemMinhBachLuong,
            int diemTocDoPhanHoi,
            int diemTonTrongUngVien,
            string? tags,
            string? noiDung,
            bool isAnDanh = true)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Json(new { success = false, message = "Vui lòng đăng nhập" });

            // Validate
            if (diemMinhBachLuong < 1 || diemMinhBachLuong > 5 ||
                diemTocDoPhanHoi < 1 || diemTocDoPhanHoi > 5 ||
                diemTonTrongUngVien < 1 || diemTonTrongUngVien > 5)
            {
                return Json(new { success = false, message = "Điểm đánh giá phải từ 1-5" });
            }

            var tinUngTuyen = _tinUngTuyenRepository.LayTinUngTuyenTheoId(maTinUngTuyen);
            if (tinUngTuyen == null)
                return Json(new { success = false, message = "Không tìm thấy đơn ứng tuyển" });

            if (tinUngTuyen.UserId != user.Id)
                return Json(new { success = false, message = "Bạn không có quyền đánh giá đơn này" });

            // Kiểm tra đã đánh giá chưa
            if (_danhGiaRepository.DaDanhGia(maTinUngTuyen))
                return Json(new { success = false, message = "Bạn đã đánh giá đơn này rồi" });

            // Kiểm tra điều kiện
            if (!KiemTraDieuKienDanhGia(tinUngTuyen))
                return Json(new { success = false, message = "Chưa đủ điều kiện để đánh giá" });

            // Lấy mã nhà tuyển dụng
            var nhaTuyenDung = _context.NhaTuyenDungs
                .FirstOrDefault(n => n.TenCongTy == tinUngTuyen.CongTy);
            
            if (nhaTuyenDung == null)
                return Json(new { success = false, message = "Không tìm thấy thông tin công ty" });

            // Tự động duyệt (không cần chờ duyệt nữa)
            var danhGia = new DanhGiaCongTy
            {
                MaTinUngTuyen = maTinUngTuyen,
                UserId = user.Id,
                MaNhaTuyenDung = nhaTuyenDung.MaNhaTuyenDung,
                DiemMinhBachLuong = diemMinhBachLuong,
                DiemTocDoPhanHoi = diemTocDoPhanHoi,
                DiemTonTrongUngVien = diemTonTrongUngVien,
                Tags = tags,
                NoiDung = noiDung,
                IsAnDanh = isAnDanh,
                TrangThai = "Da duyet" // Tự động duyệt
            };

            _danhGiaRepository.ThemDanhGia(danhGia);

            return Json(new { success = true, message = "Đánh giá của bạn đã được gửi thành công!" });
        }

        /// <summary>
        /// Cập nhật đánh giá (chỉ trong 24h)
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CapNhatDanhGia(
            int maDanhGia,
            int diemMinhBachLuong,
            int diemTocDoPhanHoi,
            int diemTonTrongUngVien,
            string? tags,
            string? noiDung,
            bool isAnDanh = true)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Json(new { success = false, message = "Vui lòng đăng nhập" });

            var danhGia = _danhGiaRepository.LayDanhGiaTheoId(maDanhGia);
            if (danhGia == null)
                return Json(new { success = false, message = "Không tìm thấy đánh giá" });

            if (danhGia.UserId != user.Id)
                return Json(new { success = false, message = "Bạn không có quyền sửa đánh giá này" });

            // Kiểm tra thời gian (24h)
            if ((DateTime.Now - danhGia.NgayTao).TotalHours > 24)
                return Json(new { success = false, message = "Chỉ có thể sửa đánh giá trong vòng 24 giờ" });

            danhGia.DiemMinhBachLuong = diemMinhBachLuong;
            danhGia.DiemTocDoPhanHoi = diemTocDoPhanHoi;
            danhGia.DiemTonTrongUngVien = diemTonTrongUngVien;
            danhGia.Tags = tags;
            danhGia.NoiDung = noiDung;
            danhGia.IsAnDanh = isAnDanh;

            var result = _danhGiaRepository.CapNhatDanhGia(danhGia);
            if (result == null)
                return Json(new { success = false, message = "Không thể cập nhật đánh giá" });

            return Json(new { success = true, message = "Đã cập nhật đánh giá thành công!" });
        }

        /// <summary>
        /// Xóa đánh giá (chỉ trong 24h)
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> XoaDanhGia(int maDanhGia)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Json(new { success = false, message = "Vui lòng đăng nhập" });

            var danhGia = _danhGiaRepository.LayDanhGiaTheoId(maDanhGia);
            if (danhGia == null)
                return Json(new { success = false, message = "Không tìm thấy đánh giá" });

            if (danhGia.UserId != user.Id)
                return Json(new { success = false, message = "Bạn không có quyền xóa đánh giá này" });

            var result = _danhGiaRepository.XoaDanhGia(maDanhGia);
            if (!result)
                return Json(new { success = false, message = "Chỉ có thể xóa đánh giá trong vòng 24 giờ" });

            return Json(new { success = true, message = "Đã xóa đánh giá thành công!" });
        }

        /// <summary>
        /// Lấy đánh giá của user cho đơn ứng tuyển
        /// </summary>
        [HttpGet]
        public IActionResult LayDanhGiaCuaToi(int maTinUngTuyen)
        {
            var user = _userManager.GetUserAsync(User).Result;
            if (user == null)
                return Json(new { success = false });

            var danhGia = _danhGiaRepository.LayDanhGiaTheoMaTinUngTuyen(maTinUngTuyen);
            if (danhGia == null || danhGia.UserId != user.Id)
                return Json(new { success = false });

            return Json(new { 
                success = true,
                maDanhGia = danhGia.MaDanhGia,
                diemMinhBachLuong = danhGia.DiemMinhBachLuong,
                diemTocDoPhanHoi = danhGia.DiemTocDoPhanHoi,
                diemTonTrongUngVien = danhGia.DiemTonTrongUngVien,
                tags = danhGia.Tags,
                noiDung = danhGia.NoiDung,
                isAnDanh = danhGia.IsAnDanh,
                canEdit = (DateTime.Now - danhGia.NgayTao).TotalHours <= 24
            });
        }

        /// <summary>
        /// Tăng lượt thích cho đánh giá (yêu cầu đăng nhập)
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TangLuotThich(int maDanhGia)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Json(new { success = false, message = "Vui lòng đăng nhập để thích đánh giá" });
            }

            var danhGia = _danhGiaRepository.LayDanhGiaTheoId(maDanhGia);
            if (danhGia == null)
            {
                return Json(new { success = false, message = "Không tìm thấy đánh giá" });
            }

            // Kiểm tra user có phải là người đăng review không
            if (danhGia.UserId == user.Id)
            {
                return Json(new { success = false, message = "Bạn không thể thích đánh giá của chính mình" });
            }

            // Kiểm tra đã like chưa
            if (_danhGiaRepository.DaLike(maDanhGia, user.Id))
            {
                return Json(new { success = false, message = "Bạn đã thích đánh giá này rồi" });
            }

            var result = _danhGiaRepository.TangLuotThich(maDanhGia, user.Id);
            if (result)
            {
                return Json(new { success = true, message = "Đã thích đánh giá" });
            }
            return Json(new { success = false, message = "Không thể thích đánh giá" });
        }

        /// <summary>
        /// Helper: Kiểm tra điều kiện hiển thị nút review
        /// </summary>
        private bool KiemTraDieuKienDanhGia(TinUngTuyen tinUngTuyen)
        {
            var trangThaiEnum = TrangThaiXuLyHelper.FromString(tinUngTuyen.TrangThaiXuLy);
            
            // Điều kiện 1: Trạng thái thuộc các nhóm cho phép
            if (trangThaiEnum.HasValue)
            {
                var trangThai = trangThaiEnum.Value;
                if (trangThai == TrangThaiXuLy.DaPhongVan ||
                    trangThai == TrangThaiXuLy.TuyenDung ||
                    trangThai == TrangThaiXuLy.TuChoi)
                {
                    return true;
                }
            }

            // Điều kiện 2: Đơn đã nộp quá 14 ngày mà vẫn là "ChoDuyet" (ghosting)
            if (trangThaiEnum == TrangThaiXuLy.DangXemXet)
            {
                var soNgay = (DateTime.Now - tinUngTuyen.NgayUngTuyen).TotalDays;
                if (soNgay >= 14)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
