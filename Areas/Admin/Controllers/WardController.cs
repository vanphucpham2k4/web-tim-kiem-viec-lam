using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Unicareer.Data;
using Unicareer.Models;
using Unicareer.Models.ViewModels;

namespace Unicareer.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{SD.Role_Admin}")]
    public class WardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WardController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Ward
        public async Task<IActionResult> Index(string? provinceCode = null, string? search = null, int pageNumber = 1, int pageSize = 50)
        {
            var query = _context.Wards
                .Include(w => w.Province)
                .Include(w => w.AdministrativeUnit)
                .AsQueryable();

            // Lọc theo tỉnh thành
            if (!string.IsNullOrEmpty(provinceCode))
            {
                query = query.Where(w => w.ProvinceCode == provinceCode);
            }

            // Tìm kiếm theo tên
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(w => 
                    (w.Name != null && w.Name.Contains(search)) ||
                    (w.FullName != null && w.FullName.Contains(search)) ||
                    (w.NameEn != null && w.NameEn.Contains(search))
                );
            }

            // Đếm tổng số bản ghi
            var totalCount = await query.CountAsync();

            // Phân trang
            var wards = await query
                .OrderBy(w => w.FullName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Lấy danh sách tỉnh thành để hiển thị trong filter
            var provinces = await _context.Provinces
                .OrderBy(p => p.FullName)
                .ToListAsync();

            var viewModel = new WardManagementViewModel
            {
                Wards = wards,
                Provinces = provinces,
                SearchTerm = search,
                SelectedProvinceCode = provinceCode,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };

            return View(viewModel);
        }

        // GET: Ward/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ward = await _context.Wards
                .Include(w => w.Province)
                .Include(w => w.AdministrativeUnit)
                .FirstOrDefaultAsync(m => m.Code == id);
            
            if (ward == null)
            {
                return NotFound();
            }

            return View(ward);
        }

        // POST: Ward/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string code, string name, string? nameEn, string? fullName, string? fullNameEn, string? codeName, string? provinceCode, int? administrativeUnitId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(code))
                {
                    return Json(new { success = false, message = "Mã quận/huyện không được để trống!" });
                }

                if (string.IsNullOrWhiteSpace(name))
                {
                    return Json(new { success = false, message = "Tên quận/huyện không được để trống!" });
                }

                // Kiểm tra mã đã tồn tại chưa
                var existingWard = await _context.Wards.FirstOrDefaultAsync(w => w.Code == code);
                if (existingWard != null)
                {
                    return Json(new { success = false, message = "Mã quận/huyện đã tồn tại!" });
                }

                // Kiểm tra tỉnh thành có tồn tại không
                if (!string.IsNullOrEmpty(provinceCode))
                {
                    var province = await _context.Provinces.FirstOrDefaultAsync(p => p.Code == provinceCode);
                    if (province == null)
                    {
                        return Json(new { success = false, message = "Tỉnh thành không tồn tại!" });
                    }
                }

                var ward = new Ward
                {
                    Code = code,
                    Name = name,
                    NameEn = nameEn,
                    FullName = fullName,
                    FullNameEn = fullNameEn,
                    CodeName = codeName,
                    ProvinceCode = provinceCode,
                    AdministrativeUnitId = administrativeUnitId
                };

                _context.Wards.Add(ward);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Thêm quận/huyện thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        // POST: Ward/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string code, string name, string? nameEn, string? fullName, string? fullNameEn, string? codeName, string? provinceCode, int? administrativeUnitId)
        {
            try
            {
                var ward = await _context.Wards.FirstOrDefaultAsync(w => w.Code == code);
                if (ward == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy quận/huyện!" });
                }

                if (string.IsNullOrWhiteSpace(name))
                {
                    return Json(new { success = false, message = "Tên quận/huyện không được để trống!" });
                }

                // Kiểm tra tỉnh thành có tồn tại không
                if (!string.IsNullOrEmpty(provinceCode))
                {
                    var province = await _context.Provinces.FirstOrDefaultAsync(p => p.Code == provinceCode);
                    if (province == null)
                    {
                        return Json(new { success = false, message = "Tỉnh thành không tồn tại!" });
                    }
                }

                ward.Name = name;
                ward.NameEn = nameEn;
                ward.FullName = fullName;
                ward.FullNameEn = fullNameEn;
                ward.CodeName = codeName;
                ward.ProvinceCode = provinceCode;
                ward.AdministrativeUnitId = administrativeUnitId;

                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Cập nhật quận/huyện thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        // POST: Ward/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string code)
        {
            try
            {
                var ward = await _context.Wards.FirstOrDefaultAsync(w => w.Code == code);
                
                if (ward == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy quận/huyện!" });
                }

                _context.Wards.Remove(ward);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Xóa quận/huyện thành công!" });
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

