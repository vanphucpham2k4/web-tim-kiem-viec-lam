using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Unicareer.Data;
using Unicareer.Models;
using Microsoft.AspNetCore.Http;

namespace Unicareer.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{SD.Role_Admin}")]
    public class ProvinceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProvinceController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Province
        public async Task<IActionResult> Index()
        {
            var provinces = await _context.Provinces
                .Include(p => p.AdministrativeUnit)
                .OrderBy(p => p.FullName)
                .ToListAsync();
            
            return View(provinces);
        }

        // GET: Province/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var province = await _context.Provinces
                .Include(p => p.AdministrativeUnit)
                .FirstOrDefaultAsync(m => m.Code == id);
            
            if (province == null)
            {
                return NotFound();
            }

            return View(province);
        }

        // Helper method để xử lý upload file
        private async Task<string?> UploadFileAsync(IFormFile? file, string folder, string provinceCode)
        {
            if (file == null || file.Length == 0)
                return null;

            // Kiểm tra định dạng file (cho phép JPG, PNG, GIF, WEBP)
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            
            if (!allowedExtensions.Contains(fileExtension))
            {
                throw new Exception("Định dạng file không hợp lệ. Chỉ chấp nhận JPG, PNG, GIF, WEBP.");
            }

            // Kiểm tra kích thước file (tối đa 10MB)
            var maxSize = 10 * 1024 * 1024; // 10MB
            if (file.Length > maxSize)
            {
                throw new Exception("Kích thước file quá lớn. Tối đa 10MB.");
            }

            // Tạo tên file unique
            var fileName = $"province_{provinceCode}_{DateTime.Now:yyyyMMddHHmmss}_{Guid.NewGuid():N}{fileExtension}";
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

        // POST: Province/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string code, string name, string? nameEn, string fullName, string? fullNameEn, string? codeName, int? administrativeUnitId, IFormFile? imageFile)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(code))
                {
                    return Json(new { success = false, message = "Mã tỉnh thành không được để trống!" });
                }

                if (string.IsNullOrWhiteSpace(name))
                {
                    return Json(new { success = false, message = "Tên tỉnh thành không được để trống!" });
                }

                if (string.IsNullOrWhiteSpace(fullName))
                {
                    return Json(new { success = false, message = "Tên đầy đủ không được để trống!" });
                }

                // Kiểm tra mã đã tồn tại chưa
                var existingProvince = await _context.Provinces.FirstOrDefaultAsync(p => p.Code == code);
                if (existingProvince != null)
                {
                    return Json(new { success = false, message = "Mã tỉnh thành đã tồn tại!" });
                }

                // Upload ảnh nếu có
                string? imagePath = null;
                if (imageFile != null && imageFile.Length > 0)
                {
                    try
                    {
                        imagePath = await UploadFileAsync(imageFile, "provinces", code);
                    }
                    catch (Exception ex)
                    {
                        return Json(new { success = false, message = ex.Message });
                    }
                }

                var province = new Province
                {
                    Code = code,
                    Name = name,
                    NameEn = nameEn,
                    FullName = fullName,
                    FullNameEn = fullNameEn,
                    CodeName = codeName,
                    AdministrativeUnitId = administrativeUnitId,
                    Image = imagePath
                };

                _context.Provinces.Add(province);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Thêm tỉnh thành thành công!", imageUrl = imagePath });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        // POST: Province/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string code, string name, string? nameEn, string fullName, string? fullNameEn, string? codeName, int? administrativeUnitId, IFormFile? imageFile)
        {
            try
            {
                var province = await _context.Provinces.FirstOrDefaultAsync(p => p.Code == code);
                if (province == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy tỉnh thành!" });
                }

                if (string.IsNullOrWhiteSpace(name))
                {
                    return Json(new { success = false, message = "Tên tỉnh thành không được để trống!" });
                }

                if (string.IsNullOrWhiteSpace(fullName))
                {
                    return Json(new { success = false, message = "Tên đầy đủ không được để trống!" });
                }

                // Upload ảnh mới nếu có
                if (imageFile != null && imageFile.Length > 0)
                {
                    try
                    {
                        // Xóa ảnh cũ nếu có
                        if (!string.IsNullOrEmpty(province.Image) && province.Image.StartsWith("/uploads/"))
                        {
                            var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", province.Image.TrimStart('/'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                try
                                {
                                    System.IO.File.Delete(oldImagePath);
                                }
                                catch
                                {
                                    // Bỏ qua lỗi xóa file cũ
                                }
                            }
                        }

                        var imagePath = await UploadFileAsync(imageFile, "provinces", code);
                        province.Image = imagePath;
                    }
                    catch (Exception ex)
                    {
                        return Json(new { success = false, message = ex.Message });
                    }
                }

                province.Name = name;
                province.NameEn = nameEn;
                province.FullName = fullName;
                province.FullNameEn = fullNameEn;
                province.CodeName = codeName;
                province.AdministrativeUnitId = administrativeUnitId;

                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Cập nhật tỉnh thành thành công!", imageUrl = province.Image });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        // POST: Province/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string code)
        {
            try
            {
                var province = await _context.Provinces
                    .Include(p => p.Wards)
                    .FirstOrDefaultAsync(p => p.Code == code);
                
                if (province == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy tỉnh thành!" });
                }

                // Kiểm tra xem có quận/huyện nào thuộc tỉnh này không
                if (province.Wards != null && province.Wards.Any())
                {
                    return Json(new { success = false, message = "Không thể xóa tỉnh thành vì còn quận/huyện thuộc tỉnh này!" });
                }

                // Xóa ảnh nếu có
                if (!string.IsNullOrEmpty(province.Image) && province.Image.StartsWith("/uploads/"))
                {
                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", province.Image.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        try
                        {
                            System.IO.File.Delete(imagePath);
                        }
                        catch
                        {
                            // Bỏ qua lỗi xóa file
                        }
                    }
                }

                _context.Provinces.Remove(province);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Xóa tỉnh thành thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        // GET: Lấy danh sách AdministrativeUnit
        [HttpGet]
        public async Task<IActionResult> GetAdministrativeUnits()
        {
            var units = await _context.AdministrativeUnits
                .OrderBy(u => u.FullName)
                .Select(u => new
                {
                    id = u.Id,
                    fullName = u.FullName
                })
                .ToListAsync();

            return Json(new { success = true, data = units });
        }
    }
}

