using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Unicareer.Models;
using Unicareer.Models.Enums;
using Unicareer.Models.ViewModels;
using Unicareer.Repository;
using Unicareer.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Unicareer.Areas.Candidate.Controllers
{
    [Area("Candidate")]
    [Authorize(Roles = $"{SD.Role_UngVien}")]
    public class CandidateController : Controller
    {
        private readonly ITinTuyenDungRepository _tinTuyenDungRepository;
        private readonly ITinUngTuyenRepository _tinUngTuyenRepository;
        private readonly IUngVienRepository _ungVienRepository;
        private readonly IChuyenNganhRepository _chuyenNganhRepository;
        private readonly INganhNgheRepository _nganhNgheRepository;
        private readonly IViecLamDaLuuRepository _viecLamDaLuuRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CandidateController(
            ITinTuyenDungRepository tinTuyenDungRepository, 
            ITinUngTuyenRepository tinUngTuyenRepository,
            IUngVienRepository ungVienRepository,
            IChuyenNganhRepository chuyenNganhRepository,
            INganhNgheRepository nganhNgheRepository,
            IViecLamDaLuuRepository viecLamDaLuuRepository,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            IMapper mapper)
        {
            _tinTuyenDungRepository = tinTuyenDungRepository;
            _tinUngTuyenRepository = tinUngTuyenRepository;
            _ungVienRepository = ungVienRepository;
            _chuyenNganhRepository = chuyenNganhRepository;
            _nganhNgheRepository = nganhNgheRepository;
            _viecLamDaLuuRepository = viecLamDaLuuRepository;
            _userManager = userManager;
            _context = context;
            _mapper = mapper;
        }
        // GET: Trang chu ung vien
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Trang chủ";
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            // Lấy tin ứng tuyển theo UserId (đã được sắp xếp theo NgayUngTuyen giảm dần trong repository)
            var danhSachTinUngTuyen = _tinUngTuyenRepository.LayDanhSachTinUngTuyenTheoUserId(user.Id);
            var danhSachViecLamDaLuu = _viecLamDaLuuRepository.LayDanhSachViecLamDaLuuTheoUserId(user.Id);
            var danhSachTinTuyenDung = danhSachViecLamDaLuu
                .Where(v => v.TinTuyenDung != null)
                .Select(v => v.TinTuyenDung!)
                .ToList();
            
            // Thống kê
            var tongTinUngTuyen = danhSachTinUngTuyen.Count;
            var dangXemXet = danhSachTinUngTuyen.Count(t => TrangThaiXuLyHelper.FromString(t.TrangThaiXuLy) == TrangThaiXuLy.DangXemXet);
            var choPhongVan = danhSachTinUngTuyen.Count(t => TrangThaiXuLyHelper.FromString(t.TrangThaiXuLy) == TrangThaiXuLy.ChoPhongVan);
            var daPhongVan = danhSachTinUngTuyen.Count(t => TrangThaiXuLyHelper.FromString(t.TrangThaiXuLy) == TrangThaiXuLy.DaPhongVan);
            var tongViecDaLuu = danhSachTinTuyenDung.Count;
            
            // Lấy 3 tin ứng tuyển gần đây nhất (đã được sắp xếp trong repository)
            ViewBag.DanhSachTinUngTuyenGanDay = danhSachTinUngTuyen.Take(3).ToList();
            ViewBag.TongTinUngTuyen = tongTinUngTuyen;
            ViewBag.DangXemXet = dangXemXet;
            ViewBag.ChoPhongVan = choPhongVan;
            ViewBag.DaPhongVan = daPhongVan;
            ViewBag.TongViecDaLuu = tongViecDaLuu;
            
            return View();
        }

        // GET: Danh sach tin ung tuyen
        public async Task<IActionResult> QuanLyTinUngTuyen()
        {
            ViewData["Title"] = "Quản lý tin ứng tuyển";
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            // Lấy tin ứng tuyển theo UserId
            var danhSach = _tinUngTuyenRepository.LayDanhSachTinUngTuyenTheoUserId(user.Id);
            return View(danhSach);
        }

        // GET: Chi tiet tin ung tuyen
        public async Task<IActionResult> ChiTietTinUngTuyen(int id)
        {
            ViewData["Title"] = "Chi tiết tin ứng tuyển";
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var tin = _tinUngTuyenRepository.LayTinUngTuyenTheoId(id);
            
            if (tin == null)
            {
                return NotFound();
            }

            // Kiểm tra quyền truy cập: chỉ cho phép xem tin ứng tuyển của chính mình
            if (tin.UserId != user.Id)
            {
                return Forbid();
            }
            
            return View(tin);
        }

        // GET: Ho so ung vien
        public async Task<IActionResult> HoSoUngVien()
        {
            ViewData["Title"] = "Hồ sơ ứng viên";
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var ungVien = _ungVienRepository.LayUngVienTheoUserId(user.Id);
            
            // Load dữ liệu cho dropdowns
            var danhSachChuyenNganh = _chuyenNganhRepository.LayDanhSachChuyenNganh();
            var danhSachNganhNghe = _nganhNgheRepository.LayDanhSachNganhNghe();
            var danhSachTinhThanh = _context.Provinces.OrderBy(p => p.FullName).ToList();
            
            ViewBag.DanhSachChuyenNganh = danhSachChuyenNganh;
            ViewBag.DanhSachNganhNghe = danhSachNganhNghe;
            ViewBag.DanhSachTinhThanh = danhSachTinhThanh;
            ViewBag.TrangThaiTimViecOptions = new List<string> { "Đang tìm việc", "Đang thực tập", "Đã có việc" };

            return View(ungVien);
        }

        // POST: Luu ho so ung vien
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LuuHoSoUngVien(
            string hoTen, string gioiTinh, DateTime? ngaySinh,
            string? viTriMongMuon, int? maChuyenNganh, string? chuyenNganhKhac,
            decimal? mucLuongKyVong, string? noiLamViecMongMuon,
            string? mucTieuNgheNghiep, string? hocVanChiTiet,
            string? kinhNghiemChiTiet, string? kyNangChiTiet, string? chungChi,
            string? linkGitHub, string? linkBehance, string? linkPortfolio,
            string? moTaBanThan, string? trangThaiTimViec, IFormFile? cvFile)
        {
            // Xử lý checkbox từ FormData
            var hienThiCongKhaiValue = Request.Form["hienThiCongKhai"].ToString();
            bool hienThiCongKhai = !string.IsNullOrEmpty(hienThiCongKhaiValue) && 
                                   (hienThiCongKhaiValue.ToLower() == "true" || hienThiCongKhaiValue == "on");
            
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Json(new { success = false, message = "Không tìm thấy người dùng" });
            }

            try
            {
                var ungVien = _ungVienRepository.LayUngVienTheoUserId(user.Id);
                
                if (ungVien == null)
                {
                    // Tạo mới hồ sơ ứng viên
                    ungVien = new UngVien
                    {
                        UserId = user.Id,
                        HoTen = hoTen ?? string.Empty,
                        Email = user.Email ?? string.Empty,
                        NgayDangKy = DateTime.Now
                    };
                }

                // Tạo object UngVien tạm thời từ các tham số để sử dụng AutoMapper
                var ungVienNguon = new UngVien
                {
                    HoTen = hoTen ?? ungVien.HoTen,
                    GioiTinh = gioiTinh,
                    NgaySinh = ngaySinh ?? ungVien.NgaySinh,
                    ViTriMongMuon = viTriMongMuon,
                    MaChuyenNganh = maChuyenNganh,
                    ChuyenNganhKhac = chuyenNganhKhac,
                    MucLuongKyVong = mucLuongKyVong,
                    NoiLamViecMongMuon = noiLamViecMongMuon,
                    MucTieuNgheNghiep = mucTieuNgheNghiep,
                    HocVanChiTiet = hocVanChiTiet,
                    KinhNghiemChiTiet = kinhNghiemChiTiet,
                    KyNangChiTiet = kyNangChiTiet,
                    ChungChi = chungChi,
                    LinkGitHub = linkGitHub,
                    LinkBehance = linkBehance,
                    LinkPortfolio = linkPortfolio,
                    MoTaBanThan = moTaBanThan,
                    TrangThaiTimViec = trangThaiTimViec,
                    HienThiCongKhai = hienThiCongKhai
                };

                // Cập nhật thông tin bằng AutoMapper
                // AutoMapper sẽ tự động map các thuộc tính và bỏ qua các thuộc tính đặc biệt
                _mapper.Map(ungVienNguon, ungVien);

                // Upload CV file nếu có
                if (cvFile != null && cvFile.Length > 0)
                {
                    var cvPath = await UploadCVFileAsync(cvFile, user.Id);
                    if (!string.IsNullOrEmpty(cvPath))
                    {
                        // Xóa CV cũ nếu có
                        if (!string.IsNullOrEmpty(ungVien.CVFile) && ungVien.CVFile.StartsWith("/uploads/"))
                        {
                            var oldCvPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", ungVien.CVFile.TrimStart('/'));
                            if (System.IO.File.Exists(oldCvPath))
                            {
                                try
                                {
                                    System.IO.File.Delete(oldCvPath);
                                }
                                catch { }
                            }
                        }
                        ungVien.CVFile = cvPath;
                    }
                }

                // Lưu vào database
                if (ungVien.MaUngVien == 0)
                {
                    _ungVienRepository.ThemUngVien(ungVien);
                }
                else
                {
                    _ungVienRepository.CapNhatUngVien(ungVien);
                }

                return Json(new { success = true, message = "Lưu hồ sơ thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        // Helper method để upload CV file
        private async Task<string?> UploadCVFileAsync(IFormFile? file, string userId)
        {
            if (file == null || file.Length == 0)
                return null;

            // Kiểm tra định dạng file (cho phép PDF, DOC, DOCX)
            var allowedExtensions = new[] { ".pdf", ".doc", ".docx" };
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            
            if (!allowedExtensions.Contains(fileExtension))
            {
                throw new Exception("Định dạng file không hợp lệ. Chỉ chấp nhận PDF, DOC, DOCX.");
            }

            // Kiểm tra kích thước file (tối đa 10MB)
            var maxSize = 10 * 1024 * 1024; // 10MB
            if (file.Length > maxSize)
            {
                throw new Exception("Kích thước file quá lớn. Tối đa 10MB.");
            }

            // Tạo tên file unique
            var fileName = $"CV_{userId}_{DateTime.Now:yyyyMMddHHmmss}_{Guid.NewGuid():N}{fileExtension}";
            var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "cvs");
            
            // Đảm bảo thư mục tồn tại
            if (!Directory.Exists(uploadsPath))
            {
                Directory.CreateDirectory(uploadsPath);
            }

            var filePath = Path.Combine(uploadsPath, fileName);
            
            // Lưu file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Trả về đường dẫn URL
            return $"/uploads/cvs/{fileName}";
        }

        // GET: Viec lam da luu
        public async Task<IActionResult> ViecLamDaLuu()
        {
            ViewData["Title"] = "Việc làm đã lưu";
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var danhSachViecLamDaLuu = _viecLamDaLuuRepository.LayDanhSachViecLamDaLuuTheoUserId(user.Id);
            var danhSachTinTuyenDung = danhSachViecLamDaLuu
                .Where(v => v.TinTuyenDung != null)
                .Select(v => v.TinTuyenDung!)
                .ToList();
            
            return View(danhSachTinTuyenDung);
        }

        // POST: Luu viec lam
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LuuViecLam(int id)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Json(new { success = false, message = "Vui lòng đăng nhập để lưu tin" });
                }

                // Kiểm tra tin tuyển dụng có tồn tại không
                var tinTuyenDung = _tinTuyenDungRepository.LayTinTuyenDungTheoId(id);
                if (tinTuyenDung == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy tin tuyển dụng" });
                }

                // Kiểm tra đã lưu chưa
                if (_viecLamDaLuuRepository.KiemTraDaLuu(user.Id, id))
                {
                    return Json(new { success = false, message = "Bạn đã lưu tin này rồi" });
                }

                // Lưu tin
                _viecLamDaLuuRepository.LuuViecLam(user.Id, id);
                
                return Json(new { success = true, message = "Đã lưu tin tuyển dụng thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        // POST: Bo luu viec lam
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BoLuuViecLam(int id)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Json(new { success = false, message = "Vui lòng đăng nhập" });
                }

                var result = _viecLamDaLuuRepository.BoLuuViecLam(user.Id, id);
                if (result)
                {
                    return Json(new { success = true, message = "Đã bỏ lưu công việc thành công!" });
                }
                else
                {
                    return Json(new { success = false, message = "Không tìm thấy tin đã lưu" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        // GET: Kiem tra da luu chua
        [HttpGet]
        public async Task<IActionResult> KiemTraDaLuu(int id)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Json(new { isSaved = false });
                }

                var isSaved = _viecLamDaLuuRepository.KiemTraDaLuu(user.Id, id);
                return Json(new { isSaved = isSaved });
            }
            catch
            {
                return Json(new { isSaved = false });
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

            // Load thông tin UngVien nếu có để hiển thị trong form
            var ungVien = _ungVienRepository.LayUngVienTheoUserId(user.Id);
            if (ungVien != null)
            {
                // Cập nhật thông tin từ UngVien vào user để hiển thị trong form
                user.PhoneNumber = ungVien.SoDienThoai ?? user.PhoneNumber;
            }

            ViewBag.UngVien = ungVien;
            return View(user);
        }

        // POST: Cap nhat thong tin tai khoan
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CapNhatThongTin(
            string hoTen, string email, string soDienThoai, 
            DateTime? ngaySinh, string gioiTinh, string? diaChi)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Json(new { success = false, message = "Không tìm thấy người dùng" });
            }

            try
            {
                // Validate required fields
                if (string.IsNullOrWhiteSpace(hoTen) || 
                    string.IsNullOrWhiteSpace(email) || 
                    string.IsNullOrWhiteSpace(soDienThoai) || 
                    !ngaySinh.HasValue || 
                    string.IsNullOrWhiteSpace(gioiTinh))
                {
                    return Json(new { success = false, message = "Vui lòng điền đầy đủ các trường bắt buộc" });
                }

                // Validate email format
                if (!email.Contains("@") || !email.Contains("."))
                {
                    return Json(new { success = false, message = "Email không hợp lệ" });
                }

                // Reload user từ database để đảm bảo tracking đúng
                var dbUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
                if (dbUser == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy người dùng trong database" });
                }

                // Cập nhật thông tin ApplicationUser
                dbUser.HoTen = hoTen;
                dbUser.Email = email;
                dbUser.NormalizedEmail = email.ToUpper();
                dbUser.PhoneNumber = soDienThoai;

                // Lưu ApplicationUser vào database
                await _context.SaveChangesAsync();

                // Cập nhật lại user trong UserManager để đồng bộ
                await _userManager.UpdateAsync(dbUser);

                // Cập nhật hoặc tạo UngVien
                var ungVien = _ungVienRepository.LayUngVienTheoUserId(user.Id);
                if (ungVien == null)
                {
                    // Tạo mới UngVien nếu chưa có
                    ungVien = new UngVien
                    {
                        UserId = user.Id,
                        HoTen = hoTen,
                        Email = email,
                        SoDienThoai = soDienThoai,
                        NgaySinh = ngaySinh.Value,
                        GioiTinh = gioiTinh,
                        DiaChi = diaChi,
                        NgayDangKy = DateTime.Now
                    };
                    _ungVienRepository.ThemUngVien(ungVien);
                }
                else
                {
                    // Tạo object UngVien tạm thời từ các tham số để sử dụng AutoMapper
                    var ungVienNguon = new UngVien
                    {
                        HoTen = hoTen,
                        Email = email,
                        SoDienThoai = soDienThoai,
                        NgaySinh = ngaySinh.Value,
                        GioiTinh = gioiTinh,
                        DiaChi = diaChi
                    };

                    // Cập nhật UngVien bằng AutoMapper
                    // AutoMapper sẽ tự động map các thuộc tính và bỏ qua các thuộc tính đặc biệt
                    _mapper.Map(ungVienNguon, ungVien);
                    _ungVienRepository.CapNhatUngVien(ungVien);
                }

                return Json(new { success = true, message = "Cập nhật thông tin thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
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

        // Helper method để xử lý upload file
        private async Task<string?> UploadFileAsync(IFormFile? file, string folder, string userId)
        {
            if (file == null || file.Length == 0)
                return null;

            // Kiểm tra định dạng file (cho phép JPG, PNG, GIF)
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            
            if (!allowedExtensions.Contains(fileExtension))
            {
                throw new Exception("Định dạng file không hợp lệ. Chỉ chấp nhận JPG, PNG, GIF.");
            }

            // Kiểm tra kích thước file (tối đa 5MB)
            var maxSize = 5 * 1024 * 1024; // 5MB
            if (file.Length > maxSize)
            {
                throw new Exception("Kích thước file quá lớn. Tối đa 5MB.");
            }

            // Tạo tên file unique
            var fileName = $"{userId}_{DateTime.Now:yyyyMMddHHmmss}_{Guid.NewGuid():N}{fileExtension}";
            var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", folder);
            
            // Đảm bảo thư mục tồn tại
            if (!Directory.Exists(uploadsPath))
            {
                Directory.CreateDirectory(uploadsPath);
            }

            var filePath = Path.Combine(uploadsPath, fileName);
            
            // Lưu file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Trả về đường dẫn URL
            return $"/uploads/{folder}/{fileName}";
        }

        // POST: Upload avatar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadAvatar(IFormFile? avatarFile)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Json(new { success = false, message = "Không tìm thấy người dùng" });
            }

            try
            {
                if (avatarFile != null && avatarFile.Length > 0)
                {
                    // Upload file mới
                    var avatarPath = await UploadFileAsync(avatarFile, "avatars", user.Id);
                    
                    if (string.IsNullOrEmpty(avatarPath))
                    {
                        return Json(new { success = false, message = "Không thể upload file" });
                    }
                    
                    // Xóa avatar cũ nếu có
                    if (!string.IsNullOrEmpty(user.Avatar) && user.Avatar.StartsWith("/uploads/"))
                    {
                        var oldAvatarPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", user.Avatar.TrimStart('/'));
                        if (System.IO.File.Exists(oldAvatarPath))
                        {
                            try
                            {
                                System.IO.File.Delete(oldAvatarPath);
                            }
                            catch
                            {
                                // Bỏ qua lỗi xóa file cũ
                            }
                        }
                    }
                    
                    // Reload user từ database để đảm bảo tracking đúng
                    var dbUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
                    if (dbUser == null)
                    {
                        return Json(new { success = false, message = "Không tìm thấy người dùng trong database" });
                    }
                    
                    // Cập nhật avatar
                    dbUser.Avatar = avatarPath;
                    
                    // Lưu vào database
                    await _context.SaveChangesAsync();
                    
                    // Cập nhật lại user trong UserManager để đồng bộ
                    await _userManager.UpdateAsync(dbUser);
                    
                    return Json(new { success = true, message = "Cập nhật avatar thành công!", avatarUrl = avatarPath });
                }
                else
                {
                    return Json(new { success = false, message = "Vui lòng chọn file ảnh" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        // GET: Lấy thông báo cho ứng viên
        [HttpGet]
        public async Task<IActionResult> GetNotifications()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy người dùng" });
                }

                var notifications = new List<NotificationItem>();

                // Lấy danh sách tin ứng tuyển của user
                var danhSachTinUngTuyen = _tinUngTuyenRepository.LayDanhSachTinUngTuyenTheoUserId(user.Id);
                var now = DateTime.Now;
                var threeDaysFromNow = now.AddDays(3);

                // 1. Thông báo trạng thái thay đổi (trong 7 ngày gần đây)
                var tinCoThayDoiTrangThai = danhSachTinUngTuyen
                    .Where(t => !string.IsNullOrEmpty(t.TrangThaiXuLy))
                    .OrderByDescending(t => t.NgayUngTuyen)
                    .Take(10)
                    .ToList();

                foreach (var tin in tinCoThayDoiTrangThai)
                {
                    var trangThaiEnum = TrangThaiXuLyHelper.FromString(tin.TrangThaiXuLy);
                    if (!trangThaiEnum.HasValue) continue;

                    var tinTuyenDung = _tinTuyenDungRepository.LayTinTuyenDungTheoId(
                        int.TryParse(tin.MaTinTuyenDung, out int maTin) ? maTin : 0);

                    string title, message, icon, color, type;
                    
                    switch (trangThaiEnum.Value)
                    {
                        case TrangThaiXuLy.ChoPhongVan:
                            type = "InterviewScheduled";
                            title = "Lịch phỏng vấn";
                            message = $"Bạn đã được mời phỏng vấn cho vị trí \"{tinTuyenDung?.TenViecLam ?? tin.ViTriUngTuyen}\" tại {tin.CongTy}";
                            icon = "bi-calendar-check-fill";
                            color = "text-info";
                            break;
                        case TrangThaiXuLy.DaPhongVan:
                            type = "StatusChanged";
                            title = "Đã phỏng vấn";
                            message = $"Đơn ứng tuyển cho vị trí \"{tinTuyenDung?.TenViecLam ?? tin.ViTriUngTuyen}\" đã được phỏng vấn. Đang chờ kết quả.";
                            icon = "bi-check-circle-fill";
                            color = "text-primary";
                            break;
                        case TrangThaiXuLy.TuyenDung:
                            type = "Accepted";
                            title = "Chúc mừng! Bạn đã được tuyển dụng";
                            message = $"Bạn đã được chấp nhận cho vị trí \"{tinTuyenDung?.TenViecLam ?? tin.ViTriUngTuyen}\" tại {tin.CongTy}";
                            icon = "bi-trophy-fill";
                            color = "text-success";
                            break;
                        case TrangThaiXuLy.TuChoi:
                        case TrangThaiXuLy.KhongPhuHop:
                            type = "Rejected";
                            title = "Thông báo kết quả";
                            message = $"Đơn ứng tuyển cho vị trí \"{tinTuyenDung?.TenViecLam ?? tin.ViTriUngTuyen}\" tại {tin.CongTy} đã bị từ chối";
                            icon = "bi-x-circle-fill";
                            color = "text-danger";
                            break;
                        default:
                            type = "StatusChanged";
                            title = "Cập nhật trạng thái";
                            message = $"Đơn ứng tuyển cho vị trí \"{tinTuyenDung?.TenViecLam ?? tin.ViTriUngTuyen}\" đã được cập nhật trạng thái";
                            icon = "bi-info-circle-fill";
                            color = "text-primary";
                            break;
                    }

                    notifications.Add(new NotificationItem
                    {
                        Type = type,
                        Title = title,
                        Message = message,
                        Icon = icon,
                        Color = color,
                        Url = Url.Action("ChiTietTinUngTuyen", "Candidate", new { area = "Candidate", id = tin.MaTinUngTuyen }),
                        CreatedAt = tin.NgayUngTuyen,
                        RelatedId = tin.MaTinUngTuyen,
                        NotificationKey = $"StatusChanged_{tin.MaTinUngTuyen}_{tin.TrangThaiXuLy}_{tin.NgayUngTuyen:yyyyMMddHHmmss}"
                    });
                }

                // 2. Tin tuyển dụng đã lưu sắp hết hạn (trong 3 ngày tới)
                var danhSachViecLamDaLuu = _viecLamDaLuuRepository.LayDanhSachViecLamDaLuuTheoUserId(user.Id);
                var tinSapHetHan = danhSachViecLamDaLuu
                    .Where(v => v.TinTuyenDung != null && 
                                v.TinTuyenDung.HanNop >= now && 
                                v.TinTuyenDung.HanNop <= threeDaysFromNow &&
                                (v.TinTuyenDung.TrangThai == "Dang tuyen" || string.IsNullOrEmpty(v.TinTuyenDung.TrangThai)))
                    .Select(v => v.TinTuyenDung!)
                    .OrderBy(t => t.HanNop)
                    .ToList();

                foreach (var tin in tinSapHetHan)
                {
                    var daysLeft = (tin.HanNop.Date - now.Date).Days;
                    notifications.Add(new NotificationItem
                    {
                        Type = "JobExpiring",
                        Title = "Tin tuyển dụng sắp hết hạn",
                        Message = $"Tin tuyển dụng \"{tin.TenViecLam}\" bạn đã lưu sẽ hết hạn sau {daysLeft} ngày ({tin.HanNop:dd/MM/yyyy})",
                        Icon = "bi-clock-history",
                        Color = "text-warning",
                        Url = Url.Action("ViecLamDaLuu", "Candidate", new { area = "Candidate" }),
                        CreatedAt = tin.HanNop,
                        RelatedId = tin.MaTinTuyenDung,
                        NotificationKey = $"JobExpiring_{tin.MaTinTuyenDung}_{tin.HanNop:yyyyMMdd}"
                    });
                }

                // 3. Tin tuyển dụng đã lưu đã hết hạn
                var tinHetHan = danhSachViecLamDaLuu
                    .Where(v => v.TinTuyenDung != null && 
                                v.TinTuyenDung.HanNop < now &&
                                (v.TinTuyenDung.TrangThai == "Dang tuyen" || string.IsNullOrEmpty(v.TinTuyenDung.TrangThai) || v.TinTuyenDung.TrangThai == "Het han"))
                    .Select(v => v.TinTuyenDung!)
                    .OrderByDescending(t => t.HanNop)
                    .Take(5)
                    .ToList();

                foreach (var tin in tinHetHan)
                {
                    notifications.Add(new NotificationItem
                    {
                        Type = "JobExpired",
                        Title = "Tin tuyển dụng đã hết hạn",
                        Message = $"Tin tuyển dụng \"{tin.TenViecLam}\" bạn đã lưu đã hết hạn từ ngày {tin.HanNop:dd/MM/yyyy}",
                        Icon = "bi-exclamation-triangle-fill",
                        Color = "text-danger",
                        Url = Url.Action("ViecLamDaLuu", "Candidate", new { area = "Candidate" }),
                        CreatedAt = tin.HanNop,
                        RelatedId = tin.MaTinTuyenDung,
                        NotificationKey = $"JobExpired_{tin.MaTinTuyenDung}_{tin.HanNop:yyyyMMdd}"
                    });
                }

                // Sắp xếp theo thời gian (mới nhất trước)
                notifications = notifications.OrderByDescending(n => n.CreatedAt).ToList();

                var viewModel = new NotificationViewModel
                {
                    TotalCount = notifications.Count,
                    Notifications = notifications
                };

                return Json(new { success = true, data = viewModel });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }
    }
}
