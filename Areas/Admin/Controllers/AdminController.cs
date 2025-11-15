using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Unicareer.Models;
using Unicareer.Repository;
using Unicareer.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Unicareer.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{SD.Role_Admin}")]
    public class AdminController : Controller
    {
        private readonly INhaTuyenDungRepository _nhaTuyenDungRepository;
        private readonly IUngVienRepository _ungVienRepository;
        private readonly ITinTuyenDungRepository _tinTuyenDungRepository;
        private readonly ITinUngTuyenRepository _tinUngTuyenRepository;
        private readonly ILoaiCongViecRepository _loaiCongViecRepository;
        private readonly INganhNgheRepository _nganhNgheRepository;
        private readonly IChuyenNganhRepository _chuyenNganhRepository;
        private readonly ITruongDaiHocRepository _truongDaiHocRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public AdminController(INhaTuyenDungRepository nhaTuyenDungRepository, IUngVienRepository ungVienRepository, ITinTuyenDungRepository tinTuyenDungRepository, ITinUngTuyenRepository tinUngTuyenRepository, ILoaiCongViecRepository loaiCongViecRepository, INganhNgheRepository nganhNgheRepository, IChuyenNganhRepository chuyenNganhRepository, ITruongDaiHocRepository truongDaiHocRepository, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _nhaTuyenDungRepository = nhaTuyenDungRepository;
            _ungVienRepository = ungVienRepository;
            _tinTuyenDungRepository = tinTuyenDungRepository;
            _tinUngTuyenRepository = tinUngTuyenRepository;
            _loaiCongViecRepository = loaiCongViecRepository;
            _nganhNgheRepository = nganhNgheRepository;
            _chuyenNganhRepository = chuyenNganhRepository;
            _truongDaiHocRepository = truongDaiHocRepository;
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Dashboard()
        {
            var danhSachNhaTuyenDung = _nhaTuyenDungRepository.LayDanhSachNhaTuyenDung();
            var danhSachUngVien = _ungVienRepository.LayDanhSachUngVien();
            var danhSachTinTuyenDung = _tinTuyenDungRepository.LayDanhSachTinTuyenDung();
            var danhSachTinUngTuyen = _tinUngTuyenRepository.LayDanhSachTinUngTuyen();
            var danhSachLoaiCongViec = _loaiCongViecRepository.LayDanhSachLoaiCongViec();
            var danhSachNganhNghe = _nganhNgheRepository.LayDanhSachNganhNghe();
            ViewBag.DanhSachNhaTuyenDung = danhSachNhaTuyenDung;
            ViewBag.DanhSachUngVien = danhSachUngVien;
            ViewBag.DanhSachTinTuyenDung = danhSachTinTuyenDung;
            ViewBag.DanhSachTinUngTuyen = danhSachTinUngTuyen;
            ViewBag.DanhSachLoaiCongViec = danhSachLoaiCongViec;
            ViewBag.DanhSachNganhNghe = danhSachNganhNghe;
            return View();
        }

        public IActionResult UngVien()
        {
            var danhSachUngVien = _ungVienRepository.LayDanhSachUngVien();
            return View(danhSachUngVien);
        }

        public IActionResult NhaTuyenDung()
        {
            var danhSachNhaTuyenDung = _nhaTuyenDungRepository.LayDanhSachNhaTuyenDung();
            var danhSachTinTuyenDung = _tinTuyenDungRepository.LayDanhSachTinTuyenDung();
            var danhSachTinUngTuyen = _tinUngTuyenRepository.LayDanhSachTinUngTuyen();
            var danhSachTinhThanh = _context.Provinces
                .OrderBy(p => p.FullName)
                .ToList();
            ViewBag.DanhSachTinTuyenDung = danhSachTinTuyenDung;
            ViewBag.DanhSachTinUngTuyen = danhSachTinUngTuyen;
            ViewBag.DanhSachTinhThanh = danhSachTinhThanh;
            return View(danhSachNhaTuyenDung);
        }

        // GET: Chi tiết nhà tuyển dụng
        public IActionResult ChiTietNhaTuyenDung(int id)
        {
            ViewData["Title"] = "Chi tiết Nhà tuyển dụng";
            
            var nhaTuyenDung = _nhaTuyenDungRepository.LayNhaTuyenDungTheoId(id);
            if (nhaTuyenDung == null)
            {
                TempData["Loi"] = "Không tìm thấy nhà tuyển dụng!";
                return RedirectToAction("NhaTuyenDung");
            }
            
            // Lấy danh sách tin tuyển dụng theo tên công ty
            var danhSachTinTuyenDung = _tinTuyenDungRepository.LayDanhSachTheoCongTy(nhaTuyenDung.TenCongTy);
            ViewBag.DanhSachTinTuyenDung = danhSachTinTuyenDung;
            
            return View(nhaTuyenDung);
        }

        public async Task<IActionResult> Settings()
        {
            ViewData["Title"] = "Cài đặt";
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                ViewBag.User = user;
            }
            var model = new Models.ViewModels.ChangePasswordViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(string hoTen, string soDienThoai)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy người dùng" });
                }

                user.HoTen = hoTen;
                user.PhoneNumber = soDienThoai;

                var result = await _userManager.UpdateAsync(user);
                
                if (result.Succeeded)
                {
                    return Json(new { success = true, message = "Cập nhật thông tin thành công!" });
                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return Json(new { success = false, message = errors });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
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

        public IActionResult NganhNghe()
        {
            var danhSachNganhNghe = _nganhNgheRepository.LayDanhSachNganhNghe();
            var danhSachTinTuyenDung = _tinTuyenDungRepository.LayDanhSachTinTuyenDung();
            
            // Tính số lượng công việc cho mỗi ngành nghề dựa trên số tin tuyển dụng
            foreach (var nganhNghe in danhSachNganhNghe)
            {
                nganhNghe.SoLuongCongViec = danhSachTinTuyenDung
                    .Count(t => !string.IsNullOrEmpty(t.NganhNghe) && 
                                t.NganhNghe.Equals(nganhNghe.TenNganhNghe, StringComparison.OrdinalIgnoreCase));
            }
            
            return View(danhSachNganhNghe);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ThemNganhNghe(string tenNganhNghe, string? moTa)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tenNganhNghe))
                {
                    return Json(new { success = false, message = "Tên ngành nghề không được để trống!" });
                }

                var nganhNghe = new NganhNghe
                {
                    TenNganhNghe = tenNganhNghe,
                    MoTa = string.IsNullOrWhiteSpace(moTa) ? null : moTa
                };

                var ketQua = _nganhNgheRepository.ThemNganhNghe(nganhNghe);
                if (ketQua == null)
                {
                    return Json(new { success = false, message = "Có lỗi xảy ra khi thêm ngành nghề!" });
                }

                return Json(new { success = true, message = "Thêm ngành nghề thành công!", data = new { ma = ketQua.MaNganhNghe, ten = ketQua.TenNganhNghe, moTa = ketQua.MoTa, ngayTao = ketQua.NgayTao.ToString("dd/MM/yyyy") } });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CapNhatNganhNghe(int id, string tenNganhNghe, string? moTa)
        {
            try
            {
                var nganhNghe = _nganhNgheRepository.LayNganhNgheTheoId(id);
                if (nganhNghe == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy ngành nghề!" });
                }

                if (string.IsNullOrWhiteSpace(tenNganhNghe))
                {
                    return Json(new { success = false, message = "Tên ngành nghề không được để trống!" });
                }

                nganhNghe.TenNganhNghe = tenNganhNghe;
                nganhNghe.MoTa = string.IsNullOrWhiteSpace(moTa) ? null : moTa;

                var ketQua = _nganhNgheRepository.CapNhatNganhNghe(nganhNghe);
                if (ketQua == null)
                {
                    return Json(new { success = false, message = "Có lỗi xảy ra khi cập nhật ngành nghề!" });
                }

                return Json(new { success = true, message = "Cập nhật ngành nghề thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult XoaNganhNghe(int id)
        {
            try
            {
                // Kiểm tra ngành nghề có tồn tại không
                var nganhNghe = _nganhNgheRepository.LayNganhNgheTheoId(id);
                if (nganhNghe == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy ngành nghề để xóa!" });
                }

                var ketQua = _nganhNgheRepository.XoaNganhNghe(id);
                if (!ketQua)
                {
                    return Json(new { success = false, message = "Không thể xóa ngành nghề. Có thể do ràng buộc dữ liệu!" });
                }

                return Json(new { success = true, message = "Xóa ngành nghề thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        public IActionResult LoaiCongViec()
        {
            var danhSachLoaiCongViec = _loaiCongViecRepository.LayDanhSachLoaiCongViec();
            var danhSachTinTuyenDung = _tinTuyenDungRepository.LayDanhSachTinTuyenDung();
            
            // Tính số lượng vị trí cho mỗi loại công việc dựa trên số tin tuyển dụng
            foreach (var loaiCongViec in danhSachLoaiCongViec)
            {
                loaiCongViec.SoLuongViTri = danhSachTinTuyenDung
                    .Count(t => !string.IsNullOrEmpty(t.LoaiCongViec) && 
                                t.LoaiCongViec.Equals(loaiCongViec.TenLoaiCongViec, StringComparison.OrdinalIgnoreCase));
            }
            
            return View(danhSachLoaiCongViec);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ThemLoaiCongViec(string tenLoaiCongViec, string? moTa)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tenLoaiCongViec))
                {
                    return Json(new { success = false, message = "Tên loại công việc không được để trống!" });
                }

                var loaiCongViec = new LoaiCongViec
                {
                    TenLoaiCongViec = tenLoaiCongViec,
                    MoTa = string.IsNullOrWhiteSpace(moTa) ? string.Empty : moTa
                };

                var ketQua = _loaiCongViecRepository.ThemLoaiCongViec(loaiCongViec);
                if (ketQua == null)
                {
                    return Json(new { success = false, message = "Có lỗi xảy ra khi thêm loại công việc!" });
                }

                return Json(new { success = true, message = "Thêm loại công việc thành công!", data = new { ma = ketQua.MaLoaiCongViec, ten = ketQua.TenLoaiCongViec, moTa = ketQua.MoTa, ngayTao = ketQua.NgayTao.ToString("dd/MM/yyyy") } });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CapNhatLoaiCongViec(int id, string tenLoaiCongViec, string? moTa)
        {
            try
            {
                var loaiCongViec = _loaiCongViecRepository.LayLoaiCongViecTheoId(id);
                if (loaiCongViec == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy loại công việc!" });
                }

                if (string.IsNullOrWhiteSpace(tenLoaiCongViec))
                {
                    return Json(new { success = false, message = "Tên loại công việc không được để trống!" });
                }

                loaiCongViec.TenLoaiCongViec = tenLoaiCongViec;
                loaiCongViec.MoTa = string.IsNullOrWhiteSpace(moTa) ? string.Empty : moTa;

                var ketQua = _loaiCongViecRepository.CapNhatLoaiCongViec(loaiCongViec);
                if (ketQua == null)
                {
                    return Json(new { success = false, message = "Có lỗi xảy ra khi cập nhật loại công việc!" });
                }

                return Json(new { success = true, message = "Cập nhật loại công việc thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult XoaLoaiCongViec(int id)
        {
            try
            {
                var loaiCongViec = _loaiCongViecRepository.LayLoaiCongViecTheoId(id);
                if (loaiCongViec == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy loại công việc để xóa!" });
                }

                var ketQua = _loaiCongViecRepository.XoaLoaiCongViec(id);
                if (!ketQua)
                {
                    return Json(new { success = false, message = "Không thể xóa loại công việc. Có thể do ràng buộc dữ liệu!" });
                }

                return Json(new { success = true, message = "Xóa loại công việc thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        public IActionResult ChuyenNganh()
        {
            var danhSachChuyenNganh = _chuyenNganhRepository.LayDanhSachChuyenNganh();
            var danhSachNganhNghe = _nganhNgheRepository.LayDanhSachNganhNghe();
            ViewBag.DanhSachNganhNghe = danhSachNganhNghe;
            return View(danhSachChuyenNganh);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ThemChuyenNganh(string tenChuyenNganh, string? moTa, int maNganhNghe)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tenChuyenNganh))
                {
                    return Json(new { success = false, message = "Tên chuyên ngành không được để trống!" });
                }

                var chuyenNganh = new ChuyenNganh
                {
                    TenChuyenNganh = tenChuyenNganh,
                    MoTa = moTa,
                    MaNganhNghe = maNganhNghe
                };

                var ketQua = _chuyenNganhRepository.ThemChuyenNganh(chuyenNganh);
                if (ketQua == null)
                {
                    return Json(new { success = false, message = "Có lỗi xảy ra khi thêm chuyên ngành!" });
                }

                return Json(new { success = true, message = "Thêm chuyên ngành thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ThemNhieuChuyenNganh([FromBody] ThemNhieuChuyenNganhRequest request)
        {
            try
            {
                if (request == null || request.MaNganhNghe <= 0)
                {
                    return Json(new { success = false, message = "Vui lòng chọn ngành nghề!" });
                }

                if (request.DanhSachChuyenNganh == null || request.DanhSachChuyenNganh.Count == 0)
                {
                    return Json(new { success = false, message = "Vui lòng thêm ít nhất một chuyên ngành!" });
                }

                var danhSachThanhCong = new List<string>();
                var danhSachLoi = new List<string>();

                foreach (var input in request.DanhSachChuyenNganh)
                {
                    if (string.IsNullOrWhiteSpace(input.TenChuyenNganh))
                    {
                        danhSachLoi.Add("Có chuyên ngành chưa nhập tên");
                        continue;
                    }

                    var chuyenNganh = new ChuyenNganh
                    {
                        TenChuyenNganh = input.TenChuyenNganh.Trim(),
                        MoTa = string.IsNullOrWhiteSpace(input.MoTa) ? null : input.MoTa.Trim(),
                        MaNganhNghe = request.MaNganhNghe
                    };

                    var ketQua = _chuyenNganhRepository.ThemChuyenNganh(chuyenNganh);
                    if (ketQua != null)
                    {
                        danhSachThanhCong.Add(input.TenChuyenNganh);
                    }
                    else
                    {
                        danhSachLoi.Add(input.TenChuyenNganh);
                    }
                }

                if (danhSachLoi.Count > 0 && danhSachThanhCong.Count == 0)
                {
                    return Json(new { success = false, message = "Không thể thêm bất kỳ chuyên ngành nào!" });
                }

                var message = $"Đã thêm thành công {danhSachThanhCong.Count} chuyên ngành";
                if (danhSachLoi.Count > 0)
                {
                    message += $". Có {danhSachLoi.Count} chuyên ngành thất bại.";
                }

                return Json(new { success = true, message = message, count = danhSachThanhCong.Count });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        public class ChuyenNganhInput
        {
            public string TenChuyenNganh { get; set; } = string.Empty;
            public string? MoTa { get; set; }
        }

        public class ThemNhieuChuyenNganhRequest
        {
            public int MaNganhNghe { get; set; }
            public List<ChuyenNganhInput> DanhSachChuyenNganh { get; set; } = new();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CapNhatChuyenNganh(int id, string tenChuyenNganh, string? moTa, int maNganhNghe)
        {
            try
            {
                var chuyenNganh = _chuyenNganhRepository.LayChuyenNganhTheoId(id);
                if (chuyenNganh == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy chuyên ngành!" });
                }

                if (string.IsNullOrWhiteSpace(tenChuyenNganh))
                {
                    return Json(new { success = false, message = "Tên chuyên ngành không được để trống!" });
                }

                chuyenNganh.TenChuyenNganh = tenChuyenNganh;
                chuyenNganh.MoTa = moTa;
                chuyenNganh.MaNganhNghe = maNganhNghe;

                var ketQua = _chuyenNganhRepository.CapNhatChuyenNganh(chuyenNganh);
                if (ketQua == null)
                {
                    return Json(new { success = false, message = "Có lỗi xảy ra khi cập nhật chuyên ngành!" });
                }

                return Json(new { success = true, message = "Cập nhật chuyên ngành thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult XoaChuyenNganh(int id)
        {
            try
            {
                var ketQua = _chuyenNganhRepository.XoaChuyenNganh(id);
                if (!ketQua)
                {
                    return Json(new { success = false, message = "Không tìm thấy chuyên ngành để xóa!" });
                }

                return Json(new { success = true, message = "Xóa chuyên ngành thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        [HttpGet]
        public IActionResult LayChuyenNganhTheoNganhNghe(int maNganhNghe)
        {
            try
            {
                var danhSachChuyenNganh = _chuyenNganhRepository.LayChuyenNganhTheoNganhNghe(maNganhNghe);
                var ketQua = danhSachChuyenNganh.Select(c => new
                {
                    maChuyenNganh = c.MaChuyenNganh,
                    tenChuyenNganh = c.TenChuyenNganh
                }).ToList();

                return Json(new { success = true, data = ketQua });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        public IActionResult TruongDaiHoc()
        {
            var danhSachTruongDaiHoc = _truongDaiHocRepository.LayDanhSachTruongDaiHoc();
            return View(danhSachTruongDaiHoc);
        }

        public IActionResult TinTuyenDung()
        {
            var danhSachTinTuyenDung = _tinTuyenDungRepository.LayDanhSachTinTuyenDung();
            return View(danhSachTinTuyenDung);
        }

        // GET: Chi tiết tin tuyển dụng
        public IActionResult ChiTietTinTuyenDung(int id)
        {
            ViewData["Title"] = "Chi tiết tin tuyển dụng";
            
            var tinTuyenDung = _tinTuyenDungRepository.LayTinTuyenDungTheoId(id);
            if (tinTuyenDung == null)
            {
                TempData["Loi"] = "Không tìm thấy tin tuyển dụng!";
                return RedirectToAction("TinTuyenDung");
            }
            
            // Lấy danh sách ứng tuyển theo mã tin
            var danhSachUngTuyen = _tinUngTuyenRepository.LayDanhSachTinUngTuyen()
                .Where(t => t.MaTinTuyenDung == id.ToString())
                .ToList();
            
            // Lấy danh sách tỉnh/thành phố
            var danhSachTinhThanh = _context.Provinces
                .OrderBy(p => p.FullName)
                .ToList();
            
            ViewBag.DanhSachUngTuyen = danhSachUngTuyen;
            ViewBag.DanhSachTinhThanh = danhSachTinhThanh;
            
            return View(tinTuyenDung);
        }

        // POST: Duyệt tin tuyển dụng
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DuyetTinTuyenDung(int id)
        {
            try
            {
                var tin = _tinTuyenDungRepository.LayTinTuyenDungTheoId(id);
                if (tin == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy tin tuyển dụng!" });
                }

                // TODO: Cập nhật trạng thái duyệt trong database
                // Ví dụ: tin.TrangThaiDuyet = "Da duyet";
                // _tinTuyenDungRepository.CapNhatTinTuyenDung(id, tin);

                return Json(new { success = true, message = "Đã duyệt tin tuyển dụng thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        // POST: Duyệt hàng loạt
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DuyetHangLoat(List<int> danhSachMa)
        {
            try
            {
                if (danhSachMa == null || danhSachMa.Count == 0)
                {
                    return Json(new { success = false, message = "Vui lòng chọn ít nhất một tin!" });
                }

                int count = 0;
                foreach (var ma in danhSachMa)
                {
                    var tin = _tinTuyenDungRepository.LayTinTuyenDungTheoId(ma);
                    if (tin != null)
                    {
                        // TODO: Cập nhật trạng thái duyệt
                        // tin.TrangThaiDuyet = "Da duyet";
                        // _tinTuyenDungRepository.CapNhatTinTuyenDung(ma, tin);
                        count++;
                    }
                }

                return Json(new { success = true, count = count, message = $"Đã duyệt {count} tin thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        // POST: Khóa tin tuyển dụng
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult KhoaTinTuyenDung(int id, string lyDo)
        {
            try
            {
                var tin = _tinTuyenDungRepository.LayTinTuyenDungTheoId(id);
                if (tin == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy tin tuyển dụng!" });
                }

                if (string.IsNullOrWhiteSpace(lyDo))
                {
                    return Json(new { success = false, message = "Vui lòng nhập lý do khóa!" });
                }

                // TODO: Cập nhật trạng thái khóa trong database
                // Ví dụ: tin.TrangThaiDuyet = "Bi khoa";
                // tin.LyDoKhoa = lyDo;
                // _tinTuyenDungRepository.CapNhatTinTuyenDung(id, tin);

                return Json(new { success = true, message = "Đã khóa tin tuyển dụng thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        // GET: Lấy danh sách ứng viên theo mã tin
        [HttpGet]
        public IActionResult LayDanhSachUngVien(int maTin)
        {
            try
            {
                var danhSachUngTuyen = _tinUngTuyenRepository.LayDanhSachTinUngTuyen()
                    .Where(t => t.MaTinTuyenDung == maTin.ToString())
                    .Select(t => new
                    {
                        hoTen = t.HoTen,
                        email = t.Email,
                        soDienThoai = t.SoDienThoai,
                        viTriUngTuyen = t.ViTriUngTuyen,
                        trangThaiXuLy = t.TrangThaiXuLy,
                        linkCV = t.LinkCV,
                        ngayUngTuyen = t.NgayUngTuyen.ToString("dd/MM/yyyy")
                    })
                    .ToList();

                return Json(new { success = true, data = danhSachUngTuyen });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        // POST: Xóa tin tuyển dụng
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult XoaTinTuyenDung(int id)
        {
            try
            {
                var tin = _tinTuyenDungRepository.LayTinTuyenDungTheoId(id);
                if (tin == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy tin tuyển dụng!" });
                }

                // TODO: Xóa tin trong database
                // _tinTuyenDungRepository.XoaTinTuyenDung(id);

                return Json(new { success = true, message = "Đã xóa tin tuyển dụng thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        // POST: Cap nhat tin tuyen dung
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CapNhatTinTuyenDung(int id, TinTuyenDung model)
        {
            // Lấy tin hiện tại để đảm bảo có đầy đủ thông tin
            var tinHienTai = _tinTuyenDungRepository.LayTinTuyenDungTheoId(id);
            if (tinHienTai == null)
            {
                TempData["Loi"] = "Không tìm thấy tin tuyển dụng!";
                return RedirectToAction("TinTuyenDung");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var tinDaCapNhat = _tinTuyenDungRepository.CapNhatTinTuyenDung(id, model);
                    if (tinDaCapNhat == null)
                    {
                        TempData["Loi"] = "Không tìm thấy tin tuyển dụng để cập nhật!";
                        return RedirectToAction("TinTuyenDung");
                    }
                    
                    TempData["ThanhCong"] = "Cập nhật tin tuyển dụng thành công!";
                    return RedirectToAction("ChiTietTinTuyenDung", new { id = id });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Có lỗi xảy ra khi cập nhật tin: " + ex.Message);
                }
            }
            
            // Nếu có lỗi validation, cập nhật các trường đã thay đổi vào tin hiện tại và hiển thị lại
            tinHienTai.TenViecLam = model.TenViecLam;
            tinHienTai.NganhNghe = model.NganhNghe;
            tinHienTai.NganhNgheChiTiet = model.NganhNgheChiTiet;
            tinHienTai.LoaiCongViec = model.LoaiCongViec;
            tinHienTai.KinhNghiem = model.KinhNghiem;
            tinHienTai.ViTri = model.ViTri;
            tinHienTai.NgoaiNgu = model.NgoaiNgu;
            tinHienTai.KyNang = model.KyNang;
            tinHienTai.MoTa = model.MoTa;
            tinHienTai.YeuCau = model.YeuCau;
            tinHienTai.QuyenLoi = model.QuyenLoi;
            tinHienTai.MucLuongThapNhat = model.MucLuongThapNhat;
            tinHienTai.MucLuongCaoNhat = model.MucLuongCaoNhat;
            tinHienTai.DiaChiLamViec = model.DiaChiLamViec;
            tinHienTai.PhuongXa = model.PhuongXa;
            tinHienTai.TinhThanhPho = model.TinhThanhPho;
            tinHienTai.NguoiLienHe = model.NguoiLienHe;
            tinHienTai.EmailLienHe = model.EmailLienHe;
            tinHienTai.SDTLienHe = model.SDTLienHe;
            tinHienTai.TuKhoa = model.TuKhoa;
            tinHienTai.HanNop = model.HanNop;
            tinHienTai.Latitude = model.Latitude;
            tinHienTai.Longitude = model.Longitude;

            // Lấy danh sách ứng tuyển để hiển thị lại
            var danhSachUngTuyen = _tinUngTuyenRepository.LayDanhSachTinUngTuyen()
                .Where(t => t.MaTinTuyenDung == id.ToString())
                .ToList();
            
            // Lấy danh sách tỉnh/thành phố
            var danhSachTinhThanh = _context.Provinces
                .OrderBy(p => p.FullName)
                .ToList();
            
            ViewBag.DanhSachUngTuyen = danhSachUngTuyen;
            ViewBag.DanhSachTinhThanh = danhSachTinhThanh;

            return View("ChiTietTinTuyenDung", tinHienTai);
        }

        // GET: Xuất Excel
        public IActionResult XuatExcel()
        {
            try
            {
                var danhSachTinTuyenDung = _tinTuyenDungRepository.LayDanhSachTinTuyenDung();
                
                // TODO: Tạo file Excel và trả về
                // Có thể sử dụng thư viện như EPPlus, ClosedXML, hoặc NPOI
                // Tạm thời trả về JSON
                return Json(new { success = true, message = "Tính năng xuất Excel đang được phát triển!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        // GET: Lấy danh sách tin tuyển dụng theo tên công ty
        [HttpGet]
        public IActionResult LayDanhSachTinTuyenDungTheoCongTy(string tenCongTy)
        {
            try
            {
                if (string.IsNullOrEmpty(tenCongTy))
                {
                    return Json(new { success = false, message = "Tên công ty không được để trống!" });
                }

                var danhSachTin = _tinTuyenDungRepository.LayDanhSachTheoCongTy(tenCongTy)
                    .Select(t => new
                    {
                        maTin = t.MaTinTuyenDung,
                        tenViecLam = t.TenViecLam,
                        congTy = t.CongTy,
                        nganhNghe = t.NganhNghe,
                        loaiCongViec = t.LoaiCongViec,
                        tinhThanhPho = t.TinhThanhPho,
                        mucLuongThapNhat = t.MucLuongThapNhat,
                        mucLuongCaoNhat = t.MucLuongCaoNhat,
                        soLuongUngTuyen = t.SoLuongUngTuyen ?? 0,
                        ngayDang = t.NgayDang.ToString("dd/MM/yyyy"),
                        hanNop = t.HanNop.ToString("dd/MM/yyyy"),
                        trangThai = t.SoLuongUngTuyen > 0 ? "Đang tuyển" : "Chưa có ứng viên"
                    })
                    .ToList();

                return Json(new { success = true, data = danhSachTin });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        // POST: Xóa nhà tuyển dụng
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult XoaNhaTuyenDung(int id)
        {
            try
            {
                var nhaTuyenDung = _nhaTuyenDungRepository.LayNhaTuyenDungTheoId(id);
                if (nhaTuyenDung == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy nhà tuyển dụng!" });
                }

                // TODO: Xóa nhà tuyển dụng trong database
                // _nhaTuyenDungRepository.XoaNhaTuyenDung(id);

                return Json(new { success = true, message = "Đã xóa nhà tuyển dụng thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        // GET: Lấy danh sách quận/huyện theo tỉnh/thành phố
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
    }
}

