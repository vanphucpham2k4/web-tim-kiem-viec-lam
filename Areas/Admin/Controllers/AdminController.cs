using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unicareer.Models;

namespace Unicareer.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{SD.Role_Admin}")]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult UngVien()
        {
            var danhSachUngVien = Models.UngVien.LayDanhSachUngVien();
            return View(danhSachUngVien);
        }

        public IActionResult NhaTuyenDung()
        {
            var danhSachNhaTuyenDung = Models.NhaTuyenDung.LayDanhSachNhaTuyenDung();
            return View(danhSachNhaTuyenDung);
        }

        public IActionResult Settings()
        {
            return View();
        }

        public IActionResult NganhNghe()
        {
            var danhSachNganhNghe = Models.NganhNghe.LayDanhSachNganhNghe();
            return View(danhSachNganhNghe);
        }

        public IActionResult LoaiCongViec()
        {
            var danhSachLoaiCongViec = Models.LoaiCongViec.LayDanhSachLoaiCongViec();
            return View(danhSachLoaiCongViec);
        }

        public IActionResult TinTuyenDung()
        {
            var danhSachTinTuyenDung = Models.TinTuyenDung.LayDanhSachTinTuyenDung();
            return View(danhSachTinTuyenDung);
        }

        public IActionResult TinUngTuyen()
        {
            var danhSachTinUngTuyen = Models.TinUngTuyen.LayDanhSachTinUngTuyen();
            return View(danhSachTinUngTuyen);
        }
    }
}

