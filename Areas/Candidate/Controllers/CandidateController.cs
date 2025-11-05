using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unicareer.Models;

namespace Unicareer.Areas.Candidate.Controllers
{
    [Area("Candidate")]
    [Authorize(Roles = $"{SD.Role_UngVien}")]
    public class CandidateController : Controller
    {
        // GET: Trang chu ung vien
        public IActionResult Index()
        {
            ViewData["Title"] = "Trang chủ";
            return View();
        }

        // GET: Danh sach tin ung tuyen
        public IActionResult QuanLyTinUngTuyen()
        {
            ViewData["Title"] = "Quản lý tin ứng tuyển";
            // TODO: Lay tin ung tuyen theo ung vien dang nhap
            var danhSach = TinUngTuyen.LayDanhSachTinUngTuyen();
            return View(danhSach);
        }

        // GET: Chi tiet tin ung tuyen
        public IActionResult ChiTietTinUngTuyen(int id)
        {
            ViewData["Title"] = "Chi tiết tin ứng tuyển";
            var danhSach = TinUngTuyen.LayDanhSachTinUngTuyen();
            var tin = danhSach.FirstOrDefault(t => t.MaTinUngTuyen == id);
            
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
            var danhSach = TinTuyenDung.LayDanhSachTinTuyenDung();
            return View(danhSach);
        }
    }
}
