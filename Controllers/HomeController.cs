using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Unicareer.Models;
using Unicareer.Repository;
using Unicareer.Data;
using Microsoft.EntityFrameworkCore;

namespace Unicareer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INhaTuyenDungRepository _nhaTuyenDungRepository;
        private readonly ITinTuyenDungRepository _tinTuyenDungRepository;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, INhaTuyenDungRepository nhaTuyenDungRepository, ITinTuyenDungRepository tinTuyenDungRepository, ApplicationDbContext context)
        {
            _logger = logger;
            _nhaTuyenDungRepository = nhaTuyenDungRepository;
            _tinTuyenDungRepository = tinTuyenDungRepository;
            _context = context;
        }

        public IActionResult Index()
        {
            // Lấy danh sách việc làm nổi bật (top 9)
            var danhSachViecLam = _tinTuyenDungRepository.LayDanhSachTinTuyenDung()
                .OrderByDescending(j => j.SoLuongUngTuyen)
                .ThenByDescending(j => j.NgayDang)
                .Take(9)
                .ToList();
            
            // Lấy danh sách tỉnh/thành phố
            var danhSachTinhThanh = _context.Provinces
                .OrderBy(p => p.FullName)
                .ToList();
            
            ViewBag.DanhSachViecLamNoiBat = danhSachViecLam;
            ViewBag.DanhSachTinhThanh = danhSachTinhThanh;
            return View();
        }

        [HttpGet]
        public IActionResult GetWardsByProvince(string provinceCode)
        {
            if (string.IsNullOrEmpty(provinceCode))
            {
                return Json(new List<object>());
            }

            var wards = _context.Wards
                .Where(w => w.ProvinceCode == provinceCode && !string.IsNullOrEmpty(w.FullName))
                .OrderBy(w => w.FullName)
                .Select(w => new
                {
                    code = w.Code,
                    fullName = w.FullName
                })
                .ToList();

            return Json(wards);
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
            
            // Lấy danh sách tỉnh/thành phố
            var danhSachTinhThanh = _context.Provinces
                .OrderBy(p => p.FullName)
                .ToList();
            
            ViewBag.SoLuongViecLamTheoTinh = soLuongViecLamTheoTinh;
            ViewBag.DanhSachTinhThanh = danhSachTinhThanh;
            
            return View(danhSachTinTuyenDung);
        }

        public IActionResult ThucTap()
        {
            var danhSachThucTap = _tinTuyenDungRepository.LayDanhSachThucTap();
            
            // Lấy danh sách tỉnh/thành phố
            var danhSachTinhThanh = _context.Provinces
                .OrderBy(p => p.FullName)
                .ToList();
            
            ViewBag.DanhSachTinhThanh = danhSachTinhThanh;
            return View(danhSachThucTap);
        }

        public IActionResult CongTy()
        {
            var danhSachCongTy = _nhaTuyenDungRepository.LayDanhSachNhaTuyenDung();
            
            // Lấy danh sách tỉnh/thành phố
            var danhSachTinhThanh = _context.Provinces
                .OrderBy(p => p.FullName)
                .ToList();
            
            ViewBag.DanhSachTinhThanh = danhSachTinhThanh;
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
