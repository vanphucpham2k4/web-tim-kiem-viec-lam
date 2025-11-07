using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Unicareer.Models;
using Unicareer.Repository;

namespace Unicareer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INhaTuyenDungRepository _nhaTuyenDungRepository;
        private readonly ITinTuyenDungRepository _tinTuyenDungRepository;

        public HomeController(ILogger<HomeController> logger, INhaTuyenDungRepository nhaTuyenDungRepository, ITinTuyenDungRepository tinTuyenDungRepository)
        {
            _logger = logger;
            _nhaTuyenDungRepository = nhaTuyenDungRepository;
            _tinTuyenDungRepository = tinTuyenDungRepository;
        }

        public IActionResult Index()
        {
            // Lấy danh sách việc làm nổi bật (top 9)
            var danhSachViecLam = _tinTuyenDungRepository.LayDanhSachTinTuyenDung()
                .OrderByDescending(j => j.SoLuongUngTuyen)
                .ThenByDescending(j => j.NgayDang)
                .Take(9)
                .ToList();
            
            ViewBag.DanhSachViecLamNoiBat = danhSachViecLam;
            return View();
        }

        public IActionResult TimViec()
        {
            var danhSachTinTuyenDung = _tinTuyenDungRepository.LayDanhSachTinTuyenDung();
            
            // Đếm số lượng việc làm theo tỉnh/thành phố và sắp xếp từ cao đến thấp, lấy top 5
            var soLuongViecLamTheoTinh = danhSachTinTuyenDung
                .GroupBy(job => job.TinhThanhPho)
                .Select(g => new { TinhThanhPho = g.Key, SoLuong = g.Count() })
                .OrderByDescending(x => x.SoLuong)
                .Take(5)
                .ToList();
            
            ViewBag.SoLuongViecLamTheoTinh = soLuongViecLamTheoTinh;
            
            return View(danhSachTinTuyenDung);
        }

        public IActionResult ThucTap()
        {
            var danhSachThucTap = _tinTuyenDungRepository.LayDanhSachThucTap();
            return View(danhSachThucTap);
        }

        public IActionResult CongTy()
        {
            var danhSachCongTy = _nhaTuyenDungRepository.LayDanhSachNhaTuyenDung();
            return View(danhSachCongTy);
        }

        public IActionResult ChiTietCongTy(int id)
        {
            var congTy = _nhaTuyenDungRepository.LayNhaTuyenDungTheoId(id);
            if (congTy == null)
            {
                return NotFound();
            }

            // Lấy danh sách công việc của công ty
            var danhSachViecLam = _tinTuyenDungRepository.LayDanhSachTheoCongTy(congTy.TenCongTy);
            
            ViewBag.DanhSachViecLam = danhSachViecLam;
            return View(congTy);
        }

        public IActionResult ChiTietViecLam(int id)
        {
            var tinTuyenDung = _tinTuyenDungRepository.LayTinTuyenDungTheoId(id);
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
