using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unicareer.Models;

namespace Unicareer.Areas.Recruiter.Controllers
{
    [Area("Recruiter")]
    [Authorize(Roles = $"{SD.Role_NhaTuyenDung}")]
    public class RecruiterController : Controller
    {
        // GET: Trang chu nha tuyen dung
        public IActionResult Index()
        {
            return View();
        }

        // GET: Form dang tin tuyen dung
        public IActionResult DangTinTuyenDung()
        {
            ViewData["Title"] = "Dang tuyen dung";
            return View();
        }

        // POST: Xu ly dang tin tuyen dung
        [HttpPost]
        public IActionResult DangTinTuyenDung(TinTuyenDung model)
        {
            if (ModelState.IsValid)
            {
                // TODO: Luu vao database
                TempData["ThanhCong"] = "Dang tin tuyen dung thanh cong!";
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: Danh sach tin da dang
        public IActionResult TinDaDang()
        {
            ViewData["Title"] = "Tin da dang";
            // TODO: Lay tin theo nha tuyen dung dang nhap
            var danhSach = TinTuyenDung.LayDanhSachTinTuyenDung();
            return View(danhSach);
        }
    }
}

