using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Unicareer.Models;
using Unicareer.Repository;
using Unicareer.Data;
using Microsoft.EntityFrameworkCore;

namespace Unicareer.Controllers
{
    [AllowAnonymous]
    public class PublicProfileController : Controller
    {
        private readonly IUngVienRepository _ungVienRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public PublicProfileController(
            IUngVienRepository ungVienRepository,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            _ungVienRepository = ungVienRepository;
            _userManager = userManager;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> ThongTinCaNhan(int? id)
        {
            ViewData["Title"] = "Thông tin cá nhân";
            
            if (!id.HasValue)
            {
                return BadRequest("Thiếu tham số id");
            }

            var ungVien = _context.UngViens
                .Include(u => u.User)
                .Include(u => u.ChuyenNganh)
                    .ThenInclude(c => c!.NganhNghe)
                .FirstOrDefault(u => u.MaUngVien == id.Value);

            if (ungVien == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            bool isOwner = currentUser != null && ungVien.UserId == currentUser.Id;
            bool isRecruiter = currentUser != null && (User.IsInRole(SD.Role_NhaTuyenDung) || User.IsInRole(SD.Role_Admin));
            bool canView = false;

            if (isOwner)
            {
                canView = true;
            }
            else if (isRecruiter)
            {
                canView = true;
            }
            else
            {
                if (!ungVien.HienThiCongKhai)
                {
                    TempData["Loi"] = "Ứng viên này không cho phép hiển thị hồ sơ công khai.";
                    return Redirect("https://" + Request.Host + "/");
                }
                
                if (currentUser == null)
                {
                    TempData["Loi"] = "Bạn cần đăng nhập để xem hồ sơ này.";
                    var loginUrl = Url.Action("Login", "Account", new { area = "", returnUrl = Request.Path + Request.QueryString });
                    return Redirect(loginUrl ?? "/Account/Login");
                }
                
                canView = true;
            }

            if (!canView)
            {
                return Forbid();
            }

            ViewBag.IsOwner = isOwner;
            
            bool emailConfirmed = false;
            if (ungVien.User != null)
            {
                emailConfirmed = ungVien.User.EmailConfirmed;
            }
            else if (ungVien.UserId != null)
            {
                var user = await _userManager.FindByIdAsync(ungVien.UserId);
                if (user != null)
                {
                    emailConfirmed = user.EmailConfirmed;
                }
            }
            ViewBag.EmailConfirmed = emailConfirmed;
            
            return View(ungVien);
        }
    }
}

