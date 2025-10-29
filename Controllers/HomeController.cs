using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Unicareer.Models;

namespace Unicareer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult TimViec()
        {
            var danhSachTinTuyenDung = TinTuyenDung.LayDanhSachTinTuyenDung();
            return View(danhSachTinTuyenDung);
        }

        public IActionResult ThucTap()
        {
            var danhSachThucTap = TinTuyenDung.LayDanhSachThucTap();
            return View(danhSachThucTap);
        }

        public IActionResult CongTy()
        {
            var danhSachCongTy = NhaTuyenDung.LayDanhSachNhaTuyenDung();
            return View(danhSachCongTy);
        }

        public IActionResult ChiTietCongTy(int id)
        {
            var congTy = NhaTuyenDung.LayNhaTuyenDungTheoId(id);
            if (congTy == null)
            {
                return NotFound();
            }

            // Lấy danh sách công việc của công ty
            var danhSachViecLam = TinTuyenDung.LayDanhSachTheoCongTy(congTy.TenCongTy);
            
            ViewBag.DanhSachViecLam = danhSachViecLam;
            return View(congTy);
        }

        public IActionResult ChiTietViecLam(int id)
        {
            var tinTuyenDung = TinTuyenDung.LayTinTuyenDungTheoId(id);
            if (tinTuyenDung == null)
            {
                return NotFound();
            }
            return View(tinTuyenDung);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
