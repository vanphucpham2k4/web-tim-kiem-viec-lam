using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Unicareer.Models;
using Unicareer.Repository;
using Unicareer.Data;
using Microsoft.EntityFrameworkCore;

namespace Unicareer.Areas.Candidate.Controllers
{
    [Area("Candidate")]
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

            System.Diagnostics.Debug.WriteLine($"PublicProfileController.ThongTinCaNhan called with id={id}");

            var ungVien = _context.UngViens
                .Include(u => u.User)
                .Include(u => u.ChuyenNganh)
                    .ThenInclude(c => c!.NganhNghe)
                .FirstOrDefault(u => u.MaUngVien == id.Value);

            if (ungVien == null)
            {
                System.Diagnostics.Debug.WriteLine($"UngVien not found with id={id}");
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            bool isOwner = currentUser != null && ungVien.UserId == currentUser.Id;
            bool isRecruiter = currentUser != null && (User.IsInRole(SD.Role_NhaTuyenDung) || User.IsInRole(SD.Role_Admin));
            bool canView = false;

            System.Diagnostics.Debug.WriteLine($"isOwner={isOwner}, isRecruiter={isRecruiter}, HienThiCongKhai={ungVien.HienThiCongKhai}, currentUser={currentUser?.Email}");

            if (isOwner)
            {
                canView = true;
                System.Diagnostics.Debug.WriteLine("User is owner, allowing view");
            }
            else if (isRecruiter)
            {
                canView = true;
                System.Diagnostics.Debug.WriteLine("User is recruiter/admin, allowing view");
            }
            else
            {
                if (!ungVien.HienThiCongKhai)
                {
                    System.Diagnostics.Debug.WriteLine("HienThiCongKhai is false and user is not recruiter, redirecting");
                    TempData["Loi"] = "Ứng viên này không cho phép hiển thị hồ sơ công khai.";
                    return Redirect("https://" + Request.Host + "/");
                }
                
                if (currentUser == null)
                {
                    System.Diagnostics.Debug.WriteLine("User not logged in, redirecting to login");
                    TempData["Loi"] = "Bạn cần đăng nhập để xem hồ sơ này.";
                    var loginUrl = Url.Action("Login", "Account", new { area = "", returnUrl = Request.Path + Request.QueryString });
                    return Redirect(loginUrl ?? "/Account/Login");
                }
                
                canView = true;
                System.Diagnostics.Debug.WriteLine("User is logged in and HienThiCongKhai is true, allowing view");
            }

            if (!canView)
            {
                System.Diagnostics.Debug.WriteLine("Forbidding access");
                return Forbid();
            }

            System.Diagnostics.Debug.WriteLine("Returning view");

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
            
            return View("~/Areas/Candidate/Views/Candidate/ThongTinCaNhan.cshtml", ungVien);
        }
    }
}

