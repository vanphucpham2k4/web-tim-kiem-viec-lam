using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Unicareer.Models;
using Unicareer.Models.ViewModels;
using Unicareer.Repository;
using Unicareer.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Unicareer.Services;

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
        private readonly IBlogRepository _blogRepository;
        private readonly ITheLoaiBlogRepository _theLoaiBlogRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IExternalArticleService _externalArticleService;
        private readonly ILogger<AdminController> _logger;

        public AdminController(INhaTuyenDungRepository nhaTuyenDungRepository, IUngVienRepository ungVienRepository, ITinTuyenDungRepository tinTuyenDungRepository, ITinUngTuyenRepository tinUngTuyenRepository, ILoaiCongViecRepository loaiCongViecRepository, INganhNgheRepository nganhNgheRepository, IChuyenNganhRepository chuyenNganhRepository, ITruongDaiHocRepository truongDaiHocRepository, IBlogRepository blogRepository, ITheLoaiBlogRepository theLoaiBlogRepository, UserManager<ApplicationUser> userManager, ApplicationDbContext context, IMapper mapper, IExternalArticleService externalArticleService, ILogger<AdminController> logger)
        {
            _nhaTuyenDungRepository = nhaTuyenDungRepository;
            _ungVienRepository = ungVienRepository;
            _tinTuyenDungRepository = tinTuyenDungRepository;
            _tinUngTuyenRepository = tinUngTuyenRepository;
            _loaiCongViecRepository = loaiCongViecRepository;
            _nganhNgheRepository = nganhNgheRepository;
            _chuyenNganhRepository = chuyenNganhRepository;
            _truongDaiHocRepository = truongDaiHocRepository;
            _blogRepository = blogRepository;
            _theLoaiBlogRepository = theLoaiBlogRepository;
            _userManager = userManager;
            _context = context;
            _mapper = mapper;
            _externalArticleService = externalArticleService;
            _logger = logger;
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
            
            var soTinDaDangDict = danhSachTinTuyenDung
                .Where(t => t.MaNhaTuyenDung.HasValue)
                .GroupBy(t => t.MaNhaTuyenDung.Value)
                .ToDictionary(g => g.Key, g => g.Count());
            
            var soLuongCongViecDict = danhSachTinTuyenDung
                .Where(t => !string.IsNullOrEmpty(t.NganhNghe))
                .GroupBy(t => t.NganhNghe)
                .ToDictionary(g => g.Key, g => g.Count());
            
            var soLuongViTriDict = danhSachTinTuyenDung
                .Where(t => !string.IsNullOrEmpty(t.LoaiCongViec))
                .GroupBy(t => t.LoaiCongViec)
                .ToDictionary(g => g.Key, g => g.Count());
            
            var ngayHienTai = DateTime.Now;
            var thangLabels = new List<string>();
            var ungVienData = new List<int>();
            var nhaTuyenDungData = new List<int>();
            var tinDangData = new List<int>();
            var luotUngTuyenData = new List<int>();
            var nganhNgheData = new List<int>();
            var loaiCongViecData = new List<int>();
            
            for (int i = 6; i >= 0; i--)
            {
                var thang = ngayHienTai.AddMonths(-i);
                var thangBatDau = new DateTime(thang.Year, thang.Month, 1);
                var thangKetThuc = thangBatDau.AddMonths(1).AddDays(-1);
                
                thangLabels.Add($"Th {thang.Month}");
                
                var soUngVien = danhSachUngVien.Count(u => 
                    u.NgayDangKy >= thangBatDau && u.NgayDangKy <= thangKetThuc);
                ungVienData.Add(soUngVien);
                
                var soNhaTuyenDung = danhSachNhaTuyenDung.Count(n => 
                    n.NgayDangKy >= thangBatDau && n.NgayDangKy <= thangKetThuc);
                nhaTuyenDungData.Add(soNhaTuyenDung);
                
                var soTinDang = danhSachTinTuyenDung.Count(t => 
                    t.NgayDang >= thangBatDau && t.NgayDang <= thangKetThuc);
                tinDangData.Add(soTinDang);
                
                var soLuotUngTuyen = danhSachTinUngTuyen.Count(t => 
                    t.NgayUngTuyen >= thangBatDau && t.NgayUngTuyen <= thangKetThuc);
                luotUngTuyenData.Add(soLuotUngTuyen);
                
                var soNganhNghe = danhSachNganhNghe.Count(n => 
                    n.NgayTao >= thangBatDau && n.NgayTao <= thangKetThuc);
                nganhNgheData.Add(soNganhNghe);
                
                var soLoaiCongViec = danhSachLoaiCongViec.Count(l => 
                    l.NgayTao >= thangBatDau && l.NgayTao <= thangKetThuc);
                loaiCongViecData.Add(soLoaiCongViec);
            }
            
            ViewBag.DanhSachNhaTuyenDung = danhSachNhaTuyenDung;
            ViewBag.DanhSachUngVien = danhSachUngVien;
            ViewBag.DanhSachTinTuyenDung = danhSachTinTuyenDung;
            ViewBag.DanhSachTinUngTuyen = danhSachTinUngTuyen;
            ViewBag.DanhSachLoaiCongViec = danhSachLoaiCongViec;
            ViewBag.DanhSachNganhNghe = danhSachNganhNghe;
            ViewBag.SoTinDaDangDict = soTinDaDangDict;
            ViewBag.SoLuongCongViecDict = soLuongCongViecDict;
            ViewBag.SoLuongViTriDict = soLuongViTriDict;
            ViewBag.ThangLabels = thangLabels;
            ViewBag.UngVienData = ungVienData;
            ViewBag.NhaTuyenDungData = nhaTuyenDungData;
            ViewBag.TinDangData = tinDangData;
            ViewBag.LuotUngTuyenData = luotUngTuyenData;
            ViewBag.NganhNgheData = nganhNgheData;
            ViewBag.LoaiCongViecData = loaiCongViecData;
            return View();
        }

        public IActionResult UngVien(string? search = null, string? nganhNghe = null, string? kinhNghiem = null, int pageNumber = 1, int pageSize = 20)
        {
            // Lấy toàn bộ dữ liệu
            var danhSachUngVien = _ungVienRepository.LayDanhSachUngVien();
            
            // Lấy danh sách ngành nghề từ database
            var danhSachNganhNghe = _nganhNgheRepository.LayDanhSachNganhNghe();
            
            // Lấy danh sách kinh nghiệm từ dropdown options
            var danhSachKinhNghiem = DropdownOptions.KinhNghiem;
            
            // Lấy danh sách tin ứng tuyển để tính số lần ứng tuyển
            var danhSachTinUngTuyen = _tinUngTuyenRepository.LayDanhSachTinUngTuyen();
            
            // Tính số lần ứng tuyển cho mỗi ứng viên từ bảng TinUngTuyen
            var soLanUngTuyenDict = danhSachTinUngTuyen
                .Where(t => !string.IsNullOrEmpty(t.UserId))
                .GroupBy(t => t.UserId)
                .ToDictionary(g => g.Key, g => g.Count());
            
            // Gán số lần ứng tuyển thực tế vào từng ứng viên
            foreach (var ungVien in danhSachUngVien)
            {
                if (soLanUngTuyenDict.TryGetValue(ungVien.UserId, out var soLan))
                {
                    ungVien.SoLanUngTuyen = soLan;
                }
                else
                {
                    ungVien.SoLanUngTuyen = 0;
                }
            }
            
            // Tính stats từ toàn bộ dữ liệu (trước khi lọc)
            var totalUngVien = danhSachUngVien.Count;
            var totalUngVienIT = danhSachUngVien.Count(u => u.NganhNghe == "Cong nghe thong tin");
            var totalUngTuyen = danhSachUngVien.Sum(u => u.SoLanUngTuyen);
            var trungBinhUngTuyenPerUngVien = totalUngVien > 0 
                ? (int)(totalUngTuyen / (double)totalUngVien) 
                : 0;
            
            // Lọc theo tìm kiếm
            if (!string.IsNullOrEmpty(search))
            {
                danhSachUngVien = danhSachUngVien.Where(u => 
                    (u.HoTen != null && u.HoTen.Contains(search, StringComparison.OrdinalIgnoreCase)) ||
                    (u.Email != null && u.Email.Contains(search, StringComparison.OrdinalIgnoreCase)) ||
                    (u.SoDienThoai != null && u.SoDienThoai.Contains(search, StringComparison.OrdinalIgnoreCase))
                ).ToList();
            }
            
            // Lọc theo ngành nghề
            if (!string.IsNullOrEmpty(nganhNghe))
            {
                danhSachUngVien = danhSachUngVien.Where(u => 
                    u.NganhNghe != null && u.NganhNghe.Equals(nganhNghe, StringComparison.OrdinalIgnoreCase)
                ).ToList();
            }
            
            // Lọc theo kinh nghiệm (sử dụng KinhNghiemChiTiet nếu có)
            if (!string.IsNullOrEmpty(kinhNghiem))
            {
                danhSachUngVien = danhSachUngVien.Where(u => 
                    u.KinhNghiemChiTiet != null && u.KinhNghiemChiTiet.Contains(kinhNghiem, StringComparison.OrdinalIgnoreCase)
                ).ToList();
            }
            
            // Tính toán phân trang
            var totalCount = danhSachUngVien.Count;
            var pagedList = danhSachUngVien
                .OrderBy(u => u.HoTen)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            
            var viewModel = new UngVienManagementViewModel
            {
                UngViens = pagedList,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                SearchTerm = search,
                NganhNgheFilter = nganhNghe,
                KinhNghiemFilter = kinhNghiem,
                DanhSachNganhNghe = danhSachNganhNghe,
                DanhSachKinhNghiem = danhSachKinhNghiem,
                TotalUngVien = totalUngVien,
                TotalUngVienIT = totalUngVienIT,
                TotalUngTuyen = totalUngTuyen,
                TrungBinhUngTuyenPerUngVien = trungBinhUngTuyenPerUngVien
            };
            
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_UngVienContent", viewModel);
            }
            
            return View(viewModel);
        }

        public IActionResult NhaTuyenDung(string? search = null, string? tinhThanhPho = null, string? quanHuyen = null, int pageNumber = 1, int pageSize = 20)
        {
            // Lấy toàn bộ dữ liệu
            var danhSachNhaTuyenDung = _nhaTuyenDungRepository.LayDanhSachNhaTuyenDung();
            var danhSachTinTuyenDung = _tinTuyenDungRepository.LayDanhSachTinTuyenDung();
            var danhSachTinUngTuyen = _tinUngTuyenRepository.LayDanhSachTinUngTuyen();
            var danhSachTinhThanh = _context.Provinces
                .OrderBy(p => p.FullName)
                .ToList();
            
            // Tính stats từ toàn bộ dữ liệu (trước khi lọc)
            var totalNhaTuyenDung = danhSachNhaTuyenDung.Count;
            var totalTinDang = danhSachTinTuyenDung.Count;
            var totalUngVienNhan = danhSachTinUngTuyen.Count;
            var trungBinhTinPerNTD = totalNhaTuyenDung > 0 
                ? (int)(totalTinDang / (double)totalNhaTuyenDung) 
                : 0;
            
            // Lọc theo tìm kiếm
            if (!string.IsNullOrEmpty(search))
            {
                danhSachNhaTuyenDung = danhSachNhaTuyenDung.Where(n => 
                    (n.TenCongTy != null && n.TenCongTy.Contains(search, StringComparison.OrdinalIgnoreCase)) ||
                    (n.Email != null && n.Email.Contains(search, StringComparison.OrdinalIgnoreCase)) ||
                    (n.NguoiDaiDien != null && n.NguoiDaiDien.Contains(search, StringComparison.OrdinalIgnoreCase))
                ).ToList();
            }
            
            // Lọc theo tỉnh/thành phố
            if (!string.IsNullOrEmpty(tinhThanhPho))
            {
                danhSachNhaTuyenDung = danhSachNhaTuyenDung.Where(n => 
                    n.TinhThanhPho != null && n.TinhThanhPho.Equals(tinhThanhPho, StringComparison.OrdinalIgnoreCase)
                ).ToList();
            }
            
            // Lọc theo quận/huyện
            if (!string.IsNullOrEmpty(quanHuyen))
            {
                danhSachNhaTuyenDung = danhSachNhaTuyenDung.Where(n => 
                    n.QuanHuyen != null && n.QuanHuyen.Equals(quanHuyen, StringComparison.OrdinalIgnoreCase)
                ).ToList();
            }
            
            // Tính toán phân trang
            var totalCount = danhSachNhaTuyenDung.Count;
            var pagedList = danhSachNhaTuyenDung
                .OrderBy(n => n.TenCongTy)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            
            var viewModel = new NhaTuyenDungManagementViewModel
            {
                NhaTuyenDungs = pagedList,
                DanhSachTinTuyenDung = danhSachTinTuyenDung,
                DanhSachTinUngTuyen = danhSachTinUngTuyen,
                DanhSachTinhThanh = danhSachTinhThanh,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                SearchTerm = search,
                TinhThanhPhoFilter = tinhThanhPho,
                QuanHuyenFilter = quanHuyen,
                TotalNhaTuyenDung = totalNhaTuyenDung,
                TotalTinDang = totalTinDang,
                TotalUngVienNhan = totalUngVienNhan,
                TrungBinhTinPerNTD = trungBinhTinPerNTD
            };
            
            return View(viewModel);
        }

        // GET: Chi tiết nhà tuyển dụng
        public IActionResult ChiTietNhaTuyenDung(int id, int pageNumber = 1, int pageSize = 10, 
            string? searchTerm = null, string? trangThaiFilter = null, string? nganhNgheFilter = null, 
            string? loaiCongViecFilter = null, string? activeTab = null)
        {
            ViewData["Title"] = "Chi tiết Nhà tuyển dụng";
            
            var nhaTuyenDung = _nhaTuyenDungRepository.LayNhaTuyenDungTheoId(id);
            if (nhaTuyenDung == null)
            {
                TempData["Loi"] = "Không tìm thấy nhà tuyển dụng!";
                return RedirectToAction("NhaTuyenDung");
            }
            
            // Lấy danh sách tin tuyển dụng theo mã nhà tuyển dụng (dùng foreign key)
            var danhSachTinTuyenDung = _tinTuyenDungRepository.LayDanhSachTheoMaNhaTuyenDung(id);
            
            // Tính toán số tin đã đăng từ dữ liệu thật (trước khi lọc)
            var soTinDaDang = danhSachTinTuyenDung.Count;
            
            // Lấy danh sách mã tin tuyển dụng của công ty
            var danhSachMaTin = danhSachTinTuyenDung.Select(t => t.MaTinTuyenDung.ToString()).ToList();
            
            // Tính tổng số đơn ứng tuyển mà công ty nhận được (tất cả trạng thái)
            var danhSachTinUngTuyen = _tinUngTuyenRepository.LayDanhSachTinUngTuyen();
            var soUngVienNhan = danhSachTinUngTuyen
                .Where(t => danhSachMaTin.Contains(t.MaTinTuyenDung))
                .Count();
            
            // Lấy danh sách ngành nghề và loại công việc từ toàn bộ danh sách (trước khi lọc) để hiển thị trong filter
            var danhSachNganhNghe = danhSachTinTuyenDung
                .Where(t => !string.IsNullOrEmpty(t.NganhNghe))
                .Select(t => t.NganhNghe)
                .Distinct()
                .OrderBy(n => n)
                .ToList();
            
            var danhSachLoaiCongViec = danhSachTinTuyenDung
                .Where(t => !string.IsNullOrEmpty(t.LoaiCongViec))
                .Select(t => t.LoaiCongViec)
                .Distinct()
                .OrderBy(l => l)
                .ToList();
            
            // Lọc theo tìm kiếm
            if (!string.IsNullOrEmpty(searchTerm))
            {
                danhSachTinTuyenDung = danhSachTinTuyenDung.Where(t => 
                    (t.TenViecLam != null && t.TenViecLam.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                    (t.CongTy != null && t.CongTy.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                ).ToList();
            }
            
            // Lọc theo trạng thái
            if (!string.IsNullOrEmpty(trangThaiFilter))
            {
                danhSachTinTuyenDung = danhSachTinTuyenDung.Where(t =>
                {
                    var trangThai = t.HanNop >= DateTime.Now ? "Dang tuyen" : "Het han";
                    return trangThai == trangThaiFilter;
                }).ToList();
            }
            
            // Lọc theo ngành nghề
            if (!string.IsNullOrEmpty(nganhNgheFilter))
            {
                danhSachTinTuyenDung = danhSachTinTuyenDung.Where(t => 
                    !string.IsNullOrEmpty(t.NganhNghe) && 
                    t.NganhNghe.Equals(nganhNgheFilter, StringComparison.OrdinalIgnoreCase)
                ).ToList();
            }
            
            // Lọc theo loại công việc
            if (!string.IsNullOrEmpty(loaiCongViecFilter))
            {
                danhSachTinTuyenDung = danhSachTinTuyenDung.Where(t => 
                    !string.IsNullOrEmpty(t.LoaiCongViec) && 
                    t.LoaiCongViec.Equals(loaiCongViecFilter, StringComparison.OrdinalIgnoreCase)
                ).ToList();
            }
            
            // Sắp xếp theo ngày đăng (mới nhất trước)
            danhSachTinTuyenDung = danhSachTinTuyenDung.OrderByDescending(t => t.NgayDang).ToList();
            
            // Tính toán phân trang
            var totalCount = danhSachTinTuyenDung.Count;
            var pagedList = danhSachTinTuyenDung.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            
            // Truyền dữ liệu vào ViewBag
            ViewBag.DanhSachTinTuyenDung = pagedList;
            ViewBag.SoTinDaDang = soTinDaDang;
            ViewBag.SoUngVienNhan = soUngVienNhan;
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalCount = totalCount;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            ViewBag.SearchTerm = searchTerm;
            ViewBag.TrangThaiFilter = trangThaiFilter;
            ViewBag.NganhNgheFilter = nganhNgheFilter;
            ViewBag.LoaiCongViecFilter = loaiCongViecFilter;
            ViewBag.DanhSachNganhNghe = danhSachNganhNghe;
            ViewBag.DanhSachLoaiCongViec = danhSachLoaiCongViec;
            ViewBag.ActiveTab = activeTab ?? "thongtin"; // Mặc định là tab thông tin
            
            // Lấy danh sách tỉnh/thành phố cho dropdown
            var danhSachTinhThanh = _context.Provinces
                .OrderBy(p => p.FullName)
                .ToList();
            ViewBag.DanhSachTinhThanh = danhSachTinhThanh;
            
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

        private async Task<string?> DownloadImageFromUrlAsync(string imageUrl, string articleId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(imageUrl))
                    return null;

                if (!Uri.TryCreate(imageUrl, UriKind.Absolute, out var uri))
                {
                    _logger.LogWarning("Invalid image URL: {ImageUrl}", imageUrl);
                    return null;
                }

                using var httpClient = new HttpClient();
                httpClient.Timeout = TimeSpan.FromSeconds(30);
                httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36");

                var response = await httpClient.GetAsync(imageUrl);
                
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Failed to download image from {ImageUrl}: {StatusCode}", imageUrl, response.StatusCode);
                    return null;
                }

                var contentType = response.Content.Headers.ContentType?.MediaType;
                var allowedTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/gif", "image/webp" };
                
                if (contentType == null || !allowedTypes.Contains(contentType.ToLower()))
                {
                    _logger.LogWarning("Invalid content type for image: {ContentType}", contentType);
                    return null;
                }

                var extension = contentType.ToLower() switch
                {
                    "image/jpeg" => ".jpg",
                    "image/jpg" => ".jpg",
                    "image/png" => ".png",
                    "image/gif" => ".gif",
                    "image/webp" => ".webp",
                    _ => ".jpg"
                };

                var bytes = await response.Content.ReadAsByteArrayAsync();
                
                if (bytes.Length > 10 * 1024 * 1024)
                {
                    _logger.LogWarning("Image too large: {Size} bytes", bytes.Length);
                    return null;
                }

                var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "blogs");
                if (!Directory.Exists(uploadsPath))
                {
                    Directory.CreateDirectory(uploadsPath);
                }

                var safeArticleId = articleId.Replace("/", "_").Replace("\\", "_").Replace(":", "_");
                var fileName = $"api_{safeArticleId}_{DateTime.Now:yyyyMMddHHmmss}{extension}";
                var filePath = Path.Combine(uploadsPath, fileName);

                await System.IO.File.WriteAllBytesAsync(filePath, bytes);

                _logger.LogInformation("Successfully downloaded image from {ImageUrl} to {FilePath}", imageUrl, fileName);
                return $"/uploads/blogs/{fileName}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error downloading image from {ImageUrl}", imageUrl);
                return null;
            }
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

        public IActionResult NganhNghe(int pageNumber = 1, int pageSize = 10, string? searchTerm = null)
        {
            // Lấy toàn bộ danh sách ngành nghề
            var danhSachNganhNghe = _nganhNgheRepository.LayDanhSachNganhNghe();
            var danhSachTinTuyenDung = _tinTuyenDungRepository.LayDanhSachTinTuyenDung();
            
            // Tính số lượng công việc cho mỗi ngành nghề dựa trên số tin tuyển dụng
            foreach (var nganhNghe in danhSachNganhNghe)
            {
                nganhNghe.SoLuongCongViec = danhSachTinTuyenDung
                    .Count(t => !string.IsNullOrEmpty(t.NganhNghe) && 
                                t.NganhNghe.Equals(nganhNghe.TenNganhNghe, StringComparison.OrdinalIgnoreCase));
            }
            
            // Tính stats từ toàn bộ dữ liệu (trước khi lọc)
            var totalNganhNghe = danhSachNganhNghe.Count;
            var totalCongViec = danhSachNganhNghe.Sum(n => n.SoLuongCongViec);
            var trungBinhCongViec = danhSachNganhNghe.Count > 0 
                ? (int)danhSachNganhNghe.Average(n => n.SoLuongCongViec) 
                : 0;
            
            // Lọc theo tìm kiếm
            if (!string.IsNullOrEmpty(searchTerm))
            {
                danhSachNganhNghe = danhSachNganhNghe.Where(n => 
                    (n.TenNganhNghe != null && n.TenNganhNghe.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                    (n.MoTa != null && n.MoTa.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                ).ToList();
            }
            
            // Sắp xếp theo tên
            danhSachNganhNghe = danhSachNganhNghe.OrderBy(n => n.TenNganhNghe).ToList();
            
            // Tính toán phân trang
            var totalCount = danhSachNganhNghe.Count;
            var pagedList = danhSachNganhNghe.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            
            var model = new Models.ViewModels.NganhNgheAdminViewModel
            {
                NganhNghes = pagedList,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                SearchTerm = searchTerm,
                TotalNganhNghe = totalNganhNghe,
                TotalCongViec = totalCongViec,
                TrungBinhCongViec = trungBinhCongViec
            };
            
            return View(model);
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

                // Tạo object NganhNghe tạm thời từ các tham số để sử dụng AutoMapper
                var nganhNgheNguon = new NganhNghe
                {
                    TenNganhNghe = tenNganhNghe,
                    MoTa = string.IsNullOrWhiteSpace(moTa) ? null : moTa
                };

                // Cập nhật thông tin bằng AutoMapper
                // AutoMapper sẽ tự động map các thuộc tính và bỏ qua các thuộc tính đặc biệt
                _mapper.Map(nganhNgheNguon, nganhNghe);

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

        public IActionResult LoaiCongViec(int pageNumber = 1, int pageSize = 10, string? searchTerm = null)
        {
            // Lấy toàn bộ danh sách loại công việc
            var danhSachLoaiCongViec = _loaiCongViecRepository.LayDanhSachLoaiCongViec();
            var danhSachTinTuyenDung = _tinTuyenDungRepository.LayDanhSachTinTuyenDung();
            
            // Tính số lượng vị trí cho mỗi loại công việc dựa trên số tin tuyển dụng
            foreach (var loaiCongViec in danhSachLoaiCongViec)
            {
                loaiCongViec.SoLuongViTri = danhSachTinTuyenDung
                    .Count(t => !string.IsNullOrEmpty(t.LoaiCongViec) && 
                                t.LoaiCongViec.Equals(loaiCongViec.TenLoaiCongViec, StringComparison.OrdinalIgnoreCase));
            }
            
            // Tính stats từ toàn bộ dữ liệu (trước khi lọc)
            var totalLoaiCongViec = danhSachLoaiCongViec.Count;
            var totalViTri = danhSachLoaiCongViec.Sum(l => l.SoLuongViTri);
            var trungBinhViTri = danhSachLoaiCongViec.Count > 0 
                ? (int)danhSachLoaiCongViec.Average(l => l.SoLuongViTri) 
                : 0;
            
            // Lọc theo tìm kiếm
            if (!string.IsNullOrEmpty(searchTerm))
            {
                danhSachLoaiCongViec = danhSachLoaiCongViec.Where(l => 
                    (l.TenLoaiCongViec != null && l.TenLoaiCongViec.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                    (l.MoTa != null && l.MoTa.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                ).ToList();
            }
            
            // Sắp xếp theo tên
            danhSachLoaiCongViec = danhSachLoaiCongViec.OrderBy(l => l.TenLoaiCongViec).ToList();
            
            // Tính toán phân trang
            var totalCount = danhSachLoaiCongViec.Count;
            var pagedList = danhSachLoaiCongViec.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            
            var model = new Models.ViewModels.LoaiCongViecAdminViewModel
            {
                LoaiCongViecs = pagedList,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                SearchTerm = searchTerm,
                TotalLoaiCongViec = totalLoaiCongViec,
                TotalViTri = totalViTri,
                TrungBinhViTri = trungBinhViTri
            };
            
            return View(model);
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

                // Tạo object LoaiCongViec tạm thời từ các tham số để sử dụng AutoMapper
                var loaiCongViecNguon = new LoaiCongViec
                {
                    TenLoaiCongViec = tenLoaiCongViec,
                    MoTa = string.IsNullOrWhiteSpace(moTa) ? string.Empty : moTa
                };

                // Cập nhật thông tin bằng AutoMapper
                // AutoMapper sẽ tự động map các thuộc tính và bỏ qua các thuộc tính đặc biệt
                _mapper.Map(loaiCongViecNguon, loaiCongViec);

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

        public IActionResult ChuyenNganh(int pageNumber = 1, int pageSize = 10, string? searchTerm = null, int? maNganhNgheFilter = null)
        {
            // Lấy toàn bộ danh sách chuyên ngành
            var danhSachChuyenNganh = _chuyenNganhRepository.LayDanhSachChuyenNganh();
            var danhSachNganhNghe = _nganhNgheRepository.LayDanhSachNganhNghe();
            
            // Tính stats từ toàn bộ dữ liệu (trước khi lọc)
            var totalChuyenNganh = danhSachChuyenNganh.Count;
            
            // Lọc theo tìm kiếm
            if (!string.IsNullOrEmpty(searchTerm))
            {
                danhSachChuyenNganh = danhSachChuyenNganh.Where(c => 
                    (c.TenChuyenNganh != null && c.TenChuyenNganh.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                    (c.MoTa != null && c.MoTa.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                ).ToList();
            }
            
            // Lọc theo ngành nghề
            if (maNganhNgheFilter.HasValue && maNganhNgheFilter.Value > 0)
            {
                danhSachChuyenNganh = danhSachChuyenNganh.Where(c => c.MaNganhNghe == maNganhNgheFilter.Value).ToList();
            }
            
            // Sắp xếp theo ngành nghề và tên chuyên ngành
            danhSachChuyenNganh = danhSachChuyenNganh
                .OrderBy(c => c.NganhNghe != null ? c.NganhNghe.TenNganhNghe : "")
                .ThenBy(c => c.TenChuyenNganh)
                .ToList();
            
            // Tính toán phân trang
            var totalCount = danhSachChuyenNganh.Count;
            var pagedList = danhSachChuyenNganh.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            
            var model = new Models.ViewModels.ChuyenNganhAdminViewModel
            {
                ChuyenNganhs = pagedList,
                DanhSachNganhNghe = danhSachNganhNghe,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                SearchTerm = searchTerm,
                MaNganhNgheFilter = maNganhNgheFilter,
                TotalChuyenNganh = totalChuyenNganh
            };
            
            return View(model);
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

                // Tạo object ChuyenNganh tạm thời từ các tham số để sử dụng AutoMapper
                var chuyenNganhNguon = new ChuyenNganh
                {
                    TenChuyenNganh = tenChuyenNganh,
                    MoTa = moTa,
                    MaNganhNghe = maNganhNghe
                };

                // Cập nhật thông tin bằng AutoMapper
                // AutoMapper sẽ tự động map các thuộc tính và bỏ qua các thuộc tính đặc biệt
                _mapper.Map(chuyenNganhNguon, chuyenNganh);

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

        public IActionResult TruongDaiHoc(int pageNumber = 1, int pageSize = 10, string? searchTerm = null)
        {
            // Lấy toàn bộ danh sách trường đại học
            var danhSachTruongDaiHoc = _truongDaiHocRepository.LayDanhSachTruongDaiHoc();
            
            // Tính stats từ toàn bộ dữ liệu (trước khi lọc)
            var totalTruongDaiHoc = danhSachTruongDaiHoc.Count;
            
            // Lọc theo tìm kiếm
            if (!string.IsNullOrEmpty(searchTerm))
            {
                danhSachTruongDaiHoc = danhSachTruongDaiHoc.Where(t => 
                    (t.TenTruong != null && t.TenTruong.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                    (t.MoTa != null && t.MoTa.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                ).ToList();
            }
            
            // Sắp xếp theo tên
            danhSachTruongDaiHoc = danhSachTruongDaiHoc.OrderBy(t => t.TenTruong).ToList();
            
            // Tính toán phân trang
            var totalCount = danhSachTruongDaiHoc.Count;
            var pagedList = danhSachTruongDaiHoc.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            
            var model = new Models.ViewModels.TruongDaiHocAdminViewModel
            {
                TruongDaiHocs = pagedList,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                SearchTerm = searchTerm,
                TotalTruongDaiHoc = totalTruongDaiHoc
            };
            
            return View(model);
        }

        public IActionResult TinTuyenDung(int pageNumber = 1, int pageSize = 10, string? searchTerm = null, string? trangThaiFilter = null, string? nganhNgheFilter = null)
        {
            var danhSachTinTuyenDung = _tinTuyenDungRepository.LayDanhSachTinTuyenDung();
            var danhSachTinUngTuyen = _tinUngTuyenRepository.LayDanhSachTinUngTuyen();
            
            var soLuongUngTuyenDict = danhSachTinUngTuyen
                .Where(t => !string.IsNullOrEmpty(t.MaTinTuyenDung))
                .GroupBy(t => t.MaTinTuyenDung)
                .ToDictionary(g => g.Key, g => g.Count());
            
            foreach (var tin in danhSachTinTuyenDung)
            {
                var maTinString = tin.MaTinTuyenDung.ToString();
                if (soLuongUngTuyenDict.TryGetValue(maTinString, out var soLuong))
                {
                    tin.SoLuongUngTuyen = soLuong;
                }
                else
                {
                    tin.SoLuongUngTuyen = 0;
                }
            }
            
            var totalDangTuyen = danhSachTinTuyenDung.Count(t => t.HanNop >= DateTime.Now);
            var totalHetHan = danhSachTinTuyenDung.Count(t => t.HanNop < DateTime.Now);
            var totalUngTuyen = danhSachTinTuyenDung.Sum(t => t.SoLuongUngTuyen ?? 0);
            var totalSapHetHan = danhSachTinTuyenDung.Count(t => t.HanNop >= DateTime.Now && (t.HanNop - DateTime.Now).Days <= 7);
            var trungBinhUngTuyen = danhSachTinTuyenDung.Count > 0 
                ? (int)danhSachTinTuyenDung.Average(t => t.SoLuongUngTuyen ?? 0) 
                : 0;
            var totalChoDuyet = danhSachTinTuyenDung.Count(t => t.TrangThaiDuyet == "Cho duyet");
            var totalDaDuyet = danhSachTinTuyenDung.Count(t => t.TrangThaiDuyet == "Da duyet");
            var totalTuChoi = danhSachTinTuyenDung.Count(t => t.TrangThaiDuyet == "Tu choi");
            
            // Lọc theo tìm kiếm
            if (!string.IsNullOrEmpty(searchTerm))
            {
                danhSachTinTuyenDung = danhSachTinTuyenDung.Where(t => 
                    (t.TenViecLam != null && t.TenViecLam.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                    (t.CongTy != null && t.CongTy.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                ).ToList();
            }
            
            // Lọc theo trạng thái
            if (!string.IsNullOrEmpty(trangThaiFilter))
            {
                var approvalFilters = new[] { "Cho duyet", "Da duyet", "Tu choi", "Bi khoa" };
                if (approvalFilters.Contains(trangThaiFilter))
                {
                    danhSachTinTuyenDung = danhSachTinTuyenDung
                        .Where(t => t.TrangThaiDuyet == trangThaiFilter)
                        .ToList();
                }
                else
                {
                    danhSachTinTuyenDung = danhSachTinTuyenDung.Where(t =>
                    {
                        var trangThai = t.HanNop >= DateTime.Now ? "Dang tuyen" : "Het han";
                        return trangThai == trangThaiFilter;
                    }).ToList();
                }
            }
            
            // Lọc theo ngành nghề
            if (!string.IsNullOrEmpty(nganhNgheFilter))
            {
                danhSachTinTuyenDung = danhSachTinTuyenDung.Where(t => 
                    !string.IsNullOrEmpty(t.NganhNghe) && 
                    t.NganhNghe.Equals(nganhNgheFilter, StringComparison.OrdinalIgnoreCase)
                ).ToList();
            }
            
            // Sắp xếp theo ngày đăng (mới nhất trước)
            danhSachTinTuyenDung = danhSachTinTuyenDung.OrderByDescending(t => t.NgayDang).ToList();
            
            // Tính toán phân trang
            var totalCount = danhSachTinTuyenDung.Count;
            var pagedList = danhSachTinTuyenDung.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            
            var model = new Models.ViewModels.TinTuyenDungAdminViewModel
            {
                TinTuyenDungs = pagedList,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                SearchTerm = searchTerm,
                TrangThaiFilter = trangThaiFilter,
                NganhNgheFilter = nganhNgheFilter,
                TotalDangTuyen = totalDangTuyen,
                TotalHetHan = totalHetHan,
                TotalUngTuyen = totalUngTuyen,
                TotalSapHetHan = totalSapHetHan,
                TrungBinhUngTuyen = trungBinhUngTuyen,
                TotalChoDuyet = totalChoDuyet,
                TotalDaDuyet = totalDaDuyet,
                TotalTuChoi = totalTuChoi
            };
            
            return View(model);
        }

        // GET: Chi tiết tin tuyển dụng
        public IActionResult ChiTietTinTuyenDung(int id, int pageNumber = 1, int pageSize = 10, 
            string? searchTerm = null, string? trangThaiFilter = null, string? tab = null)
        {
            ViewData["Title"] = "Chi tiết tin tuyển dụng";
            
            var tinTuyenDung = _tinTuyenDungRepository.LayTinTuyenDungTheoId(id);
            if (tinTuyenDung == null)
            {
                TempData["Loi"] = "Không tìm thấy tin tuyển dụng!";
                return RedirectToAction("TinTuyenDung");
            }
            
            // Lấy danh sách ứng tuyển theo mã tin
            var danhSachUngTuyenAll = _tinUngTuyenRepository.LayDanhSachTinUngTuyen()
                .Where(t => t.MaTinTuyenDung == id.ToString())
                .ToList();
            
            // Tính tổng số đơn ứng tuyển theo trạng thái (trước khi lọc)
            var totalDangXemXet = danhSachUngTuyenAll.Count(t => 
                t.TrangThaiXuLy == "Đang xem xét" || t.TrangThaiXuLy == "Dang xem xet");
            var totalChoPhongVan = danhSachUngTuyenAll.Count(t => 
                t.TrangThaiXuLy == "Chờ phỏng vấn" || t.TrangThaiXuLy == "Cho phong van");
            var totalTuyenDung = danhSachUngTuyenAll.Count(t => 
                t.TrangThaiXuLy == "Tuyển dụng" || t.TrangThaiXuLy == "Tuyen dung");
            
            // Lọc theo tìm kiếm
            if (!string.IsNullOrEmpty(searchTerm))
            {
                danhSachUngTuyenAll = danhSachUngTuyenAll.Where(t =>
                    (t.HoTen != null && t.HoTen.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                    (t.Email != null && t.Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                    (t.SoDienThoai != null && t.SoDienThoai.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                ).ToList();
            }
            
            // Lọc theo trạng thái
            if (!string.IsNullOrEmpty(trangThaiFilter))
            {
                danhSachUngTuyenAll = danhSachUngTuyenAll.Where(t =>
                    t.TrangThaiXuLy == trangThaiFilter ||
                    t.TrangThaiXuLy == trangThaiFilter.Replace(" ", "")
                ).ToList();
            }
            
            // Sắp xếp theo ngày ứng tuyển (mới nhất trước)
            danhSachUngTuyenAll = danhSachUngTuyenAll.OrderByDescending(t => t.NgayUngTuyen).ToList();
            
            // Tính toán phân trang
            var totalCount = danhSachUngTuyenAll.Count;
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            var danhSachUngTuyenPaged = danhSachUngTuyenAll
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            
            // Lấy danh sách tỉnh/thành phố
            var danhSachTinhThanh = _context.Provinces
                .OrderBy(p => p.FullName)
                .ToList();
            
            // Lấy danh sách ngành nghề
            var danhSachNganhNghe = _nganhNgheRepository.LayDanhSachNganhNghe();
            
            // Lấy danh sách loại công việc
            var danhSachLoaiCongViec = _loaiCongViecRepository.LayDanhSachLoaiCongViec();
            
            // Lấy danh sách chuyên ngành
            var danhSachChuyenNganh = _chuyenNganhRepository.LayDanhSachChuyenNganh();
            
            ViewBag.DanhSachUngTuyen = danhSachUngTuyenPaged;
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalCount = totalCount;
            ViewBag.TotalPages = totalPages;
            ViewBag.SearchTerm = searchTerm;
            ViewBag.TrangThaiFilter = trangThaiFilter;
            ViewBag.TotalDangXemXet = totalDangXemXet;
            ViewBag.TotalChoPhongVan = totalChoPhongVan;
            ViewBag.TotalTuyenDung = totalTuyenDung;
            ViewBag.DanhSachTinhThanh = danhSachTinhThanh;
            ViewBag.DanhSachNganhNghe = danhSachNganhNghe;
            ViewBag.DanhSachLoaiCongViec = danhSachLoaiCongViec;
            ViewBag.DanhSachChuyenNganh = danhSachChuyenNganh;
            
            return View(tinTuyenDung);
        }

        // POST: Duyệt tin tuyển dụng
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DuyetTinTuyenDung(int id)
        {
            try
            {
                var tin = _tinTuyenDungRepository.CapNhatTrangThaiDuyet(id, "Da duyet", null, DateTime.Now);
                if (tin == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy tin tuyển dụng!" });
                }

                return Json(new { success = true, message = $"Đã duyệt tin #{tin.MaTinTuyenDung} thành công!" });
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
                    var tin = _tinTuyenDungRepository.CapNhatTrangThaiDuyet(ma, "Da duyet", null, DateTime.Now);
                    if (tin != null)
                    {
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult TuChoiTinTuyenDung(int id, string? lyDoTuChoi)
        {
            try
            {
                var lyDo = string.IsNullOrWhiteSpace(lyDoTuChoi) ? null : lyDoTuChoi.Trim();
                var tin = _tinTuyenDungRepository.CapNhatTrangThaiDuyet(id, "Tu choi", lyDo, DateTime.Now);
                if (tin == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy tin tuyển dụng!" });
                }

                return Json(new { success = true, message = $"Đã từ chối tin #{tin.MaTinTuyenDung}!" });
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

        // GET: Chi tiết ứng viên
        public IActionResult ChiTietUngVien(int id)
        {
            ViewData["Title"] = "Chi tiết Ứng viên";
            
            var ungVien = _ungVienRepository.LayUngVienTheoId(id);
            if (ungVien == null)
            {
                TempData["Loi"] = "Không tìm thấy ứng viên!";
                return RedirectToAction("UngVien");
            }

            // Load dữ liệu cho dropdowns (nếu cần)
            var danhSachChuyenNganh = _chuyenNganhRepository.LayDanhSachChuyenNganh();
            var danhSachNganhNghe = _nganhNgheRepository.LayDanhSachNganhNghe();
            var danhSachTinhThanh = _context.Provinces.OrderBy(p => p.FullName).ToList();
            
            ViewBag.DanhSachChuyenNganh = danhSachChuyenNganh;
            ViewBag.DanhSachNganhNghe = danhSachNganhNghe;
            ViewBag.DanhSachTinhThanh = danhSachTinhThanh;

            // Lấy danh sách tin tuyển dụng đã ứng tuyển
            var danhSachTinUngTuyen = _tinUngTuyenRepository.LayDanhSachTinUngTuyen()
                .Where(t => t.UserId == ungVien.UserId)
                .ToList();

            var danhSachTinTuyenDung = new List<object>();
            foreach (var ungTuyen in danhSachTinUngTuyen)
            {
                var tinTuyenDung = _tinTuyenDungRepository.LayTinTuyenDungTheoId(int.Parse(ungTuyen.MaTinTuyenDung));
                if (tinTuyenDung != null)
                {
                    danhSachTinTuyenDung.Add(new
                    {
                        MaTinUngTuyen = ungTuyen.MaTinUngTuyen,
                        MaTin = tinTuyenDung.MaTinTuyenDung,
                        TenViecLam = tinTuyenDung.TenViecLam,
                        CongTy = tinTuyenDung.CongTy,
                        NganhNghe = tinTuyenDung.NganhNghe,
                        LoaiCongViec = tinTuyenDung.LoaiCongViec,
                        TinhThanhPho = tinTuyenDung.TinhThanhPho,
                        MucLuong = tinTuyenDung.MucLuongThapNhat.HasValue && tinTuyenDung.MucLuongCaoNhat.HasValue
                            ? $"{tinTuyenDung.MucLuongThapNhat.Value:F0} - {tinTuyenDung.MucLuongCaoNhat.Value:F0} triệu"
                            : tinTuyenDung.MucLuongThapNhat.HasValue
                            ? $"Từ {tinTuyenDung.MucLuongThapNhat.Value:F0} triệu"
                            : "Thỏa thuận",
                        NgayDang = tinTuyenDung.NgayDang.ToString("dd/MM/yyyy"),
                        HanNop = tinTuyenDung.HanNop.ToString("dd/MM/yyyy"),
                        TrangThaiXuLy = ungTuyen.TrangThaiXuLy,
                        NgayUngTuyen = ungTuyen.NgayUngTuyen.ToString("dd/MM/yyyy"),
                        ViTriUngTuyen = ungTuyen.ViTriUngTuyen
                    });
                }
            }

            ViewBag.DanhSachTinTuyenDung = danhSachTinTuyenDung;
            
            return View(ungVien);
        }

        [HttpGet]
        public IActionResult LayDanhSachTinUngTuyenFiltered(int id, string? search = null, string? trangThai = null, string? nganhNghe = null)
        {
            var ungVien = _ungVienRepository.LayUngVienTheoId(id);
            if (ungVien == null)
            {
                return Json(new { success = false, message = "Không tìm thấy ứng viên!" });
            }

            var danhSachTinUngTuyen = _tinUngTuyenRepository.LayDanhSachTinUngTuyen()
                .Where(t => t.UserId == ungVien.UserId)
                .ToList();

            var danhSachTinTuyenDungAll = new List<object>();
            var danhSachTinTuyenDungFiltered = new List<object>();
            var danhSachNganhNgheAll = new HashSet<string>();
            
            foreach (var ungTuyen in danhSachTinUngTuyen)
            {
                var tinTuyenDung = _tinTuyenDungRepository.LayTinTuyenDungTheoId(int.Parse(ungTuyen.MaTinTuyenDung));
                if (tinTuyenDung != null)
                {
                    var nganhNgheValue = tinTuyenDung.NganhNghe;
                    if (!string.IsNullOrEmpty(nganhNgheValue))
                    {
                        danhSachNganhNgheAll.Add(nganhNgheValue);
                    }

                    var tinData = new
                    {
                        MaTinUngTuyen = ungTuyen.MaTinUngTuyen,
                        MaTin = tinTuyenDung.MaTinTuyenDung,
                        TenViecLam = tinTuyenDung.TenViecLam,
                        CongTy = tinTuyenDung.CongTy,
                        NganhNghe = nganhNgheValue,
                        LoaiCongViec = tinTuyenDung.LoaiCongViec,
                        TinhThanhPho = tinTuyenDung.TinhThanhPho,
                        MucLuong = tinTuyenDung.MucLuongThapNhat.HasValue && tinTuyenDung.MucLuongCaoNhat.HasValue
                            ? $"{tinTuyenDung.MucLuongThapNhat.Value:F0} - {tinTuyenDung.MucLuongCaoNhat.Value:F0} triệu"
                            : tinTuyenDung.MucLuongThapNhat.HasValue
                            ? $"Từ {tinTuyenDung.MucLuongThapNhat.Value:F0} triệu"
                            : "Thỏa thuận",
                        NgayDang = tinTuyenDung.NgayDang.ToString("dd/MM/yyyy"),
                        HanNop = tinTuyenDung.HanNop.ToString("dd/MM/yyyy"),
                        TrangThaiXuLy = ungTuyen.TrangThaiXuLy,
                        NgayUngTuyen = ungTuyen.NgayUngTuyen.ToString("dd/MM/yyyy"),
                        ViTriUngTuyen = ungTuyen.ViTriUngTuyen
                    };

                    danhSachTinTuyenDungAll.Add(tinData);

                    var shouldInclude = true;

                    if (!string.IsNullOrEmpty(search))
                    {
                        var searchLower = search.ToLower();
                        shouldInclude = shouldInclude && (
                            (tinTuyenDung.TenViecLam != null && tinTuyenDung.TenViecLam.ToLower().Contains(searchLower)) ||
                            (tinTuyenDung.CongTy != null && tinTuyenDung.CongTy.ToLower().Contains(searchLower))
                        );
                    }

                    if (!string.IsNullOrEmpty(trangThai))
                    {
                        shouldInclude = shouldInclude && ungTuyen.TrangThaiXuLy == trangThai;
                    }

                    if (!string.IsNullOrEmpty(nganhNghe))
                    {
                        shouldInclude = shouldInclude && nganhNgheValue == nganhNghe;
                    }

                    if (shouldInclude)
                    {
                        danhSachTinTuyenDungFiltered.Add(tinData);
                    }
                }
            }

            ViewBag.DanhSachNganhNgheAll = danhSachNganhNgheAll.OrderBy(n => n).ToList();

            return PartialView("_DanhSachTinUngTuyen", danhSachTinTuyenDungFiltered);
        }

        // POST: Cập nhật thông tin ứng viên
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CapNhatUngVien(
            int id,
            string? hoTen,
            string? email,
            string? soDienThoai,
            DateTime? ngaySinh,
            string? gioiTinh,
            string? diaChi,
            string? viTriMongMuon,
            int? maChuyenNganh,
            string? chuyenNganhKhac,
            decimal? mucLuongKyVong,
            string? noiLamViecMongMuon,
            string? trangThaiTimViec,
            string? mucTieuNgheNghiep,
            string? hocVanChiTiet,
            string? kinhNghiemChiTiet,
            string? kyNangChiTiet,
            string? chungChi,
            string? linkGitHub,
            string? linkBehance,
            string? linkPortfolio,
            string? moTaBanThan)
        {
            try
            {
                var ungVien = _ungVienRepository.LayUngVienTheoId(id);
                if (ungVien == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy ứng viên!" });
                }

                // Xử lý checkbox
                var hienThiCongKhaiValue = Request.Form["hienThiCongKhai"].ToString();
                bool hienThiCongKhai = !string.IsNullOrEmpty(hienThiCongKhaiValue) && 
                                       (hienThiCongKhaiValue.ToLower() == "true" || hienThiCongKhaiValue == "on");

                // Tạo object UngVien tạm thời từ các tham số để sử dụng AutoMapper
                var ungVienNguon = new UngVien
                {
                    HoTen = hoTen ?? string.Empty,
                    Email = email ?? string.Empty,
                    SoDienThoai = soDienThoai ?? string.Empty,
                    NgaySinh = ngaySinh ?? ungVien.NgaySinh,
                    GioiTinh = gioiTinh,
                    DiaChi = diaChi,
                    ViTriMongMuon = viTriMongMuon,
                    MaChuyenNganh = maChuyenNganh,
                    ChuyenNganhKhac = chuyenNganhKhac,
                    MucLuongKyVong = mucLuongKyVong,
                    NoiLamViecMongMuon = noiLamViecMongMuon,
                    TrangThaiTimViec = trangThaiTimViec,
                    HienThiCongKhai = hienThiCongKhai,
                    MucTieuNgheNghiep = mucTieuNgheNghiep,
                    HocVanChiTiet = hocVanChiTiet,
                    KinhNghiemChiTiet = kinhNghiemChiTiet,
                    KyNangChiTiet = kyNangChiTiet,
                    ChungChi = chungChi,
                    LinkGitHub = linkGitHub,
                    LinkBehance = linkBehance,
                    LinkPortfolio = linkPortfolio,
                    MoTaBanThan = moTaBanThan
                };

                // Cập nhật thông tin bằng AutoMapper
                // AutoMapper sẽ tự động map các thuộc tính và bỏ qua các thuộc tính đặc biệt
                _mapper.Map(ungVienNguon, ungVien);

                // Lưu vào database
                var ketQua = _ungVienRepository.CapNhatUngVien(ungVien);
                if (ketQua != null)
                {
                    return Json(new { success = true, message = "Cập nhật thông tin ứng viên thành công!" });
                }
                else
                {
                    return Json(new { success = false, message = "Có lỗi xảy ra khi cập nhật thông tin!" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult XoaTinUngTuyen(int id)
        {
            try
            {
                var result = _tinUngTuyenRepository.XoaTinUngTuyen(id);
                if (result)
                {
                    return Json(new { success = true, message = "Đã xóa tin ứng tuyển thành công!" });
                }
                else
                {
                    return Json(new { success = false, message = "Không tìm thấy tin ứng tuyển!" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        // GET: Chi tiết đơn ứng tuyển
        [HttpGet]
        public IActionResult ChiTietDonUngTuyen(int id)
        {
            try
            {
                var donUngTuyen = _tinUngTuyenRepository.LayTinUngTuyenTheoId(id);
                if (donUngTuyen == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy đơn ứng tuyển!" });
                }

                var tinTuyenDung = _tinTuyenDungRepository.LayTinTuyenDungTheoId(
                    int.TryParse(donUngTuyen.MaTinTuyenDung, out int maTin) ? maTin : 0);

                return Json(new
                {
                    success = true,
                    data = new
                    {
                        maTinUngTuyen = donUngTuyen.MaTinUngTuyen,
                        hoTen = donUngTuyen.HoTen,
                        email = donUngTuyen.Email,
                        soDienThoai = donUngTuyen.SoDienThoai,
                        viTriUngTuyen = donUngTuyen.ViTriUngTuyen,
                        trangThaiXuLy = donUngTuyen.TrangThaiXuLy,
                        linkCV = donUngTuyen.LinkCV,
                        ngayUngTuyen = donUngTuyen.NgayUngTuyen.ToString("dd/MM/yyyy HH:mm"),
                        ghiChu = donUngTuyen.GhiChu,
                        tenViecLam = tinTuyenDung?.TenViecLam,
                        congTy = tinTuyenDung?.CongTy
                    }
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        // POST: Cập nhật trạng thái đơn ứng tuyển
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CapNhatTrangThaiDonUngTuyen(int maDonUngTuyen, string trangThaiMoi, string? ghiChu = null)
        {
            try
            {
                var donUngTuyen = _tinUngTuyenRepository.LayTinUngTuyenTheoId(maDonUngTuyen);
                if (donUngTuyen == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy đơn ứng tuyển" });
                }

                if (string.IsNullOrEmpty(trangThaiMoi))
                {
                    return Json(new { success = false, message = "Vui lòng chọn trạng thái mới" });
                }

                var donDaCapNhat = _tinUngTuyenRepository.CapNhatTrangThai(maDonUngTuyen, trangThaiMoi, ghiChu);
                
                if (donDaCapNhat == null)
                {
                    return Json(new { success = false, message = "Không thể cập nhật trạng thái" });
                }

                return Json(new { 
                    success = true, 
                    message = "Cập nhật trạng thái thành công!" 
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        // GET: Lấy danh sách tin tuyển dụng đã ứng tuyển của ứng viên
        [HttpGet]
        public IActionResult LayDanhSachTinTuyenDungTheoUngVien(string userId)
        {
            try
            {
                var danhSachUngTuyen = _tinUngTuyenRepository.LayDanhSachTinUngTuyen()
                    .Where(t => t.UserId == userId)
                    .ToList();

                var danhSachTin = new List<object>();
                
                foreach (var ungTuyen in danhSachUngTuyen)
                {
                    var tinTuyenDung = _tinTuyenDungRepository.LayTinTuyenDungTheoId(int.Parse(ungTuyen.MaTinTuyenDung));
                    
                    if (tinTuyenDung != null)
                    {
                        var mucLuong = "";
                        if (tinTuyenDung.MucLuongThapNhat.HasValue && tinTuyenDung.MucLuongCaoNhat.HasValue)
                        {
                            mucLuong = $"{tinTuyenDung.MucLuongThapNhat.Value:F0} - {tinTuyenDung.MucLuongCaoNhat.Value:F0} triệu";
                        }
                        else if (tinTuyenDung.MucLuongThapNhat.HasValue)
                        {
                            mucLuong = $"Từ {tinTuyenDung.MucLuongThapNhat.Value:F0} triệu";
                        }
                        else
                        {
                            mucLuong = "Thỏa thuận";
                        }

                        danhSachTin.Add(new
                        {
                            maTin = tinTuyenDung.MaTinTuyenDung,
                            tenViecLam = tinTuyenDung.TenViecLam ?? "N/A",
                            nganhNghe = tinTuyenDung.NganhNghe ?? "N/A",
                            loaiCongViec = tinTuyenDung.LoaiCongViec ?? "N/A",
                            tinhThanhPho = tinTuyenDung.TinhThanhPho ?? "N/A",
                            mucLuong = mucLuong,
                            congTy = tinTuyenDung.CongTy ?? "N/A",
                            ngayDang = tinTuyenDung.NgayDang.ToString("dd/MM/yyyy"),
                            hanNop = tinTuyenDung.HanNop.ToString("dd/MM/yyyy"),
                            trangThaiXuLy = ungTuyen.TrangThaiXuLy,
                            ngayUngTuyen = ungTuyen.NgayUngTuyen.ToString("dd/MM/yyyy"),
                            viTriUngTuyen = ungTuyen.ViTriUngTuyen
                        });
                    }
                }

                return Json(new { success = true, data = danhSachTin });
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
                    _logger.LogWarning("Attempted to delete non-existent job posting with ID: {Id}", id);
                    return Json(new { success = false, message = "Không tìm thấy tin tuyển dụng!" });
                }

                _logger.LogInformation("Admin attempting to delete job posting ID: {Id}, Title: {Title}", id, tin.TenViecLam);
                
                var ketQua = _tinTuyenDungRepository.XoaTinTuyenDung(id);
                if (ketQua)
                {
                    _logger.LogInformation("Successfully deleted job posting ID: {Id}", id);
                    return Json(new { success = true, message = "Đã xóa tin tuyển dụng thành công!" });
                }
                else
                {
                    _logger.LogWarning("Failed to delete job posting ID: {Id} - repository returned false", id);
                    return Json(new { success = false, message = "Không thể xóa tin tuyển dụng!" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting job posting ID: {Id}", id);
                return Json(new { success = false, message = "Có lỗi xảy ra khi xóa tin tuyển dụng: " + ex.Message });
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
            
            // Nếu có lỗi validation, cập nhật các trường đã thay đổi vào tin hiện tại bằng AutoMapper
            // AutoMapper sẽ tự động map các thuộc tính và bỏ qua các thuộc tính đặc biệt
            _mapper.Map(model, tinHienTai);

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

        // POST: Cập nhật thông tin nhà tuyển dụng
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CapNhatNhaTuyenDung(
            int id,
            string? tenCongTy,
            string? email,
            string? soDienThoai,
            string? diaChi,
            string? tinhThanhPho,
            string? quanHuyen,
            string? website,
            string? nguoiDaiDien,
            string? chucVu,
            string? soDienThoaiNguoiDaiDien,
            string? emailNguoiDaiDien,
            string? linhVuc,
            string? moTa,
            string? latitude,
            string? longitude)
        {
            try
            {
                var nhaTuyenDung = _nhaTuyenDungRepository.LayNhaTuyenDungTheoId(id);
                if (nhaTuyenDung == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy nhà tuyển dụng!" });
                }

                // Cập nhật thông tin
                if (!string.IsNullOrEmpty(tenCongTy))
                    nhaTuyenDung.TenCongTy = tenCongTy;
                if (!string.IsNullOrEmpty(email))
                    nhaTuyenDung.Email = email;
                if (!string.IsNullOrEmpty(soDienThoai))
                    nhaTuyenDung.SoDienThoai = soDienThoai;
                if (diaChi != null)
                    nhaTuyenDung.DiaChi = diaChi;
                if (tinhThanhPho != null)
                    nhaTuyenDung.TinhThanhPho = tinhThanhPho;
                if (quanHuyen != null)
                    nhaTuyenDung.QuanHuyen = quanHuyen;
                if (website != null)
                    nhaTuyenDung.Website = website;
                if (nguoiDaiDien != null)
                    nhaTuyenDung.NguoiDaiDien = nguoiDaiDien;
                if (chucVu != null)
                    nhaTuyenDung.ChucVu = chucVu;
                if (soDienThoaiNguoiDaiDien != null)
                    nhaTuyenDung.SoDienThoaiNguoiDaiDien = soDienThoaiNguoiDaiDien;
                if (emailNguoiDaiDien != null)
                    nhaTuyenDung.EmailNguoiDaiDien = emailNguoiDaiDien;
                if (linhVuc != null)
                    nhaTuyenDung.LinhVuc = linhVuc;
                if (moTa != null)
                    nhaTuyenDung.MoTa = moTa;
                
                if (!string.IsNullOrEmpty(latitude) && double.TryParse(latitude, out double latValue))
                    nhaTuyenDung.Latitude = latValue;
                else
                    nhaTuyenDung.Latitude = null;
                
                if (!string.IsNullOrEmpty(longitude) && double.TryParse(longitude, out double lngValue))
                    nhaTuyenDung.Longitude = lngValue;
                else
                    nhaTuyenDung.Longitude = null;

                var ketQua = _nhaTuyenDungRepository.CapNhatNhaTuyenDung(nhaTuyenDung);
                if (ketQua != null)
                {
                    TempData["ThanhCong"] = "Cập nhật thông tin nhà tuyển dụng thành công!";
                    return RedirectToAction("ChiTietNhaTuyenDung", new { id = id });
                }
                else
                {
                    TempData["Loi"] = "Có lỗi xảy ra khi cập nhật thông tin!";
                    return RedirectToAction("ChiTietNhaTuyenDung", new { id = id });
                }
            }
            catch (Exception ex)
            {
                TempData["Loi"] = "Có lỗi xảy ra: " + ex.Message;
                return RedirectToAction("ChiTietNhaTuyenDung", new { id = id });
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

        // ========== BLOG MANAGEMENT ==========
        public IActionResult Blog(string? search = null, int pageNumber = 1, int pageSize = 20)
        {
            try
            {
                var danhSachBlog = _blogRepository.LayDanhSachBlog();
                var danhSachBaiVietChoDuyet = _blogRepository.LayDanhSachBlogChoDuyet();
                
                _logger.LogInformation($"Fetched {danhSachBlog?.Count ?? 0} blogs from repository");
                
                if (danhSachBlog == null || !danhSachBlog.Any())
                {
                    _logger.LogWarning("No blogs found in database");
                    ViewBag.Search = search;
                    ViewBag.PageNumber = pageNumber;
                    ViewBag.PageSize = pageSize;
                    ViewBag.TotalPages = 0;
                    ViewBag.TotalItems = 0;
                    ViewBag.DanhSachBaiVietChoDuyet = new List<Blog>();
                    return View(new List<Blog>());
                }
                
                if (!string.IsNullOrEmpty(search))
                {
                    var searchLower = search.ToLower();
                    danhSachBlog = danhSachBlog.Where(b =>
                        (b.TieuDe != null && b.TieuDe.ToLower().Contains(searchLower)) ||
                        (b.MoTaNgan != null && b.MoTaNgan.ToLower().Contains(searchLower)) ||
                        (b.TheLoai != null && b.TheLoai.ToLower().Contains(searchLower)) ||
                        (b.TacGia != null && b.TacGia.ToLower().Contains(searchLower)) ||
                        (b.NguonBaiViet != null && b.NguonBaiViet.ToLower().Contains(searchLower))
                    ).ToList();
                }
                
                var totalItems = danhSachBlog.Count;
                var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
                var pagedBlogs = danhSachBlog
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
                
                ViewBag.Search = search;
                ViewBag.PageNumber = pageNumber;
                ViewBag.PageSize = pageSize;
                ViewBag.TotalPages = totalPages;
                ViewBag.TotalItems = totalItems;
                ViewBag.DanhSachBaiVietChoDuyet = danhSachBaiVietChoDuyet ?? new List<Blog>();
                
                return View(pagedBlogs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching blogs: {Message}", ex.Message);
                TempData["Loi"] = $"Lỗi khi tải danh sách blog: {ex.Message}. Vui lòng kiểm tra log để biết chi tiết.";
                ViewBag.Search = search;
                ViewBag.PageNumber = pageNumber;
                ViewBag.PageSize = pageSize;
                ViewBag.TotalPages = 0;
                ViewBag.TotalItems = 0;
                ViewBag.DanhSachBaiVietChoDuyet = new List<Blog>();
                return View(new List<Blog>());
            }
        }

        [HttpGet]
        public IActionResult ThemBlog()
        {
            // Redirect đến SuaBlog với id = 0 để tạo mới
            return RedirectToAction("SuaBlog", new { id = 0 });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ThemBlog(Blog blog, IFormFile? hinhAnhFile)
        {
            // Đọc Tags trực tiếp từ form nếu model binding không hoạt động
            // Thử nhiều cách để lấy Tags
            string? tagsValue = null;
            
            if (Request.Form.ContainsKey("Tags"))
            {
                tagsValue = Request.Form["Tags"].ToString();
                System.Diagnostics.Debug.WriteLine($"Tags từ Request.Form['Tags']: '{tagsValue}'");
            }
            else if (Request.Form.ContainsKey("tagsHidden"))
            {
                tagsValue = Request.Form["tagsHidden"].ToString();
                System.Diagnostics.Debug.WriteLine($"Tags từ Request.Form['tagsHidden']: '{tagsValue}'");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Request.Form không chứa key 'Tags' hoặc 'tagsHidden'");
                // Kiểm tra tất cả keys trong form
                System.Diagnostics.Debug.WriteLine("Tất cả keys trong Request.Form:");
                foreach (var key in Request.Form.Keys)
                {
                    System.Diagnostics.Debug.WriteLine($"  - {key} = '{Request.Form[key]}'");
                }
            }
            
            if (!string.IsNullOrWhiteSpace(tagsValue))
            {
                blog.Tags = tagsValue;
                System.Diagnostics.Debug.WriteLine($"Tags đã được set vào blog.Tags: '{blog.Tags}'");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Tags rỗng hoặc null, không set vào blog.Tags");
            }
            
            // Đọc TacGia trực tiếp từ form nếu model binding không hoạt động
            if (Request.Form.ContainsKey("TacGia"))
            {
                var tacGiaValue = Request.Form["TacGia"].ToString();
                blog.TacGia = tacGiaValue;
                System.Diagnostics.Debug.WriteLine($"TacGia từ Request.Form: '{tacGiaValue}'");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Request.Form không chứa key 'TacGia'");
            }
            
            if (!ModelState.IsValid)
            {
                var user = _userManager.GetUserAsync(User).Result;
                ViewBag.UserId = user?.Id;
                ViewBag.TacGia = user?.UserName ?? "Admin";
                ViewBag.DanhSachTheLoai = _theLoaiBlogRepository.LayDanhSachTheLoaiHienThi();
                return View(blog);
            }

            // Tự động cập nhật TheLoai từ TheLoaiBlog nếu có
            if (blog.MaTheLoai.HasValue)
            {
                var theLoai = _theLoaiBlogRepository.LayTheLoaiTheoId(blog.MaTheLoai.Value);
                if (theLoai != null)
                {
                    blog.TheLoai = theLoai.TenTheLoai;
                }
            }

            // Upload hình ảnh nếu có
            if (hinhAnhFile != null && hinhAnhFile.Length > 0)
            {
                var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "blogs");
                if (!Directory.Exists(uploadsPath))
                {
                    Directory.CreateDirectory(uploadsPath);
                }

                var fileName = $"blog_{DateTime.Now:yyyyMMddHHmmss}_{Guid.NewGuid():N}{Path.GetExtension(hinhAnhFile.FileName)}";
                var filePath = Path.Combine(uploadsPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await hinhAnhFile.CopyToAsync(stream);
                }

                blog.HinhAnh = $"/uploads/blogs/{fileName}";
            }

            // Lấy UserId từ user hiện tại
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser != null)
            {
                blog.UserId = currentUser.Id;
                // Tự động set TacGia từ User nếu chưa có
                if (string.IsNullOrWhiteSpace(blog.TacGia))
                {
                    blog.TacGia = currentUser.UserName ?? currentUser.Email ?? "Admin";
                }
            }

            // Mặc định: hiển thị (admin tự đăng)
            blog.HienThi = true;

            // Xử lý IsPermalinkAuto từ form (ưu tiên hidden input, sau đó mới đến radio button)
            if (Request.Form.ContainsKey("IsPermalinkAuto"))
            {
                var isPermalinkAutoValue = Request.Form["IsPermalinkAuto"].ToString();
                blog.IsPermalinkAuto = isPermalinkAutoValue == "true" || isPermalinkAutoValue == "True";
            }
            else if (Request.Form.ContainsKey("permalinkType"))
            {
                blog.IsPermalinkAuto = Request.Form["permalinkType"].ToString() == "auto";
            }
            
            // Xử lý Permalink từ form
            if (Request.Form.ContainsKey("Permalink"))
            {
                var permalinkValue = Request.Form["Permalink"].ToString();
                if (!string.IsNullOrWhiteSpace(permalinkValue))
                {
                    blog.Permalink = permalinkValue.Trim();
                    System.Diagnostics.Debug.WriteLine($"Permalink từ form (ThemBlog): '{blog.Permalink}'");
                }
            }
            
            // Tự động tạo permalink nếu IsPermalinkAuto = true và chưa có permalink
            if (blog.IsPermalinkAuto && string.IsNullOrWhiteSpace(blog.Permalink))
            {
                blog.Permalink = GeneratePermalinkFromTitle(blog.TieuDe ?? string.Empty, blog.MaBlog > 0 ? blog.MaBlog : null);
                System.Diagnostics.Debug.WriteLine($"Permalink tự động tạo (ThemBlog): '{blog.Permalink}'");
            }
            
            // ✅ QUAN TRỌNG: Đảm bảo permalink luôn duy nhất TRƯỚC KHI lưu
            // Nếu permalink là "blog-post" (mặc định) hoặc trùng với blog khác, tạo lại
            if (!string.IsNullOrWhiteSpace(blog.Permalink))
            {
                var permalinkTrimmed = blog.Permalink.Trim();
                System.Diagnostics.Debug.WriteLine($"Kiểm tra permalink (ThemBlog): '{permalinkTrimmed}', MaBlog: {blog.MaBlog}");
                
                // Nếu permalink là "blog-post" (mặc định) và đang tạo blog mới, tạo permalink duy nhất ngay
                if (permalinkTrimmed == "blog-post" && blog.MaBlog == 0)
                {
                    var oldPermalink = blog.Permalink;
                    blog.Permalink = GenerateUniquePermalink("blog-post", null);
                    System.Diagnostics.Debug.WriteLine($"⚠️ Permalink mặc định '{oldPermalink}', đã tạo lại: '{blog.Permalink}'");
                }
                // Kiểm tra xem permalink có trùng với blog khác không (trừ blog hiện tại nếu đang sửa)
                else
                {
                    var existingBlog = _blogRepository.LayBlogTheoPermalink(permalinkTrimmed);
                    if (existingBlog != null && existingBlog.MaBlog != blog.MaBlog)
                    {
                        // Permalink trùng, tạo lại với số đằng sau
                        var oldPermalink = blog.Permalink;
                        blog.Permalink = GenerateUniquePermalink(permalinkTrimmed, blog.MaBlog > 0 ? blog.MaBlog : null);
                        System.Diagnostics.Debug.WriteLine($"⚠️ Permalink '{oldPermalink}' trùng với blog #{existingBlog.MaBlog}, đã tạo lại: '{blog.Permalink}'");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"✅ Permalink '{permalinkTrimmed}' hợp lệ và duy nhất");
                    }
                }
            }
            // Nếu IsPermalinkAuto = false, Permalink đã được lấy từ form ở trên

            // Đảm bảo Tags và TacGia được lưu
            // Tags đã được gửi từ form qua name="Tags"
            // TacGia đã được gửi từ form qua asp-for="TacGia"
            
            // Debug: Log giá trị trước khi lưu
            System.Diagnostics.Debug.WriteLine($"Tags trước khi lưu: {blog.Tags}");
            System.Diagnostics.Debug.WriteLine($"TacGia trước khi lưu: {blog.TacGia}");

            Blog? result;
            
            // Kiểm tra xem blog đã tồn tại chưa (nếu có MaBlog > 0 thì là đang cập nhật từ bản nháp)
            if (blog.MaBlog > 0)
            {
                var blogHienTai = _blogRepository.LayBlogTheoId(blog.MaBlog);
                if (blogHienTai != null)
                {
                    // Cập nhật blog hiện có bằng AutoMapper
                    // AutoMapper sẽ tự động map các thuộc tính và set NgayCapNhat = DateTime.Now
                    _mapper.Map(blog, blogHienTai);
                    
                    // Set các thuộc tính đặc biệt sau khi mapping
                    blogHienTai.HienThi = true;
                    
                    result = _blogRepository.CapNhatBlog(blogHienTai);
                }
                else
                {
                    // Blog không tồn tại, tạo mới
                    result = _blogRepository.ThemBlog(blog);
                }
            }
            else
            {
                // Tạo blog mới
                result = _blogRepository.ThemBlog(blog);
                
                // ✅ Sau khi blog đã được lưu và có MaBlog, kiểm tra lại permalink
                // Nếu permalink là "blog-post" hoặc trùng với blog khác, tạo lại permalink duy nhất
                if (result != null && !string.IsNullOrWhiteSpace(result.Permalink))
                {
                    var existingBlog = _blogRepository.LayBlogTheoPermalink(result.Permalink);
                    // Nếu permalink trùng với blog khác (không phải blog hiện tại)
                    if (existingBlog != null && existingBlog.MaBlog != result.MaBlog)
                    {
                        // Tạo lại permalink duy nhất với MaBlog
                        result.Permalink = GeneratePermalinkFromTitle(result.TieuDe ?? string.Empty, result.MaBlog);
                        _blogRepository.CapNhatBlog(result);
                    }
                    // Nếu permalink là "blog-post" (mặc định), cũng tạo lại để có permalink tốt hơn
                    else if (result.Permalink == "blog-post" && !string.IsNullOrWhiteSpace(result.TieuDe))
                    {
                        result.Permalink = GeneratePermalinkFromTitle(result.TieuDe, result.MaBlog);
                        _blogRepository.CapNhatBlog(result);
                    }
                }
            }
            
            // Debug: Log giá trị sau khi lưu
            if (result != null)
            {
                System.Diagnostics.Debug.WriteLine($"Tags sau khi lưu: {result.Tags}");
                System.Diagnostics.Debug.WriteLine($"TacGia sau khi lưu: {result.TacGia}");
                System.Diagnostics.Debug.WriteLine($"Permalink sau khi lưu: {result.Permalink}");
            }
            if (result != null)
            {
                TempData["ThanhCong"] = "Đã đăng blog thành công!";
                return RedirectToAction("Blog");
            }
            else
            {
                TempData["Loi"] = "Có lỗi xảy ra khi đăng blog!";
                var user = _userManager.GetUserAsync(User).Result;
                ViewBag.UserId = user?.Id;
                ViewBag.TacGia = user?.UserName ?? "Admin";
                ViewBag.DanhSachTheLoai = _theLoaiBlogRepository.LayDanhSachTheLoaiHienThi();
                return View(blog);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LuuTamBlog(Blog blog, IFormFile? hinhAnhFile)
        {
            try
            {
                // Đọc Tags trực tiếp từ form nếu model binding không hoạt động
                if (Request.Form.ContainsKey("Tags"))
                {
                    blog.Tags = Request.Form["Tags"].ToString();
                }

            // Lấy UserId từ user hiện tại
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser != null)
            {
                blog.UserId = currentUser.Id;
                if (string.IsNullOrWhiteSpace(blog.TacGia))
                {
                    blog.TacGia = currentUser.UserName ?? currentUser.Email ?? "Admin";
                }
            }

            // Upload hình ảnh nếu có
            if (hinhAnhFile != null && hinhAnhFile.Length > 0)
            {
                var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "blogs");
                if (!Directory.Exists(uploadsPath))
                {
                    Directory.CreateDirectory(uploadsPath);
                }

                var fileName = $"blog_{DateTime.Now:yyyyMMddHHmmss}_{Guid.NewGuid():N}{Path.GetExtension(hinhAnhFile.FileName)}";
                var filePath = Path.Combine(uploadsPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await hinhAnhFile.CopyToAsync(stream);
                }

                blog.HinhAnh = $"/uploads/blogs/{fileName}";
            }

            // Tự động cập nhật TheLoai từ TheLoaiBlog nếu có
            if (blog.MaTheLoai.HasValue)
            {
                var theLoai = _theLoaiBlogRepository.LayTheLoaiTheoId(blog.MaTheLoai.Value);
                if (theLoai != null)
                {
                    blog.TheLoai = theLoai.TenTheLoai;
                }
            }

            // Xử lý IsPermalinkAuto từ form
            if (Request.Form.ContainsKey("IsPermalinkAuto"))
            {
                var isPermalinkAutoValue = Request.Form["IsPermalinkAuto"].ToString();
                blog.IsPermalinkAuto = isPermalinkAutoValue == "true" || isPermalinkAutoValue == "True";
            }
            else if (Request.Form.ContainsKey("permalinkType"))
            {
                blog.IsPermalinkAuto = Request.Form["permalinkType"].ToString() == "auto";
            }

            // Xử lý Permalink từ form
            if (Request.Form.ContainsKey("Permalink"))
            {
                var permalinkValue = Request.Form["Permalink"].ToString();
                if (!string.IsNullOrWhiteSpace(permalinkValue))
                {
                    blog.Permalink = permalinkValue.Trim();
                    System.Diagnostics.Debug.WriteLine($"Permalink từ form (LuuTamBlog): '{blog.Permalink}'");
                }
            }
            
            // Tự động tạo permalink nếu IsPermalinkAuto = true và chưa có permalink
            if (blog.IsPermalinkAuto && string.IsNullOrWhiteSpace(blog.Permalink))
            {
                blog.Permalink = GeneratePermalinkFromTitle(blog.TieuDe ?? string.Empty, blog.MaBlog > 0 ? blog.MaBlog : null);
                System.Diagnostics.Debug.WriteLine($"Permalink tự động tạo (LuuTamBlog): '{blog.Permalink}'");
            }
            
            // ✅ QUAN TRỌNG: Đảm bảo permalink luôn duy nhất TRƯỚC KHI lưu
            // Nếu permalink là "blog-post" (mặc định) hoặc trùng với blog khác, tạo lại
            if (!string.IsNullOrWhiteSpace(blog.Permalink))
            {
                var permalinkTrimmed = blog.Permalink.Trim();
                System.Diagnostics.Debug.WriteLine($"Kiểm tra permalink (LuuTamBlog): '{permalinkTrimmed}', MaBlog: {blog.MaBlog}");
                
                // Nếu permalink là "blog-post" (mặc định) và đang tạo blog mới, tạo permalink duy nhất ngay
                if (permalinkTrimmed == "blog-post" && blog.MaBlog == 0)
                {
                    var oldPermalink = blog.Permalink;
                    blog.Permalink = GenerateUniquePermalink("blog-post", null);
                    System.Diagnostics.Debug.WriteLine($"⚠️ Permalink mặc định '{oldPermalink}', đã tạo lại: '{blog.Permalink}'");
                }
                // Kiểm tra xem permalink có trùng với blog khác không (trừ blog hiện tại nếu đang sửa)
                else
                {
                    var existingBlog = _blogRepository.LayBlogTheoPermalink(permalinkTrimmed);
                    if (existingBlog != null && existingBlog.MaBlog != blog.MaBlog)
                    {
                        // Permalink trùng, tạo lại với số đằng sau
                        var oldPermalink = blog.Permalink;
                        blog.Permalink = GenerateUniquePermalink(permalinkTrimmed, blog.MaBlog > 0 ? blog.MaBlog : null);
                        System.Diagnostics.Debug.WriteLine($"⚠️ Permalink '{oldPermalink}' trùng với blog #{existingBlog.MaBlog}, đã tạo lại: '{blog.Permalink}'");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"✅ Permalink '{permalinkTrimmed}' hợp lệ và duy nhất");
                    }
                }
            }
            // Nếu IsPermalinkAuto = false, Permalink đã được lấy từ form ở trên

            // Lưu bản nháp: HienThi = false
            blog.HienThi = false;

            // Kiểm tra xem blog đã tồn tại chưa (nếu có MaBlog)
            Blog? result;
            if (blog.MaBlog > 0)
            {
                // Cập nhật blog hiện có
                var blogHienTai = _blogRepository.LayBlogTheoId(blog.MaBlog);
                if (blogHienTai != null)
                {
                    // ✅ Cập nhật tất cả các trường từ blog mới vào blog hiện có bằng AutoMapper
                    // AutoMapper sẽ tự động map các thuộc tính và set NgayCapNhat = DateTime.Now
                    _mapper.Map(blog, blogHienTai);
                    
                    // Set các thuộc tính đặc biệt sau khi mapping
                    blogHienTai.HienThi = false;
                    // ✅ Giữ nguyên DaDang (không thay đổi trạng thái đăng khi lưu bản nháp)
                    // blogHienTai.DaDang = false; // Không set lại, giữ nguyên giá trị hiện tại

                    result = _blogRepository.CapNhatBlog(blogHienTai);
                }
                else
                {
                    // Blog không tồn tại với MaBlog này, tạo mới
                    blog.MaBlog = 0; // Reset để tạo mới
                    result = _blogRepository.ThemBlog(blog);
                }
            }
            else
            {
                // Thêm blog mới
                // ✅ Đảm bảo DaDang = false và HienThi = false cho bản nháp
                blog.DaDang = false;
                blog.HienThi = false;
                result = _blogRepository.ThemBlog(blog);
                
                // ✅ Sau khi blog đã được lưu và có MaBlog, kiểm tra lại permalink
                // Nếu permalink là "blog-post" hoặc trùng với blog khác, tạo lại permalink duy nhất
                if (result != null && !string.IsNullOrWhiteSpace(result.Permalink))
                {
                    var existingBlog = _blogRepository.LayBlogTheoPermalink(result.Permalink);
                    // Nếu permalink trùng với blog khác (không phải blog hiện tại)
                    if (existingBlog != null && existingBlog.MaBlog != result.MaBlog)
                    {
                        // Tạo lại permalink duy nhất với MaBlog
                        result.Permalink = GeneratePermalinkFromTitle(result.TieuDe ?? string.Empty, result.MaBlog);
                        _blogRepository.CapNhatBlog(result);
                    }
                    // Nếu permalink là "blog-post" (mặc định), cũng tạo lại để có permalink tốt hơn
                    else if (result.Permalink == "blog-post" && !string.IsNullOrWhiteSpace(result.TieuDe))
                    {
                        result.Permalink = GeneratePermalinkFromTitle(result.TieuDe, result.MaBlog);
                        _blogRepository.CapNhatBlog(result);
                    }
                }
            }

                if (result != null)
                {
                    // Repository đã gọi SaveChanges() rồi, không cần gọi lại
                    return Json(new { success = true, message = "Đã lưu bản nháp thành công!", maBlog = result.MaBlog });
                }
                else
                {
                    return Json(new { success = false, message = "Có lỗi xảy ra khi lưu bản nháp!" });
                }
            }
            catch (Exception ex)
            {
                // Log lỗi để debug
                System.Diagnostics.Debug.WriteLine($"Lỗi khi lưu bản nháp: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                return Json(new { success = false, message = $"Có lỗi xảy ra: {ex.Message}" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> QuayVeBanNhap(Blog blog, IFormFile? hinhAnhFile)
        {
            try
            {
                if (blog.MaBlog <= 0)
                {
                    return Json(new { success = false, message = "Không tìm thấy blog!" });
                }

                // Đọc Tags trực tiếp từ form nếu model binding không hoạt động
                if (Request.Form.ContainsKey("Tags"))
                {
                    blog.Tags = Request.Form["Tags"].ToString();
                }

                // Lấy blog hiện tại từ database
                var blogHienTai = _blogRepository.LayBlogTheoId(blog.MaBlog);
                if (blogHienTai == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy blog!" });
                }

                // Upload hình ảnh mới nếu có
                if (hinhAnhFile != null && hinhAnhFile.Length > 0)
                {
                    // Xóa hình ảnh cũ nếu có
                    if (!string.IsNullOrEmpty(blogHienTai.HinhAnh))
                    {
                        var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", blogHienTai.HinhAnh.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "blogs");
                    if (!Directory.Exists(uploadsPath))
                    {
                        Directory.CreateDirectory(uploadsPath);
                    }

                    var fileName = $"blog_{DateTime.Now:yyyyMMddHHmmss}_{Guid.NewGuid():N}{Path.GetExtension(hinhAnhFile.FileName)}";
                    var filePath = Path.Combine(uploadsPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await hinhAnhFile.CopyToAsync(stream);
                    }

                    blogHienTai.HinhAnh = $"/uploads/blogs/{fileName}";
                }

                // Tự động cập nhật TheLoai từ TheLoaiBlog nếu có
                if (blog.MaTheLoai.HasValue)
                {
                    var theLoai = _theLoaiBlogRepository.LayTheLoaiTheoId(blog.MaTheLoai.Value);
                    if (theLoai != null)
                    {
                        blog.TheLoai = theLoai.TenTheLoai;
                    }
                }

                // Xử lý IsPermalinkAuto từ form
                if (Request.Form.ContainsKey("IsPermalinkAuto"))
                {
                    var isPermalinkAutoValue = Request.Form["IsPermalinkAuto"].ToString();
                    blog.IsPermalinkAuto = isPermalinkAutoValue == "true" || isPermalinkAutoValue == "True";
                }
                else if (Request.Form.ContainsKey("permalinkType"))
                {
                    blog.IsPermalinkAuto = Request.Form["permalinkType"].ToString() == "auto";
                }

                // Tự động tạo permalink nếu IsPermalinkAuto = true và chưa có permalink
                if (blog.IsPermalinkAuto && string.IsNullOrWhiteSpace(blog.Permalink))
                {
                    blog.Permalink = GeneratePermalinkFromTitle(blog.TieuDe ?? string.Empty, blogHienTai.MaBlog);
                }

                // Cập nhật thông tin blog bằng AutoMapper
                // AutoMapper sẽ tự động map các thuộc tính và set NgayCapNhat = DateTime.Now
                _mapper.Map(blog, blogHienTai);
                
                // Chuyển về bản nháp: DaDang = false (và tự động reset trạng thái hiển thị)
                blogHienTai.DaDang = false;
                blogHienTai.HienThi = false;

                var result = _blogRepository.CapNhatBlog(blogHienTai);

                if (result != null)
                {
                    return Json(new { success = true, message = "Đã chuyển về bản nháp thành công!" });
                }
                else
                {
                    return Json(new { success = false, message = "Có lỗi xảy ra khi chuyển về bản nháp!" });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi khi chuyển về bản nháp: {ex.Message}");
                return Json(new { success = false, message = $"Có lỗi xảy ra: {ex.Message}" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult XemTruocBlog(Blog blog, IFormFile? hinhAnhFile)
        {
            // Xử lý hình ảnh tạm thời nếu có
            if (hinhAnhFile != null && hinhAnhFile.Length > 0)
            {
                var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "blogs", "temp");
                if (!Directory.Exists(uploadsPath))
                {
                    Directory.CreateDirectory(uploadsPath);
                }

                var fileName = $"temp_{DateTime.Now:yyyyMMddHHmmss}_{Guid.NewGuid():N}{Path.GetExtension(hinhAnhFile.FileName)}";
                var filePath = Path.Combine(uploadsPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    hinhAnhFile.CopyTo(stream);
                }

                blog.HinhAnh = $"/uploads/blogs/temp/{fileName}";
            }

            // Lấy thông tin thể loại nếu có
            if (blog.MaTheLoai.HasValue)
            {
                var theLoai = _theLoaiBlogRepository.LayTheLoaiTheoId(blog.MaTheLoai.Value);
                if (theLoai != null)
                {
                    blog.TheLoai = theLoai.TenTheLoai;
                    blog.TheLoaiBlog = theLoai;
                }
            }

            // Set thông tin tác giả
            var currentUser = _userManager.GetUserAsync(User).Result;
            if (currentUser != null && string.IsNullOrWhiteSpace(blog.TacGia))
            {
                blog.TacGia = currentUser.UserName ?? currentUser.Email ?? "Admin";
            }

            // Set ngày đăng
            if (blog.NgayDang == default)
            {
                blog.NgayDang = DateTime.Now;
            }

            ViewBag.IsPreview = true;
            return View(blog);
        }

        [HttpGet]
        public IActionResult SuaBlog(int? id)
        {
            Blog? blog;
            
            // Nếu id = null hoặc 0, tạo blog mới
            if (id == null || id == 0)
            {
                var user = _userManager.GetUserAsync(User).Result;
                blog = new Blog
                {
                    NgayDang = DateTime.Now,
                    DaDang = false,   // Mặc định là bản nháp (chưa đăng)
                    HienThi = false,  // Mặc định không hiển thị
                    IsPermalinkAuto = true // Mặc định là tự động
                };
                ViewBag.UserId = user?.Id;
                ViewBag.TacGia = user?.HoTen ?? user?.UserName ?? "Admin";
            }
            else
            {
                // Sửa blog đã có
                blog = _blogRepository.LayBlogTheoId(id.Value);
                if (blog == null)
                {
                    TempData["Loi"] = "Không tìm thấy blog!";
                    return RedirectToAction("Blog");
                }
                
                // Debug: Log giá trị Tags khi load
                System.Diagnostics.Debug.WriteLine($"Tags khi load blog để sửa: {blog.Tags}");
                System.Diagnostics.Debug.WriteLine($"TacGia khi load blog để sửa: {blog.TacGia}");
            }
            
            ViewBag.DanhSachTheLoai = _theLoaiBlogRepository.LayDanhSachTheLoaiHienThi();
            return View(blog);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SuaBlog(Blog blog, IFormFile? hinhAnhFile)
        {
            // Kiểm tra xem là tạo mới hay cập nhật (định nghĩa sớm để tránh lỗi scope)
            bool isCreateNew = blog.MaBlog == 0;
            Blog? blogHienTai = null;
            
            // Đọc Tags trực tiếp từ form nếu model binding không hoạt động
            if (Request.Form.ContainsKey("Tags"))
            {
                blog.Tags = Request.Form["Tags"].ToString();
            }
            
            // Đọc TacGia trực tiếp từ form nếu model binding không hoạt động
            if (Request.Form.ContainsKey("TacGia") && !string.IsNullOrWhiteSpace(Request.Form["TacGia"].ToString()))
            {
                blog.TacGia = Request.Form["TacGia"].ToString();
            }
            
            if (!ModelState.IsValid)
            {
                ViewBag.DanhSachTheLoai = _theLoaiBlogRepository.LayDanhSachTheLoaiHienThi();
                return View(blog);
            }

            // Tự động cập nhật TheLoai từ TheLoaiBlog nếu có
            if (blog.MaTheLoai.HasValue)
            {
                var theLoai = _theLoaiBlogRepository.LayTheLoaiTheoId(blog.MaTheLoai.Value);
                if (theLoai != null)
                {
                    blog.TheLoai = theLoai.TenTheLoai;
                }
            }
            
            if (!isCreateNew)
            {
                // Lấy blog hiện tại từ database để sử dụng sau này
                blogHienTai = _blogRepository.LayBlogTheoId(blog.MaBlog);
                if (blogHienTai == null)
                {
                    TempData["Loi"] = "Không tìm thấy blog!";
                    return RedirectToAction("Blog");
                }
            }

            // Xử lý IsPermalinkAuto từ form (ưu tiên hidden input, sau đó mới đến radio button)
            if (Request.Form.ContainsKey("IsPermalinkAuto"))
            {
                var isPermalinkAutoValue = Request.Form["IsPermalinkAuto"].ToString();
                blog.IsPermalinkAuto = isPermalinkAutoValue == "true" || isPermalinkAutoValue == "True";
                System.Diagnostics.Debug.WriteLine($"IsPermalinkAuto từ hidden input: {isPermalinkAutoValue} -> {blog.IsPermalinkAuto}");
            }
            else if (Request.Form.ContainsKey("permalinkType"))
            {
                blog.IsPermalinkAuto = Request.Form["permalinkType"].ToString() == "auto";
                System.Diagnostics.Debug.WriteLine($"IsPermalinkAuto từ radio button: {Request.Form["permalinkType"]} -> {blog.IsPermalinkAuto}");
            }
            else if (blogHienTai != null)
            {
                // Giữ nguyên giá trị hiện tại từ database (chỉ khi đang sửa)
                blog.IsPermalinkAuto = blogHienTai.IsPermalinkAuto;
                System.Diagnostics.Debug.WriteLine($"IsPermalinkAuto giữ nguyên từ database: {blog.IsPermalinkAuto}");
            }
            
            System.Diagnostics.Debug.WriteLine($"IsPermalinkAuto cuối cùng: {blog.IsPermalinkAuto}");
            
            // Xử lý Permalink từ form
            if (Request.Form.ContainsKey("Permalink"))
            {
                var permalinkValue = Request.Form["Permalink"].ToString();
                if (!string.IsNullOrWhiteSpace(permalinkValue))
                {
                    blog.Permalink = permalinkValue.Trim();
                    System.Diagnostics.Debug.WriteLine($"Permalink từ form: '{blog.Permalink}'");
                }
            }
            
            // Tự động tạo permalink nếu IsPermalinkAuto = true và chưa có permalink
            if (blog.IsPermalinkAuto && string.IsNullOrWhiteSpace(blog.Permalink))
            {
                int? excludeMaBlog = blogHienTai != null ? blogHienTai.MaBlog : (blog.MaBlog > 0 ? blog.MaBlog : null);
                blog.Permalink = GeneratePermalinkFromTitle(blog.TieuDe ?? string.Empty, excludeMaBlog);
                System.Diagnostics.Debug.WriteLine($"Permalink tự động tạo: '{blog.Permalink}'");
            }
            
            // ✅ QUAN TRỌNG: Đảm bảo permalink luôn duy nhất TRƯỚC KHI lưu
            // Nếu permalink là "blog-post" (mặc định) hoặc trùng với blog khác, tạo lại
            if (!string.IsNullOrWhiteSpace(blog.Permalink))
            {
                int? excludeMaBlog = blogHienTai != null ? blogHienTai.MaBlog : (blog.MaBlog > 0 ? blog.MaBlog : null);
                // Kiểm tra xem permalink có trùng với blog khác không (trừ blog hiện tại nếu đang sửa)
                var existingBlog = _blogRepository.LayBlogTheoPermalink(blog.Permalink);
                if (existingBlog != null && existingBlog.MaBlog != excludeMaBlog)
                {
                    // Permalink trùng, tạo lại với số đằng sau
                    blog.Permalink = GenerateUniquePermalink(blog.Permalink, excludeMaBlog);
                    System.Diagnostics.Debug.WriteLine($"Permalink trùng, đã tạo lại: '{blog.Permalink}'");
                }
                // Nếu permalink là "blog-post" và đang tạo blog mới, tạo permalink duy nhất
                else if (blog.Permalink == "blog-post" && excludeMaBlog == null)
                {
                    blog.Permalink = GenerateUniquePermalink("blog-post", null);
                    System.Diagnostics.Debug.WriteLine($"Permalink mặc định, đã tạo lại: '{blog.Permalink}'");
                }
            }
            // Nếu IsPermalinkAuto = false, Permalink đã được lấy từ form ở trên

            // Upload hình ảnh mới nếu có
            if (hinhAnhFile != null && hinhAnhFile.Length > 0)
            {
                // Xóa hình ảnh cũ nếu có (chỉ khi đang sửa)
                if (blogHienTai != null && !string.IsNullOrEmpty(blogHienTai.HinhAnh))
                {
                    var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", blogHienTai.HinhAnh.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "blogs");
                if (!Directory.Exists(uploadsPath))
                {
                    Directory.CreateDirectory(uploadsPath);
                }

                var fileName = $"blog_{DateTime.Now:yyyyMMddHHmmss}_{Guid.NewGuid():N}{Path.GetExtension(hinhAnhFile.FileName)}";
                var filePath = Path.Combine(uploadsPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await hinhAnhFile.CopyToAsync(stream);
                }

                blog.HinhAnh = $"/uploads/blogs/{fileName}";
            }
            else if (blogHienTai != null)
            {
                // Giữ nguyên hình ảnh cũ (chỉ khi đang sửa)
                blog.HinhAnh = blogHienTai.HinhAnh;
            }

            // Lấy UserId từ user hiện tại
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser != null)
            {
                if (isCreateNew)
                {
                    blog.UserId = currentUser.Id;
                    // Tự động set TacGia từ User nếu chưa có
                    if (string.IsNullOrWhiteSpace(blog.TacGia))
                    {
                        blog.TacGia = currentUser.HoTen ?? currentUser.UserName ?? currentUser.Email ?? "Admin";
                    }
                }
                else
                {
                    // Giữ nguyên UserId và các trường không được cập nhật
                    blog.UserId = blogHienTai.UserId;
                    blog.NgayDang = blogHienTai.NgayDang;
                    blog.LuotXem = blogHienTai.LuotXem;
                    
                    // Giữ nguyên trạng thái DaDang từ database (sẽ được xử lý sau)
                    blog.DaDang = blogHienTai.DaDang;
                    
                    // Đảm bảo TacGia được giữ nguyên nếu không có trong form
                    if (string.IsNullOrWhiteSpace(blog.TacGia))
                    {
                        blog.TacGia = blogHienTai.TacGia;
                    }
                }
            }

            // ✅ Kiểm tra xem người dùng nhấn nút "Đăng" hay "Cập nhật"
            // Nếu nhấn nút "Đăng" (có action=dang trong form) -> set DaDang = true, HienThi = true
            var actionValue = Request.Form["action"].ToString();
            System.Diagnostics.Debug.WriteLine($"========== DEBUG ACTION ==========");
            System.Diagnostics.Debug.WriteLine($"actionValue từ form: '{actionValue}'");
            System.Diagnostics.Debug.WriteLine($"Trạng thái TRƯỚC khi xử lý action:");
            System.Diagnostics.Debug.WriteLine($"  - DaDang: {blog.DaDang}");
            System.Diagnostics.Debug.WriteLine($"  - HienThi: {blog.HienThi}");
            
            if (actionValue == "dang")
            {
                blog.DaDang = true;
                blog.HienThi = true;
                System.Diagnostics.Debug.WriteLine("✅ Người dùng nhấn nút ĐĂNG -> set DaDang = true, HienThi = true");
            }
            else
            {
                // Nếu không phải nhấn "Đăng" mà là "Cập nhật" hoặc tạo mới thông thường
                // Giữ nguyên các giá trị từ form hoặc database (đã xử lý ở trên)
                System.Diagnostics.Debug.WriteLine($"⚠️ Không phải nút ĐĂNG (action = '{actionValue}')");
            }
            
            System.Diagnostics.Debug.WriteLine($"Trạng thái SAU khi xử lý action:");
            System.Diagnostics.Debug.WriteLine($"  - DaDang: {blog.DaDang}");
            System.Diagnostics.Debug.WriteLine($"  - HienThi: {blog.HienThi}");
            System.Diagnostics.Debug.WriteLine($"==================================");
            
            // Debug: Log giá trị trước khi lưu
            System.Diagnostics.Debug.WriteLine($"Tags trước khi lưu: {blog.Tags}");
            System.Diagnostics.Debug.WriteLine($"TacGia trước khi lưu: {blog.TacGia}");

            Blog? result;
            if (isCreateNew)
            {
                // Tạo blog mới
                result = _blogRepository.ThemBlog(blog);
                
                // ✅ Sau khi blog đã được lưu và có MaBlog, kiểm tra lại permalink
                // Nếu permalink là "blog-post" hoặc trùng với blog khác, tạo lại permalink duy nhất
                if (result != null && !string.IsNullOrWhiteSpace(result.Permalink))
                {
                    var existingBlog = _blogRepository.LayBlogTheoPermalink(result.Permalink);
                    // Nếu permalink trùng với blog khác (không phải blog hiện tại)
                    if (existingBlog != null && existingBlog.MaBlog != result.MaBlog)
                    {
                        // Tạo lại permalink duy nhất với MaBlog
                        result.Permalink = GeneratePermalinkFromTitle(result.TieuDe ?? string.Empty, result.MaBlog);
                        _blogRepository.CapNhatBlog(result);
                    }
                    // Nếu permalink là "blog-post" (mặc định), cũng tạo lại để có permalink tốt hơn
                    else if (result.Permalink == "blog-post" && !string.IsNullOrWhiteSpace(result.TieuDe))
                    {
                        result.Permalink = GeneratePermalinkFromTitle(result.TieuDe, result.MaBlog);
                        _blogRepository.CapNhatBlog(result);
                    }
                }
                
                // Debug: Log giá trị sau khi tạo
                if (result != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Tags sau khi tạo: {result.Tags}");
                    System.Diagnostics.Debug.WriteLine($"TacGia sau khi tạo: {result.TacGia}");
                    System.Diagnostics.Debug.WriteLine($"Permalink sau khi tạo: {result.Permalink}");
                }
                
                if (result != null)
                {
                    TempData["ThanhCong"] = "Đã lưu blog thành công!";
                    return RedirectToAction("Blog");
                }
                else
                {
                    TempData["Loi"] = "Có lỗi xảy ra khi lưu blog!";
                    var user = _userManager.GetUserAsync(User).Result;
                    ViewBag.UserId = user?.Id;
                    ViewBag.TacGia = user?.HoTen ?? user?.UserName ?? "Admin";
                    ViewBag.DanhSachTheLoai = _theLoaiBlogRepository.LayDanhSachTheLoaiHienThi();
                    return View(blog);
                }
            }
            else
            {
                // Cập nhật blog hiện có
                result = _blogRepository.CapNhatBlog(blog);
                
                // Debug: Log giá trị sau khi cập nhật
                if (result != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Tags sau khi cập nhật: {result.Tags}");
                    System.Diagnostics.Debug.WriteLine($"TacGia sau khi cập nhật: {result.TacGia}");
                    System.Diagnostics.Debug.WriteLine($"========== VERIFY SAU KHI LƯU DATABASE ==========");
                    System.Diagnostics.Debug.WriteLine($"  - DaDang: {result.DaDang}");
                    System.Diagnostics.Debug.WriteLine($"  - HienThi: {result.HienThi}");
                    System.Diagnostics.Debug.WriteLine($"=================================================");
                }
                
                if (result != null)
                {
                    TempData["ThanhCong"] = "Đã cập nhật blog thành công!";
                    return RedirectToAction("Blog");
                }
                else
                {
                    TempData["Loi"] = "Có lỗi xảy ra khi cập nhật blog!";
                    ViewBag.DanhSachTheLoai = _theLoaiBlogRepository.LayDanhSachTheLoaiHienThi();
                    return View(blog);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult XoaBlog(int id)
        {
            var blog = _blogRepository.LayBlogTheoId(id);
            if (blog == null)
            {
                TempData["Loi"] = "Không tìm thấy blog!";
                return RedirectToAction("Blog");
            }

            // Xóa hình ảnh nếu có
            if (!string.IsNullOrEmpty(blog.HinhAnh))
            {
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", blog.HinhAnh.TrimStart('/'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            var result = _blogRepository.XoaBlog(id);
            if (result)
            {
                TempData["ThanhCong"] = "Đã xóa blog thành công!";
            }
            else
            {
                TempData["Loi"] = "Có lỗi xảy ra khi xóa blog!";
            }

            return RedirectToAction("Blog");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DangBlog(int id, bool daDang)
        {
            var blog = _blogRepository.LayBlogTheoId(id);
            if (blog == null)
            {
                return Json(new { success = false, message = "Không tìm thấy blog!" });
            }

            blog.DaDang = daDang;
            // Khi hủy đăng (chuyển về bản nháp), reset trạng thái hiển thị
            if (!daDang)
            {
                blog.HienThi = false;
            }
            var result = _blogRepository.CapNhatBlog(blog);
            
            if (result != null)
            {
                return Json(new { success = true, message = daDang ? "Đã đăng blog!" : "Đã chuyển về bản nháp!" });
            }
            else
            {
                return Json(new { success = false, message = "Có lỗi xảy ra!" });
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult HienThiBlog(int id, bool hienThi)
        {
            var blog = _blogRepository.LayBlogTheoId(id);
            if (blog == null)
            {
                return Json(new { success = false, message = "Không tìm thấy blog!" });
            }

            // Chỉ cho phép hiển thị nếu blog đã đăng
            if (hienThi && !blog.DaDang)
            {
                return Json(new { success = false, message = "Blog phải được đăng trước khi hiển thị!" });
            }

            blog.HienThi = hienThi;
            var result = _blogRepository.CapNhatBlog(blog);
            
            if (result != null)
            {
                return Json(new { success = true, message = hienThi ? "Đã hiển thị blog!" : "Đã ẩn blog!" });
            }
            else
            {
                return Json(new { success = false, message = "Có lỗi xảy ra!" });
            }
        }

        // ========== EXTERNAL ARTICLES MANAGEMENT ==========
        public async Task<IActionResult> QuanLyBaiVietAPI(string? keyword = null)
        {
            var danhSachBaiVietChoDuyet = _blogRepository.LayDanhSachBlogChoDuyet();
            ViewBag.DanhSachBaiVietChoDuyet = danhSachBaiVietChoDuyet;
            ViewBag.Keyword = keyword;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TimKiemBaiVietAPI(string? keyword = null, int pageSize = 10)
        {
            try
            {
                _logger.LogInformation("=== TimKiemBaiVietAPI called with keyword: {Keyword}, pageSize: {PageSize}", keyword ?? "null", pageSize);
                
                if (_externalArticleService == null)
                {
                    _logger.LogError("ExternalArticleService is NULL!");
                    return Json(new { success = false, message = "Service chưa được khởi tạo!" });
                }
                
                var articles = await _externalArticleService.FetchArticlesAsync(keyword, pageSize);
                
                _logger.LogInformation("Fetched {Count} articles from external API", articles?.Count ?? 0);
                
                if (articles == null || articles.Count == 0)
                {
                    _logger.LogWarning("No articles returned from service");
                    return Json(new { success = false, message = "Không tìm thấy bài viết nào", data = new List<object>() });
                }
                
                return Json(new { success = true, data = articles, count = articles.Count });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR in TimKiemBaiVietAPI");
                return Json(new { success = false, message = $"Lỗi khi tìm kiếm bài viết: {ex.Message}", details = ex.ToString() });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ImportBaiVietAPI(string articleId, string title, string? description, string content, string? imageUrl, string? author, string? sourceUrl, string? sourceName, string? tags, DateTime? publishedAt)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(title))
                {
                    return Json(new { success = false, message = "Tiêu đề không được để trống!" });
                }

                var existingBlog = _blogRepository.LayBlogTheoApiArticleId(articleId);
                if (existingBlog != null)
                {
                    return Json(new { success = false, message = "Bài viết này đã được import trước đó!" });
                }

                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy người dùng!" });
                }

                // ✅ LẤY NỘI DUNG ĐẦY ĐỦ TỪ API GỐC
                string finalContent = content?.Trim() ?? string.Empty;
                
                _logger.LogInformation("=== BẮT ĐẦU XỬ LÝ CONTENT ===");
                _logger.LogInformation("Content ban đầu từ JavaScript: Length={Length}, Preview={Preview}", 
                    finalContent.Length, 
                    finalContent.Length > 100 ? finalContent.Substring(0, 100) + "..." : finalContent);
                _logger.LogInformation("Description: Length={Length}, Preview={Preview}", 
                    description?.Length ?? 0,
                    description?.Length > 100 ? description.Substring(0, 100) + "..." : description);
                _logger.LogInformation("ArticleId: {ArticleId}, SourceName: {SourceName}", articleId, sourceName);
                
                // LUÔN LUÔN thử lấy full content từ API nếu là Dev.to (vì content ban đầu chỉ là description)
                // Hoặc nếu content quá ngắn (< 500 ký tự) hoặc chỉ là description
                bool shouldFetchFullContent = false;
                
                if (articleId.StartsWith("devto_"))
                {
                    // Dev.to: LUÔN LUÔN lấy full content vì content ban đầu chỉ là description
                    shouldFetchFullContent = true;
                    _logger.LogInformation("🔵 Dev.to article - SẼ lấy full content từ API");
                }
                else if (string.IsNullOrWhiteSpace(finalContent) || 
                         finalContent.Length < 500 || 
                         finalContent == description ||
                         (description != null && finalContent == description))
                {
                    // Các API khác: chỉ lấy nếu content quá ngắn
                    shouldFetchFullContent = true;
                    _logger.LogInformation("🔵 Content quá ngắn hoặc chỉ là description - SẼ lấy full content từ API");
                }
                else
                {
                    _logger.LogInformation("✅ Content đã đủ dài ({Length} ký tự), không cần lấy thêm", finalContent.Length);
                }
                
                if (shouldFetchFullContent)
                {
                    _logger.LogInformation("📡 Đang gọi FetchFullContentAsync...");
                    _logger.LogInformation("ArticleId: {ArticleId}, SourceName: {SourceName}, SourceUrl: {SourceUrl}", 
                        articleId, sourceName, sourceUrl);
                    
                    if (_externalArticleService != null)
                    {
                        try
                        {
                            var fullContent = await _externalArticleService.FetchFullContentAsync(articleId, sourceName ?? "Unknown", sourceUrl);
                            
                            if (!string.IsNullOrWhiteSpace(fullContent) && fullContent.Length > finalContent.Length)
                            {
                                finalContent = fullContent;
                                _logger.LogInformation("✅ Đã lấy được full content từ API, độ dài: {Length} ký tự", finalContent.Length);
                                _logger.LogInformation("Full content preview (first 200 chars): {Preview}", 
                                    finalContent.Length > 200 ? finalContent.Substring(0, 200) + "..." : finalContent);
                            }
                            else
                            {
                                _logger.LogWarning("⚠️ FetchFullContentAsync trả về null hoặc content ngắn hơn. FullContent length: {FullLength}, Current length: {CurrentLength}", 
                                    fullContent?.Length ?? 0, finalContent.Length);
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "❌ Lỗi khi gọi FetchFullContentAsync: {Message}", ex.Message);
                        }
                    }
                    else
                    {
                        _logger.LogError("❌ ExternalArticleService is NULL, không thể lấy full content");
                    }
                }

                // Nếu vẫn không có content, dùng description hoặc title
                if (string.IsNullOrWhiteSpace(finalContent))
                {
                    finalContent = description ?? title;
                    _logger.LogWarning("⚠️ Không có content, sử dụng description hoặc title");
                }

                if (string.IsNullOrWhiteSpace(finalContent))
                {
                    return Json(new { success = false, message = "Nội dung không được để trống!" });
                }

                string? localImagePath = null;
                if (!string.IsNullOrWhiteSpace(imageUrl))
                {
                    localImagePath = await DownloadImageFromUrlAsync(imageUrl, articleId);
                }

                string nguonBaiViet;
                if (!string.IsNullOrWhiteSpace(sourceName))
                {
                    nguonBaiViet = sourceName;
                    if (!string.IsNullOrWhiteSpace(sourceUrl) && sourceUrl != sourceName)
                    {
                        nguonBaiViet += $" ({sourceUrl})";
                    }
                }
                else if (!string.IsNullOrWhiteSpace(sourceUrl))
                {
                    nguonBaiViet = sourceUrl;
                }
                else
                {
                    nguonBaiViet = "External API";
                }

                var blog = new Blog
                {
                    TieuDe = title.Trim(),
                    MoTaNgan = string.IsNullOrWhiteSpace(description) ? title.Substring(0, Math.Min(150, title.Length)) : description.Trim(),
                    NoiDung = finalContent.Trim(),
                    HinhAnh = localImagePath ?? imageUrl,
                    TacGia = string.IsNullOrWhiteSpace(author) ? (sourceName ?? "External Author") : author.Trim(),
                    NguonBaiViet = nguonBaiViet,
                    ApiArticleId = articleId,
                    Tags = tags,
                    DaDang = false,
                    HienThi = false,
                    UserId = currentUser.Id,
                    NgayDang = publishedAt ?? DateTime.Now,
                    IsPermalinkAuto = true
                };

                blog.Permalink = GeneratePermalinkFromTitle(blog.TieuDe ?? string.Empty, null);

                _logger.LogInformation("=== Importing Blog from API ===");
                _logger.LogInformation("Title: {Title}", blog.TieuDe);
                _logger.LogInformation("Author: {Author}", blog.TacGia);
                _logger.LogInformation("Source: {Source}", blog.NguonBaiViet);
                _logger.LogInformation("ArticleId: {ArticleId}", blog.ApiArticleId);
                _logger.LogInformation("Content Length: {Length} characters", blog.NoiDung?.Length ?? 0);
                _logger.LogInformation("Content Preview (first 200 chars): {Preview}", 
                    blog.NoiDung?.Length > 200 ? blog.NoiDung.Substring(0, 200) + "..." : blog.NoiDung);
                _logger.LogInformation("Content Preview (last 200 chars): {Preview}", 
                    blog.NoiDung?.Length > 200 ? "..." + blog.NoiDung.Substring(blog.NoiDung.Length - 200) : blog.NoiDung);
                _logger.LogInformation("Image: {Image}", blog.HinhAnh);

                var result = _blogRepository.ThemBlog(blog);
                if (result != null)
                {
                    if (result.Permalink == "blog-post" || _blogRepository.LayBlogTheoPermalink(result.Permalink) != null)
                    {
                        result.Permalink = GeneratePermalinkFromTitle(result.TieuDe, result.MaBlog);
                        _blogRepository.CapNhatBlog(result);
                    }

                    _logger.LogInformation("✅ Successfully imported blog #{MaBlog} - {Title} | Author: {Author} | Source: {Source}", 
                        result.MaBlog, result.TieuDe, result.TacGia, result.NguonBaiViet);
                    return Json(new { success = true, message = "Đã import bài viết thành công! Vui lòng duyệt bài viết.", maBlog = result.MaBlog });
                }
                else
                {
                    _logger.LogError("Failed to import blog from API - repository returned null");
                    return Json(new { success = false, message = "Có lỗi xảy ra khi import bài viết!" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error importing blog from API: {Message}", ex.Message);
                return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> KiemTraBlogDatabase()
        {
            try
            {
                var allBlogs = _context.Blogs.ToList();
                var blogsWithInvalidUser = new List<object>();
                var blogsWithoutTitle = new List<object>();
                var blogsWithoutContent = new List<object>();

                foreach (var blog in allBlogs)
                {
                    if (!string.IsNullOrEmpty(blog.UserId))
                    {
                        var user = await _userManager.FindByIdAsync(blog.UserId);
                        if (user == null)
                        {
                            blogsWithInvalidUser.Add(new { 
                                MaBlog = blog.MaBlog, 
                                TieuDe = blog.TieuDe, 
                                UserId = blog.UserId,
                                NguonBaiViet = blog.NguonBaiViet
                            });
                        }
                    }

                    if (string.IsNullOrEmpty(blog.TieuDe))
                    {
                        blogsWithoutTitle.Add(new { 
                            MaBlog = blog.MaBlog, 
                            UserId = blog.UserId,
                            NguonBaiViet = blog.NguonBaiViet
                        });
                    }

                    if (string.IsNullOrEmpty(blog.NoiDung))
                    {
                        blogsWithoutContent.Add(new { 
                            MaBlog = blog.MaBlog, 
                            TieuDe = blog.TieuDe,
                            NguonBaiViet = blog.NguonBaiViet
                        });
                    }
                }

                return Json(new
                {
                    success = true,
                    totalBlogs = allBlogs.Count,
                    blogsWithInvalidUser = blogsWithInvalidUser,
                    blogsWithoutTitle = blogsWithoutTitle,
                    blogsWithoutContent = blogsWithoutContent,
                    message = "Kiểm tra hoàn tất!"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking blog database");
                return Json(new { success = false, message = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SuaBlogDatabase()
        {
            try
            {
                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy người dùng hiện tại!" });
                }

                var allBlogs = _context.Blogs.ToList();
                int fixedCount = 0;

                foreach (var blog in allBlogs)
                {
                    bool needsUpdate = false;

                    if (!string.IsNullOrEmpty(blog.UserId))
                    {
                        var user = await _userManager.FindByIdAsync(blog.UserId);
                        if (user == null)
                        {
                            blog.UserId = null;
                            needsUpdate = true;
                        }
                    }

                    if (string.IsNullOrEmpty(blog.TieuDe))
                    {
                        blog.TieuDe = $"Blog #{blog.MaBlog}";
                        needsUpdate = true;
                    }

                    if (string.IsNullOrEmpty(blog.NoiDung))
                    {
                        blog.NoiDung = blog.MoTaNgan ?? "Nội dung đang được cập nhật...";
                        needsUpdate = true;
                    }

                    if (needsUpdate)
                    {
                        fixedCount++;
                    }
                }

                if (fixedCount > 0)
                {
                    await _context.SaveChangesAsync();
                }

                return Json(new { 
                    success = true, 
                    message = $"Đã sửa {fixedCount} bài viết có vấn đề!" 
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fixing blog database");
                return Json(new { success = false, message = ex.Message });
            }
        }

        // ========== THE LOAI BLOG MANAGEMENT ==========
        public IActionResult TheLoaiBlog(string? search = null)
        {
            var danhSachTheLoai = _theLoaiBlogRepository.LayDanhSachTheLoai();
            
            // Tìm kiếm
            if (!string.IsNullOrEmpty(search))
            {
                var searchLower = search.ToLower();
                danhSachTheLoai = danhSachTheLoai.Where(t =>
                    t.TenTheLoai != null && t.TenTheLoai.ToLower().Contains(searchLower)
                ).ToList();
            }
            
            ViewBag.Search = search;
            
            return View(danhSachTheLoai);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ThemTheLoaiBlog(TheLoaiBlog theLoai)
        {
            if (string.IsNullOrWhiteSpace(theLoai.TenTheLoai))
            {
                TempData["Loi"] = "Vui lòng điền tên thể loại!";
                return RedirectToAction("TheLoaiBlog");
            }

            theLoai.ThuTu = 0;
            theLoai.HienThi = true;

            var result = _theLoaiBlogRepository.ThemTheLoai(theLoai);
            if (result != null)
            {
                TempData["ThanhCong"] = "Đã thêm thể loại blog thành công!";
            }
            else
            {
                TempData["Loi"] = "Có lỗi xảy ra khi thêm thể loại blog!";
            }

            return RedirectToAction("TheLoaiBlog");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SuaTheLoaiBlog(TheLoaiBlog theLoai)
        {
            if (string.IsNullOrWhiteSpace(theLoai.TenTheLoai))
            {
                TempData["Loi"] = "Vui lòng điền tên thể loại!";
                return RedirectToAction("TheLoaiBlog");
            }

            // Get existing record to preserve other fields
            var theLoaiHienTai = _theLoaiBlogRepository.LayTheLoaiTheoId(theLoai.MaTheLoai);
            if (theLoaiHienTai == null)
            {
                TempData["Loi"] = "Không tìm thấy thể loại!";
                return RedirectToAction("TheLoaiBlog");
            }

            // Cập nhật thông tin bằng AutoMapper
            // AutoMapper sẽ tự động map các thuộc tính và bỏ qua các thuộc tính đặc biệt
            _mapper.Map(theLoai, theLoaiHienTai);

            var result = _theLoaiBlogRepository.CapNhatTheLoai(theLoaiHienTai);
            if (result != null)
            {
                TempData["ThanhCong"] = "Đã cập nhật thể loại blog thành công!";
            }
            else
            {
                TempData["Loi"] = "Có lỗi xảy ra khi cập nhật thể loại blog!";
            }

            return RedirectToAction("TheLoaiBlog");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult XoaTheLoaiBlog(int id)
        {
            var result = _theLoaiBlogRepository.XoaTheLoai(id);
            if (result)
            {
                TempData["ThanhCong"] = "Đã xóa thể loại blog thành công!";
            }
            else
            {
                TempData["Loi"] = "Không thể xóa thể loại này vì đang có blog sử dụng hoặc có lỗi xảy ra!";
            }

            return RedirectToAction("TheLoaiBlog");
        }

        /// <summary>
        /// Tạo permalink từ tiêu đề bài viết
        /// </summary>
        private string GeneratePermalinkFromTitle(string title, int? excludeMaBlog = null)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return GenerateUniquePermalink("blog-post", excludeMaBlog);
            }

            // Chuyển thành chữ thường
            var permalink = title.ToLowerInvariant();

            // Loại bỏ dấu tiếng Việt
            permalink = System.Text.RegularExpressions.Regex.Replace(
                permalink.Normalize(System.Text.NormalizationForm.FormD),
                @"\p{Mn}",
                string.Empty
            );

            // Thay thế ký tự đặc biệt và khoảng trắng bằng dấu gạch ngang
            permalink = System.Text.RegularExpressions.Regex.Replace(permalink, @"[^a-z0-9]+", "-");

            // Loại bỏ dấu gạch ngang ở đầu và cuối
            permalink = permalink.Trim('-');

            // Nếu rỗng sau khi xử lý, trả về giá trị mặc định
            if (string.IsNullOrWhiteSpace(permalink))
            {
                permalink = "blog-post";
            }

            // Tạo permalink duy nhất (thêm số phía sau nếu trùng)
            return GenerateUniquePermalink(permalink, excludeMaBlog);
        }

        private string GenerateUniquePermalink(string basePermalink, int? excludeMaBlog = null)
        {
            // ✅ Nếu basePermalink là "blog-post" (mặc định) và chưa có MaBlog, thêm timestamp ngay để tránh trùng
            // Điều này đảm bảo mỗi blog mới có permalink duy nhất ngay từ đầu
            if (basePermalink == "blog-post" && !excludeMaBlog.HasValue)
            {
                // Tạo permalink duy nhất với timestamp để tránh trùng khi tạo nhiều blog mới
                var timestampPermalink = $"{basePermalink}-{DateTime.Now:yyyyMMddHHmmss}";
                var existingBlog = _blogRepository.LayBlogTheoPermalink(timestampPermalink);
                
                // Nếu vẫn trùng (rất hiếm), thêm thêm milliseconds
                if (existingBlog != null)
                {
                    timestampPermalink = $"{basePermalink}-{DateTime.Now:yyyyMMddHHmmssfff}";
                }
                
                return timestampPermalink;
            }
            
            // Kiểm tra xem permalink cơ bản đã tồn tại chưa
            var existingBlogCheck = _blogRepository.LayBlogTheoPermalink(basePermalink);
            
            // Nếu permalink chưa tồn tại hoặc là blog hiện tại đang sửa, trả về permalink cơ bản
            if (existingBlogCheck == null || (excludeMaBlog.HasValue && existingBlogCheck.MaBlog == excludeMaBlog.Value))
            {
                return basePermalink;
            }

            // Nếu permalink đã tồn tại, thử thêm số phía sau (từ 1 đến 999)
            for (int i = 1; i <= 999; i++)
            {
                var newPermalink = $"{basePermalink}-{i}";
                var checkBlog = _blogRepository.LayBlogTheoPermalink(newPermalink);
                
                // Nếu permalink chưa tồn tại hoặc là blog hiện tại đang sửa, trả về permalink mới
                if (checkBlog == null || (excludeMaBlog.HasValue && checkBlog.MaBlog == excludeMaBlog.Value))
                {
                    return newPermalink;
                }
            }

            // Nếu không tìm được permalink duy nhất trong phạm vi 1-999, thêm timestamp
            return $"{basePermalink}-{DateTime.Now:yyyyMMddHHmmss}";
        }

        // GET: Lấy thông báo cho admin
        [HttpGet]
        public IActionResult GetNotifications()
        {
            try
            {
                var notifications = new List<NotificationItem>();
                var now = DateTime.Now;
                var threeDaysFromNow = now.AddDays(3);
                var sevenDaysAgo = now.AddDays(-7);

                // 1. Tin tuyển dụng chờ duyệt
                var tinChoDuyet = _tinTuyenDungRepository.LayDanhSachTinTuyenDung()
                    .Where(t => t.TrangThaiDuyet == "Cho duyet")
                    .OrderByDescending(t => t.NgayDang)
                    .Take(10)
                    .ToList();

                foreach (var tin in tinChoDuyet)
                {
                    notifications.Add(new NotificationItem
                    {
                        Type = "PendingApproval",
                        Title = "Tin tuyển dụng chờ duyệt",
                        Message = $"Tin \"{tin.TenViecLam}\" từ {tin.NhaTuyenDung?.TenCongTy ?? tin.CongTy ?? "N/A"} đang chờ duyệt",
                        Icon = "bi-hourglass-split",
                        Color = "text-warning",
                        Url = Url.Action("ChiTietTinTuyenDung", "Admin", new { area = "Admin", id = tin.MaTinTuyenDung }),
                        CreatedAt = tin.NgayDang,
                        RelatedId = tin.MaTinTuyenDung,
                        NotificationKey = $"PendingApproval_{tin.MaTinTuyenDung}_{tin.NgayDang:yyyyMMddHHmmss}"
                    });
                }

                // 2. Đơn ứng tuyển mới (trong 7 ngày gần đây)
                var donUngTuyenMoi = _tinUngTuyenRepository.LayDanhSachTinUngTuyen()
                    .Where(t => t.NgayUngTuyen >= sevenDaysAgo)
                    .OrderByDescending(t => t.NgayUngTuyen)
                    .Take(10)
                    .ToList();

                foreach (var don in donUngTuyenMoi)
                {
                    // Chuyển đổi MaTinTuyenDung từ string sang int
                    TinTuyenDung? tinTuyenDung = null;
                    if (int.TryParse(don.MaTinTuyenDung, out int maTinTuyenDung))
                    {
                        tinTuyenDung = _tinTuyenDungRepository.LayTinTuyenDungTheoId(maTinTuyenDung);
                    }
                    
                    // Tìm UngVien từ UserId để lấy MaUngVien
                    var ungVien = !string.IsNullOrEmpty(don.UserId) 
                        ? _ungVienRepository.LayDanhSachUngVien().FirstOrDefault(u => u.UserId == don.UserId)
                        : null;
                    
                    // Nếu có UngVien thì link đến ChiTietUngVien, nếu không thì link đến Dashboard
                    var url = ungVien != null 
                        ? Url.Action("ChiTietUngVien", "Admin", new { area = "Admin", id = ungVien.MaUngVien })
                        : Url.Action("Dashboard", "Admin", new { area = "Admin" });
                    
                    notifications.Add(new NotificationItem
                    {
                        Type = "NewApplication",
                        Title = "Đơn ứng tuyển mới",
                        Message = $"{don.HoTen} đã ứng tuyển vào vị trí \"{tinTuyenDung?.TenViecLam ?? don.ViTriUngTuyen}\"",
                        Icon = "bi-person-plus-fill",
                        Color = "text-primary",
                        Url = url,
                        CreatedAt = don.NgayUngTuyen,
                        RelatedId = don.MaTinUngTuyen,
                        NotificationKey = $"NewApplication_{don.MaTinUngTuyen}_{don.NgayUngTuyen:yyyyMMddHHmmss}"
                    });
                }

                // 3. Tin tuyển dụng sắp hết hạn (trong 3 ngày tới)
                var tinSapHetHan = _tinTuyenDungRepository.LayDanhSachTinTuyenDung()
                    .Where(t => t.HanNop >= now && t.HanNop <= threeDaysFromNow && 
                                t.TrangThaiDuyet == "Da duyet" &&
                                (t.TrangThai == "Dang tuyen" || string.IsNullOrEmpty(t.TrangThai)))
                    .OrderBy(t => t.HanNop)
                    .ToList();

                foreach (var tin in tinSapHetHan)
                {
                    var daysLeft = (tin.HanNop.Date - now.Date).Days;
                    notifications.Add(new NotificationItem
                    {
                        Type = "ExpiringSoon",
                        Title = "Tin tuyển dụng sắp hết hạn",
                        Message = $"Tin \"{tin.TenViecLam}\" sẽ hết hạn sau {daysLeft} ngày ({tin.HanNop:dd/MM/yyyy})",
                        Icon = "bi-clock-history",
                        Color = "text-warning",
                        Url = Url.Action("ChiTietTinTuyenDung", "Admin", new { area = "Admin", id = tin.MaTinTuyenDung }),
                        CreatedAt = tin.HanNop,
                        RelatedId = tin.MaTinTuyenDung,
                        NotificationKey = $"ExpiringSoon_{tin.MaTinTuyenDung}_{tin.HanNop:yyyyMMdd}"
                    });
                }

                // 4. Tin tuyển dụng đã hết hạn (chưa đóng)
                var tinHetHan = _tinTuyenDungRepository.LayDanhSachTinTuyenDung()
                    .Where(t => t.HanNop < now && 
                                t.TrangThaiDuyet == "Da duyet" &&
                                (t.TrangThai == "Dang tuyen" || string.IsNullOrEmpty(t.TrangThai) || t.TrangThai == "Het han"))
                    .OrderByDescending(t => t.HanNop)
                    .Take(5)
                    .ToList();

                foreach (var tin in tinHetHan)
                {
                    notifications.Add(new NotificationItem
                    {
                        Type = "Expired",
                        Title = "Tin tuyển dụng đã hết hạn",
                        Message = $"Tin \"{tin.TenViecLam}\" đã hết hạn từ ngày {tin.HanNop:dd/MM/yyyy}",
                        Icon = "bi-exclamation-triangle-fill",
                        Color = "text-danger",
                        Url = Url.Action("ChiTietTinTuyenDung", "Admin", new { area = "Admin", id = tin.MaTinTuyenDung }),
                        CreatedAt = tin.HanNop,
                        RelatedId = tin.MaTinTuyenDung,
                        NotificationKey = $"Expired_{tin.MaTinTuyenDung}_{tin.HanNop:yyyyMMdd}"
                    });
                }

                // 5. Nhà tuyển dụng mới đăng ký (trong 7 ngày gần đây)
                var nhaTuyenDungMoi = _nhaTuyenDungRepository.LayDanhSachNhaTuyenDung()
                    .Where(n => n.User != null && n.User.NgayDangKy >= sevenDaysAgo)
                    .OrderByDescending(n => n.User!.NgayDangKy)
                    .Take(5)
                    .ToList();

                foreach (var ntd in nhaTuyenDungMoi)
                {
                    if (ntd.User != null)
                    {
                        notifications.Add(new NotificationItem
                        {
                            Type = "NewRecruiter",
                            Title = "Nhà tuyển dụng mới đăng ký",
                            Message = $"{ntd.TenCongTy} đã đăng ký tài khoản mới",
                            Icon = "bi-building",
                            Color = "text-info",
                            Url = Url.Action("ChiTietNhaTuyenDung", "Admin", new { area = "Admin", id = ntd.MaNhaTuyenDung }),
                            CreatedAt = ntd.User.NgayDangKy,
                            RelatedId = ntd.MaNhaTuyenDung,
                            NotificationKey = $"NewRecruiter_{ntd.MaNhaTuyenDung}_{ntd.User.NgayDangKy:yyyyMMddHHmmss}"
                        });
                    }
                }

                // 6. Ứng viên mới đăng ký (trong 7 ngày gần đây)
                var ungVienMoi = _ungVienRepository.LayDanhSachUngVien()
                    .Where(u => u.User != null && u.User.NgayDangKy >= sevenDaysAgo)
                    .OrderByDescending(u => u.User!.NgayDangKy)
                    .Take(5)
                    .ToList();

                foreach (var uv in ungVienMoi)
                {
                    if (uv.User != null)
                    {
                        notifications.Add(new NotificationItem
                        {
                            Type = "NewCandidate",
                            Title = "Ứng viên mới đăng ký",
                            Message = $"{uv.HoTen} đã đăng ký tài khoản mới",
                            Icon = "bi-person-badge",
                            Color = "text-info",
                            Url = Url.Action("ChiTietUngVien", "Admin", new { area = "Admin", id = uv.MaUngVien }),
                            CreatedAt = uv.User.NgayDangKy,
                            RelatedId = uv.MaUngVien,
                            NotificationKey = $"NewCandidate_{uv.MaUngVien}_{uv.User.NgayDangKy:yyyyMMddHHmmss}"
                        });
                    }
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

