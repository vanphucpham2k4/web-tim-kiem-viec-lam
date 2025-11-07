using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Unicareer.Models;
using Unicareer.Models.ViewModels;
using Unicareer.Repository;

namespace Unicareer.Areas.Candidate.Controllers
{
    [Area("Candidate")]
    [Authorize(Roles = $"{SD.Role_UngVien}")]
    public class CandidateController : Controller
    {
        private readonly ITinTuyenDungRepository _tinTuyenDungRepository;
        private readonly ITinUngTuyenRepository _tinUngTuyenRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public CandidateController(
            ITinTuyenDungRepository tinTuyenDungRepository, 
            ITinUngTuyenRepository tinUngTuyenRepository,
            UserManager<ApplicationUser> userManager)
        {
            _tinTuyenDungRepository = tinTuyenDungRepository;
            _tinUngTuyenRepository = tinUngTuyenRepository;
            _userManager = userManager;
        }
        // GET: Trang chu ung vien
        public IActionResult Index()
        {
            ViewData["Title"] = "Trang chủ";
            // TODO: Lay tin ung tuyen theo ung vien dang nhap
            var danhSachTinUngTuyen = _tinUngTuyenRepository.LayDanhSachTinUngTuyen();
            var danhSachViecLamDaLuu = _tinTuyenDungRepository.LayDanhSachTinTuyenDung(); // TODO: Lay theo ung vien dang nhap
            
            // Thống kê
            var tongTinUngTuyen = danhSachTinUngTuyen.Count;
            var dangXemXet = danhSachTinUngTuyen.Count(t => t.TrangThaiXuLy == "Dang xem xet");
            var choPhongVan = danhSachTinUngTuyen.Count(t => t.TrangThaiXuLy == "Cho phong van");
            var daPhongVan = danhSachTinUngTuyen.Count(t => t.TrangThaiXuLy == "Da phong van");
            var tongViecDaLuu = danhSachViecLamDaLuu.Count; // TODO: Lay theo ung vien dang nhap
            
            ViewBag.DanhSachTinUngTuyenGanDay = danhSachTinUngTuyen.OrderByDescending(t => t.NgayUngTuyen).Take(3).ToList();
            ViewBag.TongTinUngTuyen = tongTinUngTuyen;
            ViewBag.DangXemXet = dangXemXet;
            ViewBag.ChoPhongVan = choPhongVan;
            ViewBag.DaPhongVan = daPhongVan;
            ViewBag.TongViecDaLuu = tongViecDaLuu;
            
            return View();
        }

        // GET: Danh sach tin ung tuyen
        public IActionResult QuanLyTinUngTuyen()
        {
            ViewData["Title"] = "Quản lý tin ứng tuyển";
            // TODO: Lay tin ung tuyen theo ung vien dang nhap
            var danhSach = _tinUngTuyenRepository.LayDanhSachTinUngTuyen();
            return View(danhSach);
        }

        // GET: Chi tiet tin ung tuyen
        public IActionResult ChiTietTinUngTuyen(int id)
        {
            ViewData["Title"] = "Chi tiết tin ứng tuyển";
            var tin = _tinUngTuyenRepository.LayTinUngTuyenTheoId(id);
            
            if (tin == null)
            {
                return NotFound();
            }
            
            return View(tin);
        }

        // GET: Thong tin tai khoan
        public IActionResult ThongTinTaiKhoan()
        {
            ViewData["Title"] = "Thông tin tài khoản";
            return View();
        }

        // GET: CV cua toi
        public IActionResult CVCuaToi()
        {
            ViewData["Title"] = "CV của tôi";
            return View();
        }

        // GET: Viec lam da luu
        public IActionResult ViecLamDaLuu()
        {
            ViewData["Title"] = "Việc làm đã lưu";
            // TODO: Lay danh sach viec lam da luu theo ung vien dang nhap
            var danhSach = _tinTuyenDungRepository.LayDanhSachTinTuyenDung();
            return View(danhSach);
        }

        // POST: Bo luu viec lam
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult BoLuuViecLam(int id)
        {
            try
            {
                // TODO: Implement remove saved job from database
                // Hiện tại chỉ trả về success, sau này sẽ xóa khỏi database
                return Json(new { success = true, message = "Đã bỏ lưu công việc thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        // GET: Cai dat
        public async Task<IActionResult> CaiDat()
        {
            ViewData["Title"] = "Cài đặt";
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Doi mat khau
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DoiMatKhau(ChangePasswordViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                ViewData["Title"] = "Cài đặt";
                ViewBag.ActiveTab = "baomat";
                ViewBag.PasswordModel = model;
                return View("CaiDat", user);
            }

            // Kiểm tra mật khẩu cũ
            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            
            if (result.Succeeded)
            {
                TempData["ThanhCong"] = "Đổi mật khẩu thành công!";
                return RedirectToAction("CaiDat", new { area = "Candidate" });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            ViewData["Title"] = "Cài đặt";
            ViewBag.ActiveTab = "baomat";
            ViewBag.PasswordModel = model;
            return View("CaiDat", user);
        }
    }
}
