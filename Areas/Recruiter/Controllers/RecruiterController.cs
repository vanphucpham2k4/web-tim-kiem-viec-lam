using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Unicareer.Models;
using Unicareer.Models.ViewModels;
using Unicareer.Models.Enums;
using Unicareer.Repository;
using Unicareer.Data;
using Microsoft.EntityFrameworkCore;
using Unicareer.Services;

namespace Unicareer.Areas.Recruiter.Controllers
{
    [Area("Recruiter")]
    [Authorize(Roles = $"{SD.Role_NhaTuyenDung}")]
    public class RecruiterController : Controller
    {
        private readonly ITinTuyenDungRepository _tinTuyenDungRepository;
        private readonly ITinUngTuyenRepository _tinUngTuyenRepository;
        private readonly INganhNgheRepository _nganhNgheRepository;
        private readonly IChuyenNganhRepository _chuyenNganhRepository;
        private readonly ILoaiCongViecRepository _loaiCongViecRepository;
        private readonly INhaTuyenDungRepository _nhaTuyenDungRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        public RecruiterController(
            ITinTuyenDungRepository tinTuyenDungRepository, 
            ITinUngTuyenRepository tinUngTuyenRepository,
            INganhNgheRepository nganhNgheRepository,
            IChuyenNganhRepository chuyenNganhRepository,
            ILoaiCongViecRepository loaiCongViecRepository,
            INhaTuyenDungRepository nhaTuyenDungRepository,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            IEmailService emailService)
        {
            _tinTuyenDungRepository = tinTuyenDungRepository;
            _tinUngTuyenRepository = tinUngTuyenRepository;
            _nganhNgheRepository = nganhNgheRepository;
            _chuyenNganhRepository = chuyenNganhRepository;
            _loaiCongViecRepository = loaiCongViecRepository;
            _nhaTuyenDungRepository = nhaTuyenDungRepository;
            _userManager = userManager;
            _context = context;
            _emailService = emailService;
        }
        // GET: Trang chu nha tuyen dung
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            // Lấy thông tin công ty của user đăng nhập
            var nhaTuyenDung = _nhaTuyenDungRepository.LayNhaTuyenDungTheoUserId(user.Id);
            if (nhaTuyenDung == null)
            {
                // Nếu chưa có thông tin công ty, trả về dữ liệu rỗng
                ViewBag.TongTinDaDang = 0;
                ViewBag.TongDonUngTuyen = 0;
                ViewBag.TongLuotXem = 0;
                ViewBag.TongUngVienPhuHop = 0;
                ViewBag.ChartData = new { labels = new string[0], views = new int[0], applications = new int[0] };
                return View();
            }

            // Lấy tin tuyển dụng theo mã nhà tuyển dụng (foreign key)
            var danhSachTinTuyenDung = _tinTuyenDungRepository.LayDanhSachTheoMaNhaTuyenDung(nhaTuyenDung.MaNhaTuyenDung);
            
            // Lấy danh sách mã tin tuyển dụng của công ty
            var danhSachMaTin = danhSachTinTuyenDung.Select(t => t.MaTinTuyenDung.ToString()).ToList();
            
            // Lấy đơn ứng tuyển theo các tin tuyển dụng của công ty
            var danhSachTinUngTuyen = _tinUngTuyenRepository.LayDanhSachTinUngTuyen()
                .Where(t => danhSachMaTin.Contains(t.MaTinTuyenDung))
                .ToList();
            
            // Thống kê
            var tongTinDaDang = danhSachTinTuyenDung.Count;
            var tongDonUngTuyen = danhSachTinUngTuyen.Count;
            var tongLuotXem = danhSachTinTuyenDung.Sum(t => (t.SoLuongUngTuyen ?? 0) * 10); // Ước tính lượt xem
            var tongUngVienPhuHop = danhSachTinUngTuyen.Count(t => 
            {
                var trangThai = TrangThaiXuLyHelper.FromString(t.TrangThaiXuLy);
                return trangThai.HasValue && trangThai.Value == TrangThaiXuLy.TuyenDung;
            });
            
            // Dữ liệu chart cho 7 ngày gần nhất
            var ngayHienTai = DateTime.Now.Date;
            var chartLabels = new List<string>();
            var chartViews = new List<int>();
            var chartApplications = new List<int>();
            
            for (int i = 6; i >= 0; i--)
            {
                var ngay = ngayHienTai.AddDays(-i);
                var ngayStr = ngay.DayOfWeek switch
                {
                    DayOfWeek.Monday => "T2",
                    DayOfWeek.Tuesday => "T3",
                    DayOfWeek.Wednesday => "T4",
                    DayOfWeek.Thursday => "T5",
                    DayOfWeek.Friday => "T6",
                    DayOfWeek.Saturday => "T7",
                    DayOfWeek.Sunday => "CN",
                    _ => ""
                };
                chartLabels.Add(ngayStr);
                
                // Đếm lượt xem (ước tính từ số lượng ứng tuyển)
                var views = danhSachTinTuyenDung
                    .Where(t => t.NgayDang.Date <= ngay)
                    .Sum(t => (t.SoLuongUngTuyen ?? 0) * 10);
                chartViews.Add(views);
                
                // Đếm đơn ứng tuyển
                var applications = danhSachTinUngTuyen
                    .Count(t => t.NgayUngTuyen.Date == ngay);
                chartApplications.Add(applications);
            }
            
            ViewBag.TongTinDaDang = tongTinDaDang;
            ViewBag.TongDonUngTuyen = tongDonUngTuyen;
            ViewBag.TongLuotXem = tongLuotXem;
            ViewBag.TongUngVienPhuHop = tongUngVienPhuHop;
            ViewBag.ChartData = new { 
                labels = chartLabels.ToArray(), 
                views = chartViews.ToArray(), 
                applications = chartApplications.ToArray() 
            };
            
            return View();
        }

        // GET: Form dang tin tuyen dung
        public IActionResult DangTinTuyenDung()
        {
            ViewData["Title"] = "Dang tuyen dung";
            
            // Lấy danh sách tỉnh/thành phố
            var danhSachTinhThanh = _context.Provinces
                .OrderBy(p => p.FullName)
                .ToList();
            
            // Lấy danh sách ngành nghề
            var danhSachNganhNghe = _nganhNgheRepository.LayDanhSachNganhNghe();
            
            // Lấy danh sách loại công việc
            var danhSachLoaiCongViec = _loaiCongViecRepository.LayDanhSachLoaiCongViec();
            
            // Lấy danh sách chuyên ngành (để hiển thị khi có giá trị sẵn)
            var danhSachChuyenNganh = _chuyenNganhRepository.LayDanhSachChuyenNganh();
            
            ViewBag.DanhSachTinhThanh = danhSachTinhThanh;
            ViewBag.DanhSachNganhNghe = danhSachNganhNghe;
            ViewBag.DanhSachLoaiCongViec = danhSachLoaiCongViec;
            ViewBag.DanhSachChuyenNganh = danhSachChuyenNganh;
            return View();
        }

        // POST: Xu ly dang tin tuyen dung
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DangTinTuyenDung(TinTuyenDung model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Lấy thông tin công ty từ user đăng nhập
                    var user = await _userManager.GetUserAsync(User);
                    if (user == null)
                    {
                        ModelState.AddModelError("", "Không tìm thấy thông tin người dùng!");
                        return View(model);
                    }

                    var nhaTuyenDung = _nhaTuyenDungRepository.LayNhaTuyenDungTheoUserId(user.Id);
                    if (nhaTuyenDung == null || string.IsNullOrEmpty(nhaTuyenDung.TenCongTy))
                    {
                        ModelState.AddModelError("", "Vui lòng cập nhật thông tin công ty trước khi đăng tin!");
                        TempData["Loi"] = "Vui lòng cập nhật thông tin công ty trong phần Cài đặt trước khi đăng tin tuyển dụng!";
                        return RedirectToAction("CaiDat");
                    }

                    // Set foreign key và tên công ty
                    model.MaNhaTuyenDung = nhaTuyenDung.MaNhaTuyenDung;
                    model.CongTy = nhaTuyenDung.TenCongTy;
                    
                    // Xử lý upload ảnh văn phòng/cửa hàng
                    var anhVanPhongFiles = Request.Form.Files.GetFiles("AnhVanPhong");
                    if (anhVanPhongFiles != null && anhVanPhongFiles.Count > 0)
                    {
                        var danhSachAnh = new List<string>();
                        foreach (var file in anhVanPhongFiles)
                        {
                            if (file != null && file.Length > 0)
                            {
                                try
                                {
                                    var imagePath = await UploadFileAsync(file, "vanphong", user.Id);
                                    if (!string.IsNullOrEmpty(imagePath))
                                    {
                                        danhSachAnh.Add(imagePath);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    // Log lỗi nhưng không dừng quá trình upload
                                    ModelState.AddModelError("", $"Lỗi khi upload ảnh {file.FileName}: {ex.Message}");
                                }
                            }
                        }
                        
                        // Lưu danh sách ảnh dưới dạng JSON array
                        if (danhSachAnh.Count > 0)
                        {
                            model.AnhVanPhong = System.Text.Json.JsonSerializer.Serialize(danhSachAnh);
                        }
                    }
                    
                    // Lưu tin vào repository
                    var tinDaTao = _tinTuyenDungRepository.ThemTinTuyenDung(model);
                    
                    TempData["ThanhCong"] = "Đăng tin tuyển dụng thành công!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Có lỗi xảy ra khi đăng tin: " + ex.Message);
                }
            }
            
            // Nếu có lỗi validation, hiển thị lại form với lỗi
            ViewData["Title"] = "Dang tuyen dung";
            
            // Lấy danh sách tỉnh/thành phố
            var danhSachTinhThanh = _context.Provinces
                .OrderBy(p => p.FullName)
                .ToList();
            
            // Lấy danh sách ngành nghề
            var danhSachNganhNghe = _nganhNgheRepository.LayDanhSachNganhNghe();
            
            // Lấy danh sách loại công việc
            var danhSachLoaiCongViec = _loaiCongViecRepository.LayDanhSachLoaiCongViec();
            
            // Lấy danh sách chuyên ngành (để hiển thị khi có giá trị sẵn)
            var danhSachChuyenNganh = _chuyenNganhRepository.LayDanhSachChuyenNganh();
            
            ViewBag.DanhSachTinhThanh = danhSachTinhThanh;
            ViewBag.DanhSachNganhNghe = danhSachNganhNghe;
            ViewBag.DanhSachLoaiCongViec = danhSachLoaiCongViec;
            ViewBag.DanhSachChuyenNganh = danhSachChuyenNganh;
            return View(model);
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

        // GET: Danh sach tin da dang
        public async Task<IActionResult> TinDaDang(int pageNumber = 1, int pageSize = 10, string? searchTerm = null, string? trangThaiFilter = null)
        {
            ViewData["Title"] = "Tin da dang";
            
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            // Lấy thông tin công ty của user đăng nhập
            var nhaTuyenDung = _nhaTuyenDungRepository.LayNhaTuyenDungTheoUserId(user.Id);
            if (nhaTuyenDung == null || string.IsNullOrEmpty(nhaTuyenDung.TenCongTy))
            {
                // Nếu chưa có thông tin công ty, trả về danh sách rỗng
                var emptyModel = new TinDaDangViewModel
                {
                    TinTuyenDungs = new List<TinTuyenDung>(),
                    PageNumber = 1,
                    PageSize = pageSize,
                    TotalCount = 0,
                    SearchTerm = searchTerm,
                    TrangThaiFilter = trangThaiFilter
                };
                return View(emptyModel);
            }

            // Lấy tin tuyển dụng theo mã nhà tuyển dụng (foreign key)
            var danhSach = _tinTuyenDungRepository.LayDanhSachTheoMaNhaTuyenDung(nhaTuyenDung.MaNhaTuyenDung);
            
            // Tính stats từ toàn bộ dữ liệu (trước khi lọc)
            var totalDangTuyen = danhSach.Count(t => 
                t.TrangThai == "Dang tuyen" || (string.IsNullOrEmpty(t.TrangThai) && t.HanNop >= DateTime.Now));
            var totalHetHan = danhSach.Count(t => 
                t.TrangThai == "Het han" || (string.IsNullOrEmpty(t.TrangThai) && t.HanNop < DateTime.Now) || t.TrangThai == "Da dong");
            var totalUngTuyen = danhSach.Sum(t => t.SoLuongUngTuyen) ?? 0;
            
            // Lọc theo tìm kiếm
            if (!string.IsNullOrEmpty(searchTerm))
            {
                danhSach = danhSach.Where(t => 
                    (t.TenViecLam != null && t.TenViecLam.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                    (t.ViTri != null && t.ViTri.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                ).ToList();
            }
            
            // Lọc theo trạng thái
            if (!string.IsNullOrEmpty(trangThaiFilter))
            {
                danhSach = danhSach.Where(t =>
                {
                    var trangThai = !string.IsNullOrEmpty(t.TrangThai)
                        ? t.TrangThai
                        : (t.HanNop >= DateTime.Now ? "Dang tuyen" : "Het han");
                    return trangThai == trangThaiFilter;
                }).ToList();
            }
            
            // Sắp xếp theo ngày đăng (mới nhất trước)
            danhSach = danhSach.OrderByDescending(t => t.NgayDang).ToList();
            
            // Tính toán phân trang
            var totalCount = danhSach.Count;
            var pagedList = danhSach.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            
            var model = new TinDaDangViewModel
            {
                TinTuyenDungs = pagedList,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                SearchTerm = searchTerm,
                TrangThaiFilter = trangThaiFilter,
                TotalDangTuyen = totalDangTuyen,
                TotalHetHan = totalHetHan,
                TotalUngTuyen = totalUngTuyen
            };
            
            return View(model);
        }

        // GET: Chi tiet tin da dang
        public async Task<IActionResult> ChiTietTinDaDang(int id, int pageNumber = 1, int pageSize = 10, 
            string? searchTerm = null, string? trangThaiFilter = null)
        {
            ViewData["Title"] = "Chi tiết tin đã đăng";
            
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            // Lấy thông tin công ty của user đăng nhập
            var nhaTuyenDung = _nhaTuyenDungRepository.LayNhaTuyenDungTheoUserId(user.Id);
            if (nhaTuyenDung == null || string.IsNullOrEmpty(nhaTuyenDung.TenCongTy))
            {
                TempData["Loi"] = "Vui lòng cập nhật thông tin công ty trước!";
                return RedirectToAction("CaiDat");
            }
            
            var tinTuyenDung = _tinTuyenDungRepository.LayTinTuyenDungTheoId(id);
            if (tinTuyenDung == null)
            {
                TempData["Loi"] = "Không tìm thấy tin tuyển dụng!";
                return RedirectToAction("TinDaDang");
            }

            // Kiểm tra xem tin tuyển dụng có thuộc về công ty của user không (sử dụng foreign key)
            if (tinTuyenDung.MaNhaTuyenDung != nhaTuyenDung.MaNhaTuyenDung)
            {
                TempData["Loi"] = "Bạn không có quyền xem tin tuyển dụng này!";
                return RedirectToAction("TinDaDang");
            }
            
            // Lấy danh sách ứng tuyển theo mã tin
            var danhSachUngTuyen = _tinUngTuyenRepository.LayDanhSachTinUngTuyen()
                .Where(t => t.MaTinTuyenDung == id.ToString())
                .ToList();
            
            // Tính stats từ toàn bộ dữ liệu (trước khi lọc)
            var totalDangXemXet = danhSachUngTuyen.Count(t => 
                Models.Enums.TrangThaiXuLyHelper.FromString(t.TrangThaiXuLy) == Models.Enums.TrangThaiXuLy.DangXemXet);
            var totalChoPhongVan = danhSachUngTuyen.Count(t => 
                Models.Enums.TrangThaiXuLyHelper.FromString(t.TrangThaiXuLy) == Models.Enums.TrangThaiXuLy.ChoPhongVan);
            var totalTuyenDung = danhSachUngTuyen.Count(t => 
                Models.Enums.TrangThaiXuLyHelper.FromString(t.TrangThaiXuLy) == Models.Enums.TrangThaiXuLy.TuyenDung);
            
            // Lọc theo tìm kiếm
            if (!string.IsNullOrEmpty(searchTerm))
            {
                danhSachUngTuyen = danhSachUngTuyen.Where(t => 
                    (t.HoTen != null && t.HoTen.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                    (t.Email != null && t.Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                    (t.SoDienThoai != null && t.SoDienThoai.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                ).ToList();
            }
            
            // Lọc theo trạng thái
            if (!string.IsNullOrEmpty(trangThaiFilter))
            {
                danhSachUngTuyen = danhSachUngTuyen.Where(t =>
                {
                    var trangThaiEnum = Models.Enums.TrangThaiXuLyHelper.FromString(t.TrangThaiXuLy);
                    var trangThaiDisplay = trangThaiEnum.HasValue 
                        ? Models.Enums.TrangThaiXuLyHelper.ToString(trangThaiEnum.Value) 
                        : t.TrangThaiXuLy;
                    return trangThaiDisplay == trangThaiFilter;
                }).ToList();
            }
            
            // Sắp xếp theo ngày ứng tuyển (mới nhất trước)
            danhSachUngTuyen = danhSachUngTuyen.OrderByDescending(t => t.NgayUngTuyen).ToList();
            
            // Tính toán phân trang
            var totalCount = danhSachUngTuyen.Count;
            var pagedList = danhSachUngTuyen.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            
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
            
            ViewBag.DanhSachUngTuyen = pagedList;
            ViewBag.DanhSachTinhThanh = danhSachTinhThanh;
            ViewBag.DanhSachNganhNghe = danhSachNganhNghe;
            ViewBag.DanhSachLoaiCongViec = danhSachLoaiCongViec;
            ViewBag.DanhSachChuyenNganh = danhSachChuyenNganh;
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalCount = totalCount;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            ViewBag.SearchTerm = searchTerm;
            ViewBag.TrangThaiFilter = trangThaiFilter;
            ViewBag.TotalDangXemXet = totalDangXemXet;
            ViewBag.TotalChoPhongVan = totalChoPhongVan;
            ViewBag.TotalTuyenDung = totalTuyenDung;
            
            return View(tinTuyenDung);
        }

        // POST: Cap nhat tin tuyen dung
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CapNhatTinTuyenDung(int id, TinTuyenDung model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            // Lấy thông tin công ty của user đăng nhập
            var nhaTuyenDung = _nhaTuyenDungRepository.LayNhaTuyenDungTheoUserId(user.Id);
            if (nhaTuyenDung == null || string.IsNullOrEmpty(nhaTuyenDung.TenCongTy))
            {
                TempData["Loi"] = "Vui lòng cập nhật thông tin công ty trước!";
                return RedirectToAction("CaiDat");
            }

            // Lấy tin hiện tại để đảm bảo có đầy đủ thông tin
            var tinHienTai = _tinTuyenDungRepository.LayTinTuyenDungTheoId(id);
            if (tinHienTai == null)
            {
                TempData["Loi"] = "Không tìm thấy tin tuyển dụng!";
                return RedirectToAction("TinDaDang");
            }

            // Kiểm tra xem tin tuyển dụng có thuộc về công ty của user không (sử dụng foreign key)
            if (tinHienTai.MaNhaTuyenDung != nhaTuyenDung.MaNhaTuyenDung)
            {
                TempData["Loi"] = "Bạn không có quyền cập nhật tin tuyển dụng này!";
                return RedirectToAction("TinDaDang");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Xử lý danh sách ảnh còn lại sau khi xóa (từ hidden input)
                    var danhSachAnhConLai = new List<string>();
                    
                    // Kiểm tra xem có hidden input RemainingExistingImages không
                    if (Request.Form.ContainsKey("RemainingExistingImages"))
                    {
                        // Có hidden input, lấy giá trị (có thể là empty string nếu đã xóa hết)
                        var remainingExistingImages = Request.Form["RemainingExistingImages"].ToString();
                        
                        if (!string.IsNullOrEmpty(remainingExistingImages))
                        {
                            try
                            {
                                // Parse JSON array từ hidden input
                                var jsonArray = System.Text.Json.JsonSerializer.Deserialize<string[]>(remainingExistingImages);
                                if (jsonArray != null)
                                {
                                    danhSachAnhConLai = jsonArray.ToList();
                                }
                            }
                            catch
                            {
                                // Nếu parse lỗi, thử split bằng dấu phẩy
                                danhSachAnhConLai = remainingExistingImages
                                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                                    .Select(s => s.Trim())
                                    .Where(s => !string.IsNullOrEmpty(s))
                                    .ToList();
                            }
                        }
                        // Nếu remainingExistingImages là empty, danhSachAnhConLai sẽ là empty list (đã xóa hết)
                    }
                    else
                    {
                        // Không có hidden input trong form, có nghĩa là người dùng không thay đổi ảnh cũ
                        // Giữ nguyên ảnh cũ từ database
                        if (!string.IsNullOrEmpty(tinHienTai.AnhVanPhong))
                        {
                            try
                            {
                                if (tinHienTai.AnhVanPhong.Trim().StartsWith("["))
                                {
                                    var jsonArray = System.Text.Json.JsonSerializer.Deserialize<string[]>(tinHienTai.AnhVanPhong);
                                    if (jsonArray != null)
                                    {
                                        danhSachAnhConLai = jsonArray.ToList();
                                    }
                                }
                                else
                                {
                                    danhSachAnhConLai = tinHienTai.AnhVanPhong
                                        .Split(',', StringSplitOptions.RemoveEmptyEntries)
                                        .Select(s => s.Trim())
                                        .Where(s => !string.IsNullOrEmpty(s))
                                        .ToList();
                                }
                            }
                            catch
                            {
                                danhSachAnhConLai = tinHienTai.AnhVanPhong
                                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                                    .Select(s => s.Trim())
                                    .Where(s => !string.IsNullOrEmpty(s))
                                    .ToList();
                            }
                        }
                    }
                    
                    // Xử lý upload ảnh văn phòng/cửa hàng mới (nếu có)
                    var anhVanPhongFiles = Request.Form.Files.GetFiles("AnhVanPhong");
                    if (anhVanPhongFiles != null && anhVanPhongFiles.Count > 0)
                    {
                        var danhSachAnhMoi = new List<string>();
                        
                        // Upload các ảnh mới
                        foreach (var file in anhVanPhongFiles)
                        {
                            if (file != null && file.Length > 0)
                            {
                                try
                                {
                                    var imagePath = await UploadFileAsync(file, "vanphong", user.Id);
                                    if (!string.IsNullOrEmpty(imagePath))
                                    {
                                        danhSachAnhMoi.Add(imagePath);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    // Log lỗi nhưng không dừng quá trình upload
                                    ModelState.AddModelError("", $"Lỗi khi upload ảnh {file.FileName}: {ex.Message}");
                                }
                            }
                        }
                        
                        // Gộp danh sách ảnh còn lại và ảnh mới
                        danhSachAnhConLai.AddRange(danhSachAnhMoi);
                    }
                    
                    // Lưu danh sách ảnh dưới dạng JSON array
                    if (danhSachAnhConLai.Count > 0)
                    {
                        model.AnhVanPhong = System.Text.Json.JsonSerializer.Serialize(danhSachAnhConLai);
                    }
                    else
                    {
                        model.AnhVanPhong = null;
                    }
                    
                    var tinDaCapNhat = _tinTuyenDungRepository.CapNhatTinTuyenDung(id, model);
                    if (tinDaCapNhat == null)
                    {
                        TempData["Loi"] = "Không tìm thấy tin tuyển dụng để cập nhật!";
                        return RedirectToAction("TinDaDang");
                    }
                    
                    TempData["ThanhCong"] = "Cập nhật tin tuyển dụng thành công!";
                    return RedirectToAction("ChiTietTinDaDang", new { id = id });
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
            tinHienTai.AnhVanPhong = model.AnhVanPhong ?? tinHienTai.AnhVanPhong;
            
            ViewData["Title"] = "Chi tiết tin tuyển dụng";
            var danhSachUngTuyen = _tinUngTuyenRepository.LayDanhSachTinUngTuyen()
                .Where(t => t.MaTinTuyenDung == id.ToString())
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
            
            ViewBag.DanhSachUngTuyen = danhSachUngTuyen;
            ViewBag.DanhSachTinhThanh = danhSachTinhThanh;
            ViewBag.DanhSachNganhNghe = danhSachNganhNghe;
            ViewBag.DanhSachLoaiCongViec = danhSachLoaiCongViec;
            ViewBag.DanhSachChuyenNganh = danhSachChuyenNganh;
            return View("ChiTietTinDaDang", tinHienTai);
        }

        // POST: Xoa tin tuyen dung
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> XoaTinTuyenDung(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Json(new { success = false, message = "Không tìm thấy người dùng!" });
            }

            // Lấy thông tin công ty của user đăng nhập
            var nhaTuyenDung = _nhaTuyenDungRepository.LayNhaTuyenDungTheoUserId(user.Id);
            if (nhaTuyenDung == null || string.IsNullOrEmpty(nhaTuyenDung.TenCongTy))
            {
                return Json(new { success = false, message = "Vui lòng cập nhật thông tin công ty trước!" });
            }

            // Lấy tin hiện tại để kiểm tra quyền
            var tinTuyenDung = _tinTuyenDungRepository.LayTinTuyenDungTheoId(id);
            if (tinTuyenDung == null)
            {
                return Json(new { success = false, message = "Không tìm thấy tin tuyển dụng!" });
            }

            // Kiểm tra xem tin tuyển dụng có thuộc về công ty của user không
            if (tinTuyenDung.MaNhaTuyenDung != nhaTuyenDung.MaNhaTuyenDung)
            {
                return Json(new { success = false, message = "Bạn không có quyền xóa tin tuyển dụng này!" });
            }

            try
            {
                var ketQua = _tinTuyenDungRepository.XoaTinTuyenDung(id);
                if (ketQua)
                {
                    return Json(new { success = true, message = "Xóa tin tuyển dụng thành công!" });
                }
                else
                {
                    return Json(new { success = false, message = "Không thể xóa tin tuyển dụng!" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra khi xóa tin: " + ex.Message });
            }
        }

        // POST: Cap nhat trang thai tin tuyen dung
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CapNhatTrangThaiTinTuyenDung(int id, string trangThai)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Json(new { success = false, message = "Không tìm thấy người dùng" });
            }

            // Lấy thông tin công ty của user đăng nhập
            var nhaTuyenDung = _nhaTuyenDungRepository.LayNhaTuyenDungTheoUserId(user.Id);
            if (nhaTuyenDung == null || string.IsNullOrEmpty(nhaTuyenDung.TenCongTy))
            {
                return Json(new { success = false, message = "Không tìm thấy thông tin công ty" });
            }

            // Lấy tin tuyển dụng từ database
            var tinTuyenDung = _tinTuyenDungRepository.LayTinTuyenDungTheoId(id);
            if (tinTuyenDung == null)
            {
                return Json(new { success = false, message = "Không tìm thấy tin tuyển dụng" });
            }

            // Kiểm tra quyền truy cập
            if (tinTuyenDung.MaNhaTuyenDung != nhaTuyenDung.MaNhaTuyenDung)
            {
                return Json(new { success = false, message = "Bạn không có quyền cập nhật tin tuyển dụng này" });
            }

            // Kiểm tra trạng thái hợp lệ
            var trangThaiHopLe = new[] { "Dang tuyen", "Het han", "Da dong" };
            if (string.IsNullOrEmpty(trangThai) || !trangThaiHopLe.Contains(trangThai))
            {
                return Json(new { success = false, message = "Trạng thái không hợp lệ" });
            }

            try
            {
                // Cập nhật trạng thái
                var tinDaCapNhat = _tinTuyenDungRepository.CapNhatTrangThai(id, trangThai);
                
                if (tinDaCapNhat == null)
                {
                    return Json(new { success = false, message = "Không thể cập nhật trạng thái" });
                }

                // Chuyển đổi trạng thái để hiển thị
                var trangThaiDisplay = trangThai switch
                {
                    "Dang tuyen" => "Đang tuyển",
                    "Het han" => "Hết hạn",
                    "Da dong" => "Đã đóng",
                    _ => trangThai
                };

                return Json(new { 
                    success = true, 
                    message = "Cập nhật trạng thái thành công!",
                    trangThai = trangThaiDisplay
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        // GET: Danh sach don ung tuyen
        public async Task<IActionResult> DonUngTuyen(int pageNumber = 1, int pageSize = 10, string? searchTerm = null, string? trangThaiFilter = null, string? tinTuyenDungFilter = null)
        {
            ViewData["Title"] = "Đơn ứng tuyển";
            
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            // Lấy thông tin công ty của user đăng nhập
            var nhaTuyenDung = _nhaTuyenDungRepository.LayNhaTuyenDungTheoUserId(user.Id);
            if (nhaTuyenDung == null || string.IsNullOrEmpty(nhaTuyenDung.TenCongTy))
            {
                // Nếu chưa có thông tin công ty, trả về danh sách rỗng
                var emptyModel = new DonUngTuyenViewModel
                {
                    DonUngTuyens = new List<TinUngTuyen>(),
                    DanhSachTinTuyenDung = new List<TinTuyenDung>(),
                    PageNumber = 1,
                    PageSize = pageSize,
                    TotalCount = 0,
                    SearchTerm = searchTerm,
                    TrangThaiFilter = trangThaiFilter,
                    TinTuyenDungFilter = tinTuyenDungFilter
                };
                return View(emptyModel);
            }

            // Lấy tin tuyển dụng theo mã nhà tuyển dụng (foreign key)
            var danhSachTinTuyenDung = _tinTuyenDungRepository.LayDanhSachTheoMaNhaTuyenDung(nhaTuyenDung.MaNhaTuyenDung);
            
            // Lấy danh sách mã tin tuyển dụng của công ty
            var danhSachMaTin = danhSachTinTuyenDung.Select(t => t.MaTinTuyenDung.ToString()).ToList();
            
            // Lấy đơn ứng tuyển theo các tin tuyển dụng của công ty
            var danhSachDonUngTuyen = _tinUngTuyenRepository.LayDanhSachTinUngTuyen()
                .Where(t => danhSachMaTin.Contains(t.MaTinTuyenDung))
                .ToList();
            
            // Tính stats từ toàn bộ dữ liệu (trước khi lọc)
            var totalDangXemXet = danhSachDonUngTuyen.Count(t => 
                Models.Enums.TrangThaiXuLyHelper.FromString(t.TrangThaiXuLy) == Models.Enums.TrangThaiXuLy.DangXemXet);
            var totalChoPhongVan = danhSachDonUngTuyen.Count(t => 
                Models.Enums.TrangThaiXuLyHelper.FromString(t.TrangThaiXuLy) == Models.Enums.TrangThaiXuLy.ChoPhongVan);
            var totalTuyenDung = danhSachDonUngTuyen.Count(t => 
                Models.Enums.TrangThaiXuLyHelper.FromString(t.TrangThaiXuLy) == Models.Enums.TrangThaiXuLy.TuyenDung);
            
            // Lọc theo tìm kiếm
            if (!string.IsNullOrEmpty(searchTerm))
            {
                danhSachDonUngTuyen = danhSachDonUngTuyen.Where(t => 
                    (t.HoTen != null && t.HoTen.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                    (t.Email != null && t.Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                    (t.SoDienThoai != null && t.SoDienThoai.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                    (t.ViTriUngTuyen != null && t.ViTriUngTuyen.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                ).ToList();
            }
            
            // Lọc theo trạng thái
            if (!string.IsNullOrEmpty(trangThaiFilter))
            {
                danhSachDonUngTuyen = danhSachDonUngTuyen.Where(t =>
                {
                    var trangThaiEnum = Models.Enums.TrangThaiXuLyHelper.FromString(t.TrangThaiXuLy);
                    var trangThaiDisplay = trangThaiEnum.HasValue 
                        ? Models.Enums.TrangThaiXuLyHelper.ToString(trangThaiEnum.Value) 
                        : t.TrangThaiXuLy;
                    return trangThaiDisplay == trangThaiFilter;
                }).ToList();
            }
            
            // Lọc theo tin tuyển dụng
            if (!string.IsNullOrEmpty(tinTuyenDungFilter))
            {
                danhSachDonUngTuyen = danhSachDonUngTuyen.Where(t => t.MaTinTuyenDung == tinTuyenDungFilter).ToList();
            }
            
            // Sắp xếp theo ngày ứng tuyển (mới nhất trước)
            danhSachDonUngTuyen = danhSachDonUngTuyen.OrderByDescending(t => t.NgayUngTuyen).ToList();
            
            // Tính toán phân trang
            var totalCount = danhSachDonUngTuyen.Count;
            var pagedList = danhSachDonUngTuyen.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            
            var model = new DonUngTuyenViewModel
            {
                DonUngTuyens = pagedList,
                DanhSachTinTuyenDung = danhSachTinTuyenDung,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                SearchTerm = searchTerm,
                TrangThaiFilter = trangThaiFilter,
                TinTuyenDungFilter = tinTuyenDungFilter,
                TotalDangXemXet = totalDangXemXet,
                TotalChoPhongVan = totalChoPhongVan,
                TotalTuyenDung = totalTuyenDung
            };
            
            return View(model);
        }

        // GET: Chi tiet don ung tuyen
        [HttpGet]
        public async Task<IActionResult> ChiTietDonUngTuyen(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Json(new { success = false, message = "Không tìm thấy người dùng" });
            }

            // Lấy thông tin công ty của user đăng nhập
            var nhaTuyenDung = _nhaTuyenDungRepository.LayNhaTuyenDungTheoUserId(user.Id);
            if (nhaTuyenDung == null || string.IsNullOrEmpty(nhaTuyenDung.TenCongTy))
            {
                return Json(new { success = false, message = "Không tìm thấy thông tin công ty" });
            }

            // Lấy đơn ứng tuyển từ database
            var donUngTuyen = _tinUngTuyenRepository.LayTinUngTuyenTheoId(id);
            if (donUngTuyen == null)
            {
                return Json(new { success = false, message = "Không tìm thấy đơn ứng tuyển" });
            }

            // Kiểm tra quyền truy cập: đơn ứng tuyển phải thuộc về một tin tuyển dụng của công ty này
            var tinTuyenDung = _tinTuyenDungRepository.LayTinTuyenDungTheoId(
                int.TryParse(donUngTuyen.MaTinTuyenDung, out int maTin) ? maTin : 0);
            
            if (tinTuyenDung == null || tinTuyenDung.MaNhaTuyenDung != nhaTuyenDung.MaNhaTuyenDung)
            {
                return Json(new { success = false, message = "Bạn không có quyền xem đơn ứng tuyển này" });
            }

            // Lấy thông tin tin tuyển dụng liên quan
            var trangThaiEnum = Models.Enums.TrangThaiXuLyHelper.FromString(donUngTuyen.TrangThaiXuLy);
            var trangThaiDisplay = trangThaiEnum.HasValue 
                ? Models.Enums.TrangThaiXuLyHelper.ToString(trangThaiEnum.Value) 
                : donUngTuyen.TrangThaiXuLy;

            return Json(new
            {
                success = true,
                data = new
                {
                    maTinUngTuyen = donUngTuyen.MaTinUngTuyen,
                    userId = donUngTuyen.UserId, // UserId của người ứng tuyển
                    hoTen = donUngTuyen.HoTen,
                    email = donUngTuyen.Email,
                    soDienThoai = donUngTuyen.SoDienThoai,
                    viTriUngTuyen = donUngTuyen.ViTriUngTuyen,
                    congTy = donUngTuyen.CongTy,
                    maTinTuyenDung = donUngTuyen.MaTinTuyenDung,
                    tenViecLam = tinTuyenDung.TenViecLam,
                    trangThaiXuLy = trangThaiDisplay,
                    linkCV = donUngTuyen.LinkCV,
                    ghiChu = donUngTuyen.GhiChu,
                    ngayUngTuyen = donUngTuyen.NgayUngTuyen.ToString("dd/MM/yyyy HH:mm")
                }
            });
        }

        // POST: Cap nhat trang thai don ung tuyen
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CapNhatTrangThaiDonUngTuyen(int maDonUngTuyen, string trangThaiMoi, string? ghiChu = null)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Json(new { success = false, message = "Không tìm thấy người dùng" });
            }

            // Lấy thông tin công ty của user đăng nhập
            var nhaTuyenDung = _nhaTuyenDungRepository.LayNhaTuyenDungTheoUserId(user.Id);
            if (nhaTuyenDung == null || string.IsNullOrEmpty(nhaTuyenDung.TenCongTy))
            {
                return Json(new { success = false, message = "Không tìm thấy thông tin công ty" });
            }

            // Lấy đơn ứng tuyển từ database
            var donUngTuyen = _tinUngTuyenRepository.LayTinUngTuyenTheoId(maDonUngTuyen);
            if (donUngTuyen == null)
            {
                return Json(new { success = false, message = "Không tìm thấy đơn ứng tuyển" });
            }

            // Kiểm tra quyền truy cập: đơn ứng tuyển phải thuộc về một tin tuyển dụng của công ty này
            var tinTuyenDung = _tinTuyenDungRepository.LayTinTuyenDungTheoId(
                int.TryParse(donUngTuyen.MaTinTuyenDung, out int maTin) ? maTin : 0);
            
            if (tinTuyenDung == null || tinTuyenDung.MaNhaTuyenDung != nhaTuyenDung.MaNhaTuyenDung)
            {
                return Json(new { success = false, message = "Bạn không có quyền cập nhật đơn ứng tuyển này" });
            }

            // Kiểm tra trạng thái hợp lệ
            if (string.IsNullOrEmpty(trangThaiMoi))
            {
                return Json(new { success = false, message = "Vui lòng chọn trạng thái mới" });
            }

            try
            {
                // Cập nhật trạng thái
                var donDaCapNhat = _tinUngTuyenRepository.CapNhatTrangThai(maDonUngTuyen, trangThaiMoi, ghiChu);
                
                if (donDaCapNhat == null)
                {
                    return Json(new { success = false, message = "Không thể cập nhật trạng thái" });
                }

                // Gửi email thông báo cho ứng viên
                try
                {
                    var trangThaiEnum = TrangThaiXuLyHelper.FromString(trangThaiMoi);
                    if (trangThaiEnum.HasValue && !string.IsNullOrEmpty(donDaCapNhat.UserId))
                    {
                        // Load User nếu chưa được load
                        ApplicationUser? candidateUser = null;
                        if (donDaCapNhat.User == null)
                        {
                            candidateUser = await _userManager.FindByIdAsync(donDaCapNhat.UserId);
                        }
                        else
                        {
                            candidateUser = donDaCapNhat.User;
                        }

                        if (candidateUser != null && !string.IsNullOrEmpty(candidateUser.Email))
                        {
                            var candidateEmail = candidateUser.Email;
                            var candidateName = donDaCapNhat.HoTen ?? candidateUser.HoTen ?? "Ứng viên";
                            var jobTitle = tinTuyenDung.TenViecLam ?? donDaCapNhat.ViTriUngTuyen ?? "";
                            var companyName = tinTuyenDung.CongTy ?? donDaCapNhat.CongTy ?? "";

                            // Lấy thông tin liên hệ từ tin tuyển dụng
                            var contactPerson = tinTuyenDung.NguoiLienHe;
                            var contactEmail = tinTuyenDung.EmailLienHe;
                            var contactPhone = tinTuyenDung.SDTLienHe;
                            
                            // Tạo địa chỉ đầy đủ
                            var contactAddressParts = new List<string>();
                            if (!string.IsNullOrEmpty(tinTuyenDung.DiaChiLamViec))
                                contactAddressParts.Add(tinTuyenDung.DiaChiLamViec);
                            if (!string.IsNullOrEmpty(tinTuyenDung.PhuongXa))
                                contactAddressParts.Add(tinTuyenDung.PhuongXa);
                            if (!string.IsNullOrEmpty(tinTuyenDung.TinhThanhPho))
                                contactAddressParts.Add(tinTuyenDung.TinhThanhPho);
                            var contactAddress = string.Join(", ", contactAddressParts);

                            _ = Task.Run(async () =>
                            {
                                switch (trangThaiEnum.Value)
                                {
                                    case TrangThaiXuLy.ChoPhongVan:
                                        await _emailService.SendInterviewScheduledNotificationAsync(
                                            candidateEmail, candidateName, jobTitle, companyName,
                                            contactPerson, contactEmail, contactPhone, contactAddress);
                                        break;
                                    case TrangThaiXuLy.TuyenDung:
                                        await _emailService.SendApplicationAcceptedNotificationAsync(
                                            candidateEmail, candidateName, jobTitle, companyName,
                                            contactPerson, contactEmail, contactPhone, contactAddress);
                                        break;
                                    case TrangThaiXuLy.TuChoi:
                                    case TrangThaiXuLy.KhongPhuHop:
                                        await _emailService.SendApplicationRejectedNotificationAsync(
                                            candidateEmail, candidateName, jobTitle, companyName, ghiChu,
                                            contactPerson, contactEmail, contactPhone, contactAddress);
                                        break;
                                    default:
                                        await _emailService.SendApplicationStatusChangedNotificationAsync(
                                            candidateEmail, candidateName, jobTitle, companyName, trangThaiMoi, ghiChu,
                                            contactPerson, contactEmail, contactPhone, contactAddress);
                                        break;
                                }
                            });
                        }
                    }
                }
                catch (Exception emailEx)
                {
                    // Log lỗi nhưng không ảnh hưởng đến kết quả cập nhật
                    var logger = HttpContext.RequestServices.GetRequiredService<ILogger<RecruiterController>>();
                    logger.LogError(emailEx, "Lỗi khi gửi email thông báo cập nhật trạng thái cho ứng viên");
                }

                return Json(new { success = true, message = "Cập nhật trạng thái thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        // GET: Thong tin tai khoan (redirect to CaiDat)
        public IActionResult ThongTinTaiKhoan()
        {
            return RedirectToAction("CaiDat");
        }

        // GET: Thong tin tai khoan user theo userId
        [HttpGet]
        public async Task<IActionResult> ThongTinTaiKhoanUser(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return Json(new { success = false, message = "UserId không hợp lệ" });
            }

            try
            {
                // Lấy thông tin user
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy tài khoản" });
                }

                // Lấy roles của user
                var roles = await _userManager.GetRolesAsync(user);

                // Lấy thông tin UngVien nếu có
                var ungVien = _context.UngViens
                    .Include(u => u.ChuyenNganh)
                    .FirstOrDefault(u => u.UserId == userId);

                // Lấy số lượng đơn ứng tuyển của user
                var soLuongDonUngTuyen = _context.TinUngTuyens
                    .Count(t => t.UserId == userId);

                return Json(new
                {
                    success = true,
                    data = new
                    {
                        userId = user.Id,
                        email = user.Email,
                        phoneNumber = user.PhoneNumber,
                        hoTen = user.HoTen,
                        avatar = user.Avatar,
                        loaiTaiKhoan = user.LoaiTaiKhoan,
                        ngayDangKy = user.NgayDangKy.ToString("dd/MM/yyyy"),
                        emailConfirmed = user.EmailConfirmed,
                        roles = roles.ToList(),
                        // Thông tin UngVien nếu có
                        ungVien = ungVien != null ? new
                        {
                            maUngVien = ungVien.MaUngVien,
                            ngaySinh = ungVien.NgaySinh.ToString("dd/MM/yyyy"),
                            gioiTinh = ungVien.GioiTinh,
                            diaChi = ungVien.DiaChi,
                            nganhNghe = ungVien.NganhNghe,
                            chuyenNganh = ungVien.ChuyenNganh?.TenChuyenNganh,
                            linkCV = ungVien.LinkCV,
                            soLanUngTuyen = ungVien.SoLanUngTuyen,
                            viTriMongMuon = ungVien.ViTriMongMuon,
                            mucLuongKyVong = ungVien.MucLuongKyVong,
                            noiLamViecMongMuon = ungVien.NoiLamViecMongMuon,
                            trangThaiTimViec = ungVien.TrangThaiTimViec
                        } : null,
                        soLuongDonUngTuyen = soLuongDonUngTuyen
                    }
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        // GET: Thong ke
        public async Task<IActionResult> ThongKe()
        {
            ViewData["Title"] = "Thống kê";
            
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            // Lấy thông tin công ty của user đăng nhập
            var nhaTuyenDung = _nhaTuyenDungRepository.LayNhaTuyenDungTheoUserId(user.Id);
            if (nhaTuyenDung == null || string.IsNullOrEmpty(nhaTuyenDung.TenCongTy))
            {
                // Nếu chưa có thông tin công ty, trả về dữ liệu rỗng
                ViewBag.TongTinDaDang = 0;
                ViewBag.TongDonUngTuyen = 0;
                ViewBag.TongLuotXem = 0;
                ViewBag.TyLePhuHop = 0;
                ViewBag.ThongKeNganhNghe = new List<object>();
                ViewBag.ThongKeTrangThai = new List<object>();
                ViewBag.TopTinHieuQua = new List<object>();
                return View();
            }

            // Lấy tin tuyển dụng theo mã nhà tuyển dụng (foreign key)
            var danhSachTinTuyenDung = _tinTuyenDungRepository.LayDanhSachTheoMaNhaTuyenDung(nhaTuyenDung.MaNhaTuyenDung);
            
            // Lấy danh sách mã tin tuyển dụng của công ty
            var danhSachMaTin = danhSachTinTuyenDung.Select(t => t.MaTinTuyenDung.ToString()).ToList();
            
            // Lấy đơn ứng tuyển theo các tin tuyển dụng của công ty
            var danhSachTinUngTuyen = _tinUngTuyenRepository.LayDanhSachTinUngTuyen()
                .Where(t => danhSachMaTin.Contains(t.MaTinTuyenDung))
                .ToList();
            
            // Thống kê tổng quan
            var tongTinDaDang = danhSachTinTuyenDung.Count;
            var tongDonUngTuyen = danhSachTinUngTuyen.Count;
            var tongLuotXem = danhSachTinTuyenDung.Sum(t => t.SoLuongUngTuyen * 10);
            // Tính tỷ lệ phù hợp: chỉ đếm các đơn có trạng thái "Tuyển dụng"
            var tyLePhuHop = tongDonUngTuyen > 0 
                ? (danhSachTinUngTuyen.Count(t => 
                    {
                        var trangThai = TrangThaiXuLyHelper.FromString(t.TrangThaiXuLy);
                        return trangThai.HasValue && trangThai.Value == TrangThaiXuLy.TuyenDung;
                    }) * 100.0 / tongDonUngTuyen) 
                : 0;
            
            // Thống kê theo ngành nghề - Top ngành nghề được ứng tuyển nhiều nhất
            var thongKeNganhNghe = danhSachTinUngTuyen
                .Join(danhSachTinTuyenDung,
                    ungTuyen => ungTuyen.MaTinTuyenDung,
                    tinTuyenDung => tinTuyenDung.MaTinTuyenDung.ToString(),
                    (ungTuyen, tinTuyenDung) => new { NganhNghe = tinTuyenDung.NganhNghe ?? "Khác" })
                .GroupBy(x => x.NganhNghe)
                .Select(g => new { NganhNghe = g.Key, SoLuong = g.Count() })
                .OrderByDescending(x => x.SoLuong)
                .Take(5)
                .ToList();
            
            // Thống kê trạng thái đơn ứng tuyển
            var thongKeTrangThai = danhSachTinUngTuyen
                .GroupBy(t => t.TrangThaiXuLy ?? "Chưa xử lý")
                .Select(g => new { TrangThai = g.Key, SoLuong = g.Count() })
                .ToList();
            
            // Top 5 tin tuyển dụng hiệu quả nhất
            var topTinHieuQua = danhSachTinTuyenDung
                .Select(t => new
                {
                    TenTin = t.TenViecLam,
                    SoLuongUngTuyen = danhSachTinUngTuyen.Count(u => u.MaTinTuyenDung == t.MaTinTuyenDung.ToString()),
                    SoLuotXem = (t.SoLuongUngTuyen ?? 0) * 10
                })
                .OrderByDescending(x => x.SoLuongUngTuyen)
                .Take(5)
                .ToList();
            
            ViewBag.TongTinDaDang = tongTinDaDang;
            ViewBag.TongDonUngTuyen = tongDonUngTuyen;
            ViewBag.TongLuotXem = tongLuotXem;
            ViewBag.TyLePhuHop = Math.Round(tyLePhuHop, 1);
            ViewBag.ThongKeNganhNghe = thongKeNganhNghe;
            ViewBag.ThongKeTrangThai = thongKeTrangThai;
            ViewBag.TopTinHieuQua = topTinHieuQua;
            
            return View();
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
            
            // Lấy thông tin NhaTuyenDung từ database
            var nhaTuyenDung = await _context.NhaTuyenDungs
                .FirstOrDefaultAsync(n => n.UserId == user.Id);
            
            // Lấy danh sách tỉnh/thành phố
            var danhSachTinhThanh = _context.Provinces
                .OrderBy(p => p.FullName)
                .ToList();
            
            ViewBag.DanhSachTinhThanh = danhSachTinhThanh;
            ViewBag.NhaTuyenDung = nhaTuyenDung; // Truyền dữ liệu vào ViewBag
            return View(user);
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
                return RedirectToAction("CaiDat", new { area = "Recruiter" });
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
            Console.WriteLine("\n==========================================================");
            Console.WriteLine("🔥 BACKEND: UploadFileAsync CALLED");
            Console.WriteLine($"Folder: {folder}");
            Console.WriteLine($"User ID: {userId}");
            
            if (file == null || file.Length == 0)
            {
                Console.WriteLine("❌ BACKEND: File is NULL or EMPTY!");
                Console.WriteLine("==========================================================\n");
                return null;
            }

            Console.WriteLine($"✅ BACKEND: File received!");
            Console.WriteLine($"  File name: {file.FileName}");
            Console.WriteLine($"  File size: {file.Length} bytes");
            Console.WriteLine($"  Content type: {file.ContentType}");

            // Kiểm tra định dạng file (cho phép JPG, PNG, GIF)
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            
            Console.WriteLine($"  File extension: {fileExtension}");
            Console.WriteLine($"  Is GIF? {fileExtension == ".gif"}");
            Console.WriteLine($"  Content-Type is GIF? {file.ContentType == "image/gif"}");
            
            if (!allowedExtensions.Contains(fileExtension))
            {
                Console.WriteLine($"Invalid file extension!");
                throw new Exception("Định dạng file không hợp lệ. Chỉ chấp nhận JPG, PNG, GIF.");
            }

            // Kiểm tra kích thước file (tối đa 5MB)
            var maxSize = 5 * 1024 * 1024; // 5MB
            if (file.Length > maxSize)
            {
                Console.WriteLine($"File too large!");
                throw new Exception("Kích thước file quá lớn. Tối đa 5MB.");
            }

            // Tạo tên file unique
            var fileName = $"{userId}_{DateTime.Now:yyyyMMddHHmmss}_{Guid.NewGuid():N}{fileExtension}";
            var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", folder);
            
            Console.WriteLine($"Generated file name: {fileName}");
            Console.WriteLine($"Upload path: {uploadsPath}");
            
            // Đảm bảo thư mục tồn tại
            if (!Directory.Exists(uploadsPath))
            {
                Console.WriteLine($"Creating directory: {uploadsPath}");
                Directory.CreateDirectory(uploadsPath);
            }

            var filePath = Path.Combine(uploadsPath, fileName);
            Console.WriteLine($"Full file path: {filePath}");
            
            // Lưu file
            try
            {
                Console.WriteLine($"💾 BACKEND: Saving file to disk...");
                Console.WriteLine($"  Path: {filePath}");
                
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                
                Console.WriteLine($"✅ BACKEND: File saved successfully!");
                
                // Kiểm tra file đã lưu
                var savedFileInfo = new FileInfo(filePath);
                Console.WriteLine($"📊 BACKEND: Saved file info:");
                Console.WriteLine($"  Exists: {savedFileInfo.Exists}");
                Console.WriteLine($"  Size: {savedFileInfo.Length} bytes");
                Console.WriteLine($"  Extension: {savedFileInfo.Extension}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ BACKEND: Error saving file: {ex.Message}");
                Console.WriteLine("==========================================================\n");
                throw;
            }

            // Trả về đường dẫn URL
            var urlPath = $"/uploads/{folder}/{fileName}";
            Console.WriteLine($"🔗 BACKEND: Returning URL: {urlPath}");
            Console.WriteLine("==========================================================\n");
            return urlPath;
        }

        // POST: Cap nhat thong tin nha tuyen dung
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CapNhatThongTin(
            string tenCongTy, string email, string soDienThoai, string? website,
            string? tinhThanhPho, string? quanHuyen, string? diaChi,
            string? nguoiDaiDien, string? chucVu, string? soDienThoaiNguoiDaiDien, string? emailNguoiDaiDien,
            IFormFile? logoFile, string? moTa, string? latitude, string? longitude, string? xoaLogo)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Json(new { success = false, message = "Không tìm thấy người dùng" });
            }

            try
            {
                // Debug logging
                Console.WriteLine($"=== CapNhatThongTin Debug ===");
                Console.WriteLine($"User ID: {user.Id}");
                Console.WriteLine($"LogoFile received: {logoFile != null}");
                if (logoFile != null)
                {
                    Console.WriteLine($"LogoFile name: {logoFile.FileName}");
                    Console.WriteLine($"LogoFile size: {logoFile.Length} bytes");
                    Console.WriteLine($"LogoFile content type: {logoFile.ContentType}");
                }
                
                // Xử lý upload logo nếu có
                string? logoPath = null;
                if (logoFile != null && logoFile.Length > 0)
                {
                    Console.WriteLine($"Attempting to upload logo...");
                    logoPath = await UploadFileAsync(logoFile, "logos", user.Id);
                    Console.WriteLine($"Logo uploaded to: {logoPath}");
                }
                else
                {
                    Console.WriteLine($"No logo file to upload");
                }

                // Tìm hoặc tạo bản ghi NhaTuyenDung
                var nhaTuyenDung = await _context.NhaTuyenDungs
                    .FirstOrDefaultAsync(n => n.UserId == user.Id);

                if (nhaTuyenDung == null)
                {
                    // Tạo mới nếu chưa có
                    nhaTuyenDung = new NhaTuyenDung
                    {
                        UserId = user.Id,
                        TenCongTy = tenCongTy,
                        Email = email,
                        SoDienThoai = soDienThoai,
                        Website = website,
                        TinhThanhPho = tinhThanhPho,
                        QuanHuyen = quanHuyen,
                        DiaChi = diaChi,
                        NguoiDaiDien = nguoiDaiDien,
                        ChucVu = chucVu,
                        SoDienThoaiNguoiDaiDien = soDienThoaiNguoiDaiDien,
                        EmailNguoiDaiDien = emailNguoiDaiDien,
                        Logo = logoPath,
                        MoTa = moTa,
                        Latitude = !string.IsNullOrEmpty(latitude) ? double.Parse(latitude) : null,
                        Longitude = !string.IsNullOrEmpty(longitude) ? double.Parse(longitude) : null,
                        NgayDangKy = DateTime.Now,
                        SoTinDaDang = 0,
                        SoUngVienNhan = 0
                    };
                    _context.NhaTuyenDungs.Add(nhaTuyenDung);
                }
                else
                {
                    // Cập nhật thông tin
                    nhaTuyenDung.TenCongTy = tenCongTy;
                    nhaTuyenDung.Email = email;
                    nhaTuyenDung.SoDienThoai = soDienThoai;
                    nhaTuyenDung.Website = website;
                    nhaTuyenDung.TinhThanhPho = tinhThanhPho;
                    nhaTuyenDung.QuanHuyen = quanHuyen;
                    nhaTuyenDung.DiaChi = diaChi;
                    nhaTuyenDung.NguoiDaiDien = nguoiDaiDien;
                    nhaTuyenDung.ChucVu = chucVu;
                    nhaTuyenDung.SoDienThoaiNguoiDaiDien = soDienThoaiNguoiDaiDien;
                    nhaTuyenDung.EmailNguoiDaiDien = emailNguoiDaiDien;
                    
                    // Xử lý logo: xóa hoặc cập nhật
                    if (xoaLogo == "true")
                    {
                        // Xóa logo cũ nếu có
                        if (!string.IsNullOrEmpty(nhaTuyenDung.Logo) && nhaTuyenDung.Logo.StartsWith("/uploads/"))
                        {
                            var oldLogoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", nhaTuyenDung.Logo.TrimStart('/'));
                            if (System.IO.File.Exists(oldLogoPath))
                            {
                                System.IO.File.Delete(oldLogoPath);
                            }
                        }
                        nhaTuyenDung.Logo = null;
                    }
                    else if (logoPath != null)
                    {
                        // Cập nhật logo mới
                        // Xóa logo cũ nếu có
                        if (!string.IsNullOrEmpty(nhaTuyenDung.Logo) && nhaTuyenDung.Logo.StartsWith("/uploads/"))
                        {
                            var oldLogoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", nhaTuyenDung.Logo.TrimStart('/'));
                            if (System.IO.File.Exists(oldLogoPath))
                            {
                                System.IO.File.Delete(oldLogoPath);
                            }
                        }
                        nhaTuyenDung.Logo = logoPath;
                    }
                    // Nếu không có logoFile mới và không xóa logo, giữ nguyên logo cũ
                    
                    nhaTuyenDung.MoTa = moTa;
                    nhaTuyenDung.Latitude = !string.IsNullOrEmpty(latitude) ? double.Parse(latitude) : null;
                    nhaTuyenDung.Longitude = !string.IsNullOrEmpty(longitude) ? double.Parse(longitude) : null;
                    _context.NhaTuyenDungs.Update(nhaTuyenDung);
                }

                // Cập nhật thông tin user
                // Với nhà tuyển dụng, HoTen là tên công ty, không phải tên người đại diện
                user.Email = email;
                user.PhoneNumber = soDienThoai;
                // Cập nhật HoTen thành tên công ty (không phải tên người đại diện)
                user.HoTen = tenCongTy;
                await _userManager.UpdateAsync(user);

                // Lưu vào database
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Cập nhật thông tin thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
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

        // GET: Lấy thông báo cho nhà tuyển dụng
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

                // Lấy thông tin công ty của user đăng nhập
                var nhaTuyenDung = _nhaTuyenDungRepository.LayNhaTuyenDungTheoUserId(user.Id);
                if (nhaTuyenDung == null)
                {
                    return Json(new { success = true, data = new NotificationViewModel { TotalCount = 0, Notifications = new List<NotificationItem>() } });
                }

                var notifications = new List<NotificationItem>();

                // Lấy tin tuyển dụng theo mã nhà tuyển dụng
                var danhSachTinTuyenDung = _tinTuyenDungRepository.LayDanhSachTheoMaNhaTuyenDung(nhaTuyenDung.MaNhaTuyenDung);
                var danhSachMaTin = danhSachTinTuyenDung.Select(t => t.MaTinTuyenDung.ToString()).ToList();

                // Lấy đơn ứng tuyển theo các tin tuyển dụng của công ty
                var danhSachTinUngTuyen = _tinUngTuyenRepository.LayDanhSachTinUngTuyen()
                    .Where(t => danhSachMaTin.Contains(t.MaTinTuyenDung))
                    .ToList();

                var now = DateTime.Now;
                var threeDaysFromNow = now.AddDays(3);

                // 1. Đơn ứng tuyển mới (trong 7 ngày gần đây, chưa xử lý)
                var donUngTuyenMoi = danhSachTinUngTuyen
                    .Where(t => t.NgayUngTuyen >= now.AddDays(-7) && 
                                (string.IsNullOrEmpty(t.TrangThaiXuLy) || 
                                 TrangThaiXuLyHelper.FromString(t.TrangThaiXuLy) == TrangThaiXuLy.DangXemXet))
                    .OrderByDescending(t => t.NgayUngTuyen)
                    .Take(10)
                    .ToList();

                foreach (var don in donUngTuyenMoi)
                {
                    var tinTuyenDung = danhSachTinTuyenDung.FirstOrDefault(t => t.MaTinTuyenDung.ToString() == don.MaTinTuyenDung);
                    notifications.Add(new NotificationItem
                    {
                        Type = "NewApplication",
                        Title = "Đơn ứng tuyển mới",
                        Message = $"{don.HoTen} đã ứng tuyển vào vị trí \"{tinTuyenDung?.TenViecLam ?? don.ViTriUngTuyen}\"",
                        Icon = "bi-person-plus-fill",
                        Color = "text-primary",
                        Url = Url.Action("DonUngTuyen", "Recruiter", new { area = "Recruiter" }),
                        CreatedAt = don.NgayUngTuyen,
                        RelatedId = don.MaTinUngTuyen,
                        NotificationKey = $"NewApplication_{don.MaTinUngTuyen}_{don.NgayUngTuyen:yyyyMMddHHmmss}"
                    });
                }

                // 2. Tin tuyển dụng sắp hết hạn (trong 3 ngày tới)
                var tinSapHetHan = danhSachTinTuyenDung
                    .Where(t => t.HanNop >= now && t.HanNop <= threeDaysFromNow && 
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
                        Url = Url.Action("ChiTietTinDaDang", "Recruiter", new { area = "Recruiter", id = tin.MaTinTuyenDung }),
                        CreatedAt = tin.HanNop,
                        RelatedId = tin.MaTinTuyenDung,
                        NotificationKey = $"ExpiringSoon_{tin.MaTinTuyenDung}_{tin.HanNop:yyyyMMdd}"
                    });
                }

                // 3. Tin tuyển dụng đã hết hạn (chưa đóng)
                var tinHetHan = danhSachTinTuyenDung
                    .Where(t => t.HanNop < now && 
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
                        Message = $"Tin \"{tin.TenViecLam}\" đã hết hạn từ ngày {tin.HanNop:dd/MM/yyyy}. Vui lòng cập nhật trạng thái.",
                        Icon = "bi-exclamation-triangle-fill",
                        Color = "text-danger",
                        Url = Url.Action("ChiTietTinDaDang", "Recruiter", new { area = "Recruiter", id = tin.MaTinTuyenDung }),
                        CreatedAt = tin.HanNop,
                        RelatedId = tin.MaTinTuyenDung,
                        NotificationKey = $"Expired_{tin.MaTinTuyenDung}_{tin.HanNop:yyyyMMdd}"
                    });
                }

                // 4. Đơn ứng tuyển chờ xem xét (chưa xử lý trong 3 ngày)
                var donChoXemXet = danhSachTinUngTuyen
                    .Where(t => (string.IsNullOrEmpty(t.TrangThaiXuLy) || 
                                 TrangThaiXuLyHelper.FromString(t.TrangThaiXuLy) == TrangThaiXuLy.DangXemXet) &&
                                t.NgayUngTuyen < now.AddDays(-3))
                    .OrderBy(t => t.NgayUngTuyen)
                    .Take(5)
                    .ToList();

                foreach (var don in donChoXemXet)
                {
                    var tinTuyenDung = danhSachTinTuyenDung.FirstOrDefault(t => t.MaTinTuyenDung.ToString() == don.MaTinTuyenDung);
                    var daysWaiting = (now.Date - don.NgayUngTuyen.Date).Days;
                    notifications.Add(new NotificationItem
                    {
                        Type = "PendingReview",
                        Title = "Đơn ứng tuyển chờ xem xét",
                        Message = $"Đơn ứng tuyển của {don.HoTen} cho vị trí \"{tinTuyenDung?.TenViecLam ?? don.ViTriUngTuyen}\" đã chờ {daysWaiting} ngày",
                        Icon = "bi-hourglass-split",
                        Color = "text-info",
                        Url = Url.Action("DonUngTuyen", "Recruiter", new { area = "Recruiter" }),
                        CreatedAt = don.NgayUngTuyen,
                        RelatedId = don.MaTinUngTuyen,
                        NotificationKey = $"PendingReview_{don.MaTinUngTuyen}_{don.NgayUngTuyen:yyyyMMddHHmmss}"
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

