using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Unicareer.Models;
using Unicareer.Repository;

namespace Unicareer.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{SD.Role_Admin}")]
    public class AdminController : Controller
    {
        private readonly INhaTuyenDungRepository _nhaTuyenDungRepository;
        private readonly IUngVienRepository _ungVienRepository;
        private readonly ITinTuyenDungRepository _tinTuyenDungRepository;
        private readonly ITinUngTuyenRepository _tinUngTuyenRepository;
        private readonly ILoaiCongViecRepository _loaiCongViecRepository;
        private readonly INganhNgheRepository _nganhNgheRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(INhaTuyenDungRepository nhaTuyenDungRepository, IUngVienRepository ungVienRepository, ITinTuyenDungRepository tinTuyenDungRepository, ITinUngTuyenRepository tinUngTuyenRepository, ILoaiCongViecRepository loaiCongViecRepository, INganhNgheRepository nganhNgheRepository, UserManager<ApplicationUser> userManager)
        {
            _nhaTuyenDungRepository = nhaTuyenDungRepository;
            _ungVienRepository = ungVienRepository;
            _tinTuyenDungRepository = tinTuyenDungRepository;
            _tinUngTuyenRepository = tinUngTuyenRepository;
            _loaiCongViecRepository = loaiCongViecRepository;
            _nganhNgheRepository = nganhNgheRepository;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Dashboard()
        {
            var danhSachNhaTuyenDung = _nhaTuyenDungRepository.LayDanhSachNhaTuyenDung();
            var danhSachUngVien = _ungVienRepository.LayDanhSachUngVien();
            var danhSachTinTuyenDung = _tinTuyenDungRepository.LayDanhSachTinTuyenDung();
            var danhSachTinUngTuyen = _tinUngTuyenRepository.LayDanhSachTinUngTuyen();
            var danhSachLoaiCongViec = _loaiCongViecRepository.LayDanhSachLoaiCongViec();
            var danhSachNganhNghe = _nganhNgheRepository.LayDanhSachNganhNghe();
            ViewBag.DanhSachNhaTuyenDung = danhSachNhaTuyenDung;
            ViewBag.DanhSachUngVien = danhSachUngVien;
            ViewBag.DanhSachTinTuyenDung = danhSachTinTuyenDung;
            ViewBag.DanhSachTinUngTuyen = danhSachTinUngTuyen;
            ViewBag.DanhSachLoaiCongViec = danhSachLoaiCongViec;
            ViewBag.DanhSachNganhNghe = danhSachNganhNghe;
            return View();
        }

        public IActionResult UngVien()
        {
            var danhSachUngVien = _ungVienRepository.LayDanhSachUngVien();
            return View(danhSachUngVien);
        }

        public IActionResult NhaTuyenDung()
        {
            var danhSachNhaTuyenDung = _nhaTuyenDungRepository.LayDanhSachNhaTuyenDung();
            return View(danhSachNhaTuyenDung);
        }

        public async Task<IActionResult> Settings()
        {
            ViewData["Title"] = "Cài đặt";
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                ViewBag.User = user;
            }
            var model = new Models.ViewModels.ChangePasswordViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(string hoTen, string soDienThoai)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy người dùng" });
                }

                user.HoTen = hoTen;
                user.PhoneNumber = soDienThoai;

                var result = await _userManager.UpdateAsync(user);
                
                if (result.Succeeded)
                {
                    return Json(new { success = true, message = "Cập nhật thông tin thành công!" });
                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return Json(new { success = false, message = errors });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        public IActionResult NganhNghe()
        {
            var danhSachNganhNghe = _nganhNgheRepository.LayDanhSachNganhNghe();
            return View(danhSachNganhNghe);
        }

        public IActionResult LoaiCongViec()
        {
            var danhSachLoaiCongViec = _loaiCongViecRepository.LayDanhSachLoaiCongViec();
            return View(danhSachLoaiCongViec);
        }

        public IActionResult TinTuyenDung()
        {
            var danhSachTinTuyenDung = _tinTuyenDungRepository.LayDanhSachTinTuyenDung();
            return View(danhSachTinTuyenDung);
        }

        public IActionResult TinUngTuyen()
        {
            var danhSachTinUngTuyen = _tinUngTuyenRepository.LayDanhSachTinUngTuyen();
            return View(danhSachTinUngTuyen);
        }

        // GET: Chi tiết tin tuyển dụng
        public IActionResult ChiTietTinTuyenDung(int id)
        {
            ViewData["Title"] = "Chi tiết tin tuyển dụng";
            
            var tinTuyenDung = _tinTuyenDungRepository.LayTinTuyenDungTheoId(id);
            if (tinTuyenDung == null)
            {
                TempData["Loi"] = "Không tìm thấy tin tuyển dụng!";
                return RedirectToAction("TinTuyenDung");
            }
            
            // Lấy danh sách ứng tuyển theo mã tin
            var danhSachUngTuyen = _tinUngTuyenRepository.LayDanhSachTinUngTuyen()
                .Where(t => t.MaTinTuyenDung == id.ToString())
                .ToList();
            
            ViewBag.DanhSachUngTuyen = danhSachUngTuyen;
            
            return View(tinTuyenDung);
        }

        // POST: Duyệt tin tuyển dụng
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DuyetTinTuyenDung(int id)
        {
            try
            {
                var tin = _tinTuyenDungRepository.LayTinTuyenDungTheoId(id);
                if (tin == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy tin tuyển dụng!" });
                }

                // TODO: Cập nhật trạng thái duyệt trong database
                // Ví dụ: tin.TrangThaiDuyet = "Da duyet";
                // _tinTuyenDungRepository.CapNhatTinTuyenDung(id, tin);

                return Json(new { success = true, message = "Đã duyệt tin tuyển dụng thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        // POST: Duyệt hàng loạt
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DuyetHangLoat(List<int> danhSachMa)
        {
            try
            {
                if (danhSachMa == null || danhSachMa.Count == 0)
                {
                    return Json(new { success = false, message = "Vui lòng chọn ít nhất một tin!" });
                }

                int count = 0;
                foreach (var ma in danhSachMa)
                {
                    var tin = _tinTuyenDungRepository.LayTinTuyenDungTheoId(ma);
                    if (tin != null)
                    {
                        // TODO: Cập nhật trạng thái duyệt
                        // tin.TrangThaiDuyet = "Da duyet";
                        // _tinTuyenDungRepository.CapNhatTinTuyenDung(ma, tin);
                        count++;
                    }
                }

                return Json(new { success = true, count = count, message = $"Đã duyệt {count} tin thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        // POST: Khóa tin tuyển dụng
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult KhoaTinTuyenDung(int id, string lyDo)
        {
            try
            {
                var tin = _tinTuyenDungRepository.LayTinTuyenDungTheoId(id);
                if (tin == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy tin tuyển dụng!" });
                }

                if (string.IsNullOrWhiteSpace(lyDo))
                {
                    return Json(new { success = false, message = "Vui lòng nhập lý do khóa!" });
                }

                // TODO: Cập nhật trạng thái khóa trong database
                // Ví dụ: tin.TrangThaiDuyet = "Bi khoa";
                // tin.LyDoKhoa = lyDo;
                // _tinTuyenDungRepository.CapNhatTinTuyenDung(id, tin);

                return Json(new { success = true, message = "Đã khóa tin tuyển dụng thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        // GET: Lấy danh sách ứng viên theo mã tin
        [HttpGet]
        public IActionResult LayDanhSachUngVien(int maTin)
        {
            try
            {
                var danhSachUngTuyen = _tinUngTuyenRepository.LayDanhSachTinUngTuyen()
                    .Where(t => t.MaTinTuyenDung == maTin.ToString())
                    .Select(t => new
                    {
                        hoTen = t.HoTen,
                        email = t.Email,
                        soDienThoai = t.SoDienThoai,
                        viTriUngTuyen = t.ViTriUngTuyen,
                        trangThaiXuLy = t.TrangThaiXuLy,
                        linkCV = t.LinkCV,
                        ngayUngTuyen = t.NgayUngTuyen.ToString("dd/MM/yyyy")
                    })
                    .ToList();

                return Json(new { success = true, data = danhSachUngTuyen });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        // POST: Xóa tin tuyển dụng
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult XoaTinTuyenDung(int id)
        {
            try
            {
                var tin = _tinTuyenDungRepository.LayTinTuyenDungTheoId(id);
                if (tin == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy tin tuyển dụng!" });
                }

                // TODO: Xóa tin trong database
                // _tinTuyenDungRepository.XoaTinTuyenDung(id);

                return Json(new { success = true, message = "Đã xóa tin tuyển dụng thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        // GET: Xuất Excel
        public IActionResult XuatExcel()
        {
            try
            {
                var danhSachTinTuyenDung = _tinTuyenDungRepository.LayDanhSachTinTuyenDung();
                
                // TODO: Tạo file Excel và trả về
                // Có thể sử dụng thư viện như EPPlus, ClosedXML, hoặc NPOI
                // Tạm thời trả về JSON
                return Json(new { success = true, message = "Tính năng xuất Excel đang được phát triển!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }
    }
}

