using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Unicareer.Models;
using Unicareer.Models.Enums;
using Unicareer.Repository;
using Unicareer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Unicareer.Services;
using AutoMapper;

namespace Unicareer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INhaTuyenDungRepository _nhaTuyenDungRepository;
        private readonly ITinTuyenDungRepository _tinTuyenDungRepository;
        private readonly IUngVienRepository _ungVienRepository;
        private readonly ITruongDaiHocRepository _truongDaiHocRepository;
        private readonly INganhNgheRepository _nganhNgheRepository;
        private readonly IBlogRepository _blogRepository;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;

        public HomeController(ILogger<HomeController> logger, INhaTuyenDungRepository nhaTuyenDungRepository, ITinTuyenDungRepository tinTuyenDungRepository, IUngVienRepository ungVienRepository, ITruongDaiHocRepository truongDaiHocRepository, INganhNgheRepository nganhNgheRepository, IBlogRepository blogRepository, ApplicationDbContext context, UserManager<ApplicationUser> userManager, IEmailService emailService, IMapper mapper)
        {
            _logger = logger;
            _nhaTuyenDungRepository = nhaTuyenDungRepository;
            _tinTuyenDungRepository = tinTuyenDungRepository;
            _ungVienRepository = ungVienRepository;
            _truongDaiHocRepository = truongDaiHocRepository;
            _nganhNgheRepository = nganhNgheRepository;
            _blogRepository = blogRepository;
            _context = context;
            _userManager = userManager;
            _emailService = emailService;
            _mapper = mapper;
        }

        // Helper method để lấy logo theo tên công ty
        private Dictionary<string, string?> GetCompanyLogos(List<string> companyNames)
        {
            var logos = _context.NhaTuyenDungs
                .Where(n => companyNames.Contains(n.TenCongTy))
                .ToDictionary(n => n.TenCongTy, n => n.Logo);
            
            return logos;
        }

        public async Task<IActionResult> Index()
        {
            // Lấy danh sách việc làm nổi bật (top 9)
            var query = _tinTuyenDungRepository.LayDanhSachTinTuyenDung(onlyApproved: true);
            
            // Kiểm tra nếu user đã đăng nhập và là ứng viên có NoiLamViecMongMuon
            string? noiLamViecMongMuon = null;
            if (User.Identity?.IsAuthenticated == true && User.IsInRole(SD.Role_UngVien))
            {
                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser != null)
                {
                    var ungVien = _ungVienRepository.LayDanhSachUngVien()
                        .FirstOrDefault(u => u.UserId == currentUser.Id);
                    
                    if (ungVien != null && !string.IsNullOrEmpty(ungVien.NoiLamViecMongMuon))
                    {
                        noiLamViecMongMuon = ungVien.NoiLamViecMongMuon;
                    }
                }
            }
            
            // Hàm helper để kiểm tra xem tên tỉnh/thành phố có khớp không (linh hoạt)
            Func<string, string, bool> isMatch = (provinceFullName, jobCityName) =>
            {
                if (string.IsNullOrEmpty(provinceFullName) || string.IsNullOrEmpty(jobCityName))
                    return false;
                
                // So sánh chính xác
                if (provinceFullName.Equals(jobCityName, StringComparison.OrdinalIgnoreCase))
                    return true;
                
                // Loại bỏ "Thành phố" và "Tỉnh" để so sánh
                var provinceNameOnly = provinceFullName
                    .Replace("Thành phố ", "", StringComparison.OrdinalIgnoreCase)
                    .Replace("Tỉnh ", "", StringComparison.OrdinalIgnoreCase)
                    .Trim();
                
                var jobCityNameOnly = jobCityName
                    .Replace("Thành phố ", "", StringComparison.OrdinalIgnoreCase)
                    .Replace("Tỉnh ", "", StringComparison.OrdinalIgnoreCase)
                    .Trim();
                
                // So sánh sau khi loại bỏ tiền tố
                if (provinceNameOnly.Equals(jobCityNameOnly, StringComparison.OrdinalIgnoreCase))
                    return true;
                
                // Kiểm tra chứa (một phần)
                if (provinceFullName.Contains(jobCityName, StringComparison.OrdinalIgnoreCase) ||
                    jobCityName.Contains(provinceFullName, StringComparison.OrdinalIgnoreCase) ||
                    provinceNameOnly.Contains(jobCityNameOnly, StringComparison.OrdinalIgnoreCase) ||
                    jobCityNameOnly.Contains(provinceNameOnly, StringComparison.OrdinalIgnoreCase))
                    return true;
                
                return false;
            };
            
            // Nếu có NoiLamViecMongMuon, ưu tiên hiển thị việc làm ở tỉnh thành đó
            List<TinTuyenDung> danhSachViecLam;
            if (!string.IsNullOrEmpty(noiLamViecMongMuon))
            {
                // Lấy tất cả việc làm và phân loại
                var tatCaViecLam = query.ToList();
                
                // Tách thành 2 nhóm: việc làm ở tỉnh thành mong muốn và các việc làm khác
                var viecLamTheoTinhMongMuon = tatCaViecLam
                    .Where(j => !string.IsNullOrEmpty(j.TinhThanhPho) && 
                           isMatch(noiLamViecMongMuon, j.TinhThanhPho))
                    .OrderByDescending(j => j.SoLuongUngTuyen ?? 0)
                    .ThenByDescending(j => j.NgayDang)
                    .ToList();
                
                var viecLamKhac = tatCaViecLam
                    .Where(j => string.IsNullOrEmpty(j.TinhThanhPho) || 
                           !isMatch(noiLamViecMongMuon, j.TinhThanhPho))
                    .OrderByDescending(j => j.SoLuongUngTuyen ?? 0)
                    .ThenByDescending(j => j.NgayDang)
                    .ToList();
                
                // Ưu tiên hiển thị việc làm ở tỉnh thành mong muốn trước, sau đó là các việc làm khác
                danhSachViecLam = viecLamTheoTinhMongMuon
                    .Take(9)
                    .Concat(viecLamKhac.Take(9 - viecLamTheoTinhMongMuon.Count))
                    .Take(9)
                    .ToList();
            }
            else
            {
                // Logic hiện tại: sắp xếp theo số lượng ứng tuyển và ngày đăng
                danhSachViecLam = query
                    .OrderByDescending(j => j.SoLuongUngTuyen ?? 0)
                    .ThenByDescending(j => j.NgayDang)
                    .Take(9)
                    .ToList();
            }
            
            // Lấy danh sách tỉnh/thành phố
            var danhSachTinhThanh = _context.Provinces
                .OrderBy(p => p.FullName)
                .ToList();
            
            // Lấy tất cả tin tuyển dụng để phân tích
            var tatCaTinTuyenDung = _tinTuyenDungRepository.LayDanhSachTinTuyenDung(onlyApproved: true);
            
            // Lấy danh sách 10 tỉnh thành ưu tiên cho phần "Bạn Muốn Làm Việc Ở Đâu?"
            // Đếm số lượng việc làm theo tỉnh/thành phố
            var soLuongViecLamTheoTinh = tatCaTinTuyenDung
                .Where(t => !string.IsNullOrEmpty(t.TinhThanhPho))
                .GroupBy(t => t.TinhThanhPho)
                .Select(g => new { TinhThanhPho = g.Key, SoLuong = g.Count() })
                .ToList();
            
            // Sử dụng lại hàm isMatch đã được định nghĩa ở trên
            
            // Lấy danh sách tỉnh thành với thông tin số lượng việc làm
            var danhSachTinhThanhWithCount = danhSachTinhThanh
                .Select(p => new
                {
                    Province = p,
                    SoLuongViecLam = soLuongViecLamTheoTinh
                        .Where(x => isMatch(p.FullName, x.TinhThanhPho))
                        .Sum(x => x.SoLuong),
                    IsThanhPho = p.AdministrativeUnitId == 1
                })
                .ToList();
            
            // Kiểm tra xem có tỉnh thành nào có tin tuyển dụng không
            var coTinhThanhCoTin = danhSachTinhThanhWithCount.Any(x => x.SoLuongViecLam > 0);
            
            List<Province> danhSachTinhThanhUuTien;
            
            if (!coTinhThanhCoTin)
            {
                // Nếu không có tỉnh thành nào có tin → ưu tiên hiển thị 10 thành phố
                danhSachTinhThanhUuTien = danhSachTinhThanhWithCount
                    .Where(x => x.IsThanhPho)
                    .OrderBy(x => x.Province.FullName)
                    .Take(10)
                    .Select(x => x.Province)
                    .ToList();
            }
            else
            {
                // Nếu có tỉnh thành có tin → sắp xếp theo logic:
                // 1. Các tỉnh có tin → sắp xếp giảm dần theo số lượng
                // 2. Các tỉnh không có tin → ưu tiên thành phố, sau đó là tỉnh
                danhSachTinhThanhUuTien = danhSachTinhThanhWithCount
                    .OrderByDescending(x => x.SoLuongViecLam > 0) // Ưu tiên tỉnh có tin
                    .ThenByDescending(x => x.SoLuongViecLam) // Trong các tỉnh có tin: sắp xếp giảm dần
                    .ThenByDescending(x => x.IsThanhPho) // Trong các tỉnh không có tin: ưu tiên thành phố
                    .Take(10)
                    .Select(x => x.Province)
                    .ToList();
            }
            
            ViewBag.DanhSachTinhThanhUuTien = danhSachTinhThanhUuTien;
            
            // Lấy logo cho các công ty
            var companyNames = danhSachViecLam.Select(j => j.CongTy).Distinct().ToList();
            var companyLogos = GetCompanyLogos(companyNames);
            
            // Lấy danh sách ngành nghề từ database
            var danhSachNganhNghe = _context.NganhNghes
                .OrderBy(n => n.TenNganhNghe)
                .ToList();
            
            // Lấy danh sách loại công việc từ database
            var danhSachLoaiCongViec = _context.LoaiCongViecs
                .OrderBy(l => l.TenLoaiCongViec)
                .ToList();
            
            // Lấy danh sách cấp bậc (vị trí) từ DropdownOptions
            var danhSachCapBac = DropdownOptions.ViTri;
            
            // Lấy danh sách kinh nghiệm từ DropdownOptions
            var danhSachKinhNghiem = DropdownOptions.KinhNghiem;
            
            // Tạo danh sách gợi ý tìm kiếm từ dữ liệu thực tế
            var danhSachGoiYTimKiem = new List<GoiYTimKiem>();
            
            // Top ngành nghề có nhiều việc làm nhất (lấy top 3)
            var topNganhNghe = tatCaTinTuyenDung
                .Where(t => !string.IsNullOrEmpty(t.NganhNghe))
                .GroupBy(t => t.NganhNghe)
                .Select(g => new { Ten = g.Key, SoLuong = g.Count() })
                .OrderByDescending(x => x.SoLuong)
                .Take(3)
                .ToList();
            
            foreach (var nganhNghe in topNganhNghe)
            {
                danhSachGoiYTimKiem.Add(new GoiYTimKiem 
                { 
                    Text = $"Việc làm {nganhNghe.Ten}", 
                    Url = $"/Home/TimViec?nganhNghe={Uri.EscapeDataString(nganhNghe.Ten)}" 
                });
            }
            
            // Top loại công việc phổ biến (lấy top 2)
            var topLoaiCongViec = tatCaTinTuyenDung
                .Where(t => !string.IsNullOrEmpty(t.LoaiCongViec))
                .GroupBy(t => t.LoaiCongViec)
                .Select(g => new { Ten = g.Key, SoLuong = g.Count() })
                .OrderByDescending(x => x.SoLuong)
                .Take(2)
                .ToList();
            
            foreach (var loaiCongViec in topLoaiCongViec)
            {
                danhSachGoiYTimKiem.Add(new GoiYTimKiem 
                { 
                    Text = $"Việc làm {loaiCongViec.Ten}", 
                    Url = $"/Home/TimViec?loaiCongViec={Uri.EscapeDataString(loaiCongViec.Ten)}" 
                });
            }
            
            // Top tỉnh/thành phố có nhiều việc làm nhất (lấy top 1)
            var topTinhThanh = tatCaTinTuyenDung
                .Where(t => !string.IsNullOrEmpty(t.TinhThanhPho))
                .GroupBy(t => t.TinhThanhPho)
                .Select(g => new { Ten = g.Key, SoLuong = g.Count() })
                .OrderByDescending(x => x.SoLuong)
                .Take(1)
                .ToList();
            
            foreach (var tinhThanh in topTinhThanh)
            {
                danhSachGoiYTimKiem.Add(new GoiYTimKiem 
                { 
                    Text = $"Việc làm tại {tinhThanh.Ten}", 
                    Url = $"/Home/TimViec?tinhThanh={Uri.EscapeDataString(tinhThanh.Ten)}" 
                });
            }
            
            // Thêm gợi ý cố định
            danhSachGoiYTimKiem.Add(new GoiYTimKiem { Text = "Việc làm thực tập", Url = "/Home/ThucTap" });
            danhSachGoiYTimKiem.Add(new GoiYTimKiem { Text = "Việc làm mới nhất", Url = "/Home/TimViec?sort=mới nhất" });
            
            ViewBag.DanhSachViecLamNoiBat = danhSachViecLam;
            ViewBag.DanhSachTinhThanh = danhSachTinhThanh;
            ViewBag.CompanyLogos = companyLogos;
            ViewBag.DanhSachNganhNghe = danhSachNganhNghe;
            ViewBag.DanhSachLoaiCongViec = danhSachLoaiCongViec;
            ViewBag.DanhSachCapBac = danhSachCapBac;
            ViewBag.DanhSachKinhNghiem = danhSachKinhNghiem;
            ViewBag.DanhSachGoiYTimKiem = danhSachGoiYTimKiem;
            
            // Lấy danh sách ứng viên công khai (chỉ hiển thị cho nhà tuyển dụng đã đăng nhập)
            var danhSachUngVienCongKhai = _ungVienRepository.LayDanhSachUngVien()
                .Where(u => u.HienThiCongKhai && !string.IsNullOrEmpty(u.HoTen))
                .OrderByDescending(u => u.NgayDangKy)
                .Take(6)
                .ToList();
            
            ViewBag.DanhSachUngVienCongKhai = danhSachUngVienCongKhai;
            
            // Lấy danh sách blog đã duyệt và hiển thị (tối đa 6 bài)
            var danhSachBlog = _blogRepository.LayDanhSachBlogHienThi()
                .Take(6)
                .ToList();
            
            ViewBag.DanhSachBlog = danhSachBlog;
            
            return View();
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

        public IActionResult TimViec(string keyword, string tinhThanh, string quanHuyen, string nganhNghe, string loaiCongViec, string capBac, string kinhNghiem, string mucLuong, string sort)
        {
            // Query trực tiếp từ database để tối ưu hiệu suất
            var danhSachTinTuyenDung = _context.TinTuyenDungs
                .Where(t => t.TrangThaiDuyet == "Da duyet")
                .AsQueryable();
            
            // Tìm kiếm theo từ khóa (keyword) - tìm trong TuKhoa, TenViecLam, CongTy, MoTa, YeuCau, KyNang
            if (!string.IsNullOrEmpty(keyword))
            {
                var keywordTrimmed = keyword.Trim();
                danhSachTinTuyenDung = danhSachTinTuyenDung.Where(t =>
                    (!string.IsNullOrEmpty(t.TuKhoa) && t.TuKhoa.Contains(keywordTrimmed)) ||
                    (!string.IsNullOrEmpty(t.TenViecLam) && t.TenViecLam.Contains(keywordTrimmed)) ||
                    (!string.IsNullOrEmpty(t.CongTy) && t.CongTy.Contains(keywordTrimmed)) ||
                    (!string.IsNullOrEmpty(t.MoTa) && t.MoTa.Contains(keywordTrimmed)) ||
                    (!string.IsNullOrEmpty(t.YeuCau) && t.YeuCau.Contains(keywordTrimmed)) ||
                    (!string.IsNullOrEmpty(t.KyNang) && t.KyNang.Contains(keywordTrimmed)) ||
                    (!string.IsNullOrEmpty(t.NganhNghe) && t.NganhNghe.Contains(keywordTrimmed)) ||
                    (!string.IsNullOrEmpty(t.NganhNgheChiTiet) && t.NganhNgheChiTiet.Contains(keywordTrimmed))
                );
            }
            
            // Lọc theo tỉnh/thành phố
            if (!string.IsNullOrEmpty(tinhThanh))
            {
                danhSachTinTuyenDung = danhSachTinTuyenDung.Where(t => 
                    !string.IsNullOrEmpty(t.TinhThanhPho) && t.TinhThanhPho.Contains(tinhThanh));
            }
            
            // Lọc theo quận/huyện
            if (!string.IsNullOrEmpty(quanHuyen))
            {
                danhSachTinTuyenDung = danhSachTinTuyenDung.Where(t => 
                    !string.IsNullOrEmpty(t.PhuongXa) && t.PhuongXa.Contains(quanHuyen));
            }
            
            // Lọc theo ngành nghề
            if (!string.IsNullOrEmpty(nganhNghe))
            {
                danhSachTinTuyenDung = danhSachTinTuyenDung.Where(t => 
                    !string.IsNullOrEmpty(t.NganhNghe) && t.NganhNghe == nganhNghe);
            }
            
            // Lọc theo loại công việc
            if (!string.IsNullOrEmpty(loaiCongViec))
            {
                danhSachTinTuyenDung = danhSachTinTuyenDung.Where(t => 
                    !string.IsNullOrEmpty(t.LoaiCongViec) && t.LoaiCongViec == loaiCongViec);
            }
            
            // Lọc theo cấp bậc (vị trí)
            if (!string.IsNullOrEmpty(capBac))
            {
                danhSachTinTuyenDung = danhSachTinTuyenDung.Where(t => 
                    !string.IsNullOrEmpty(t.ViTri) && t.ViTri == capBac);
            }
            
            // Lọc theo kinh nghiệm
            if (!string.IsNullOrEmpty(kinhNghiem))
            {
                danhSachTinTuyenDung = danhSachTinTuyenDung.Where(t => 
                    !string.IsNullOrEmpty(t.KinhNghiem) && t.KinhNghiem == kinhNghiem);
            }
            
            // Lọc theo mức lương
            if (!string.IsNullOrEmpty(mucLuong))
            {
                if (mucLuong == "Dưới 10 triệu")
                {
                    danhSachTinTuyenDung = danhSachTinTuyenDung.Where(t => 
                        t.MucLuongCaoNhat.HasValue && t.MucLuongCaoNhat < 10);
                }
                else if (mucLuong == "10-15 triệu")
                {
                    danhSachTinTuyenDung = danhSachTinTuyenDung.Where(t => 
                        (t.MucLuongThapNhat.HasValue && t.MucLuongThapNhat >= 10 && t.MucLuongThapNhat <= 15) ||
                        (t.MucLuongCaoNhat.HasValue && t.MucLuongCaoNhat >= 10 && t.MucLuongCaoNhat <= 15) ||
                        (t.MucLuongThapNhat.HasValue && t.MucLuongCaoNhat.HasValue && 
                         t.MucLuongThapNhat <= 15 && t.MucLuongCaoNhat >= 10));
                }
                else if (mucLuong == "15-25 triệu")
                {
                    danhSachTinTuyenDung = danhSachTinTuyenDung.Where(t => 
                        (t.MucLuongThapNhat.HasValue && t.MucLuongThapNhat >= 15 && t.MucLuongThapNhat <= 25) ||
                        (t.MucLuongCaoNhat.HasValue && t.MucLuongCaoNhat >= 15 && t.MucLuongCaoNhat <= 25) ||
                        (t.MucLuongThapNhat.HasValue && t.MucLuongCaoNhat.HasValue && 
                         t.MucLuongThapNhat <= 25 && t.MucLuongCaoNhat >= 15));
                }
                else if (mucLuong == "Trên 25 triệu")
                {
                    danhSachTinTuyenDung = danhSachTinTuyenDung.Where(t => 
                        t.MucLuongThapNhat.HasValue && t.MucLuongThapNhat >= 25);
                }
            }
            
            // Sắp xếp
            if (sort == "mới nhất" || sort == "moi nhat")
            {
                danhSachTinTuyenDung = danhSachTinTuyenDung.OrderByDescending(t => t.NgayDang);
            }
            else
            {
                // Mặc định sắp xếp theo số lượng ứng tuyển và ngày đăng
                danhSachTinTuyenDung = danhSachTinTuyenDung
                    .OrderByDescending(t => t.SoLuongUngTuyen ?? 0)
                    .ThenByDescending(t => t.NgayDang);
            }
            
            var ketQuaTimKiem = danhSachTinTuyenDung.ToList();
            
            // Đếm số lượng việc làm theo tỉnh/thành phố và sắp xếp từ cao đến thấp, lấy top 5
            var tatCaTinTuyenDung = _tinTuyenDungRepository.LayDanhSachTinTuyenDung(onlyApproved: true);
            var soLuongViecLamTheoTinh = tatCaTinTuyenDung
                .GroupBy(job => job.TinhThanhPho)
                .Select(g => new { TinhThanhPho = g.Key, SoLuong = g.Count() })
                .OrderByDescending(x => x.SoLuong)
                .Take(5)
                .ToList();
            
            // Lấy danh sách tỉnh/thành phố
            var danhSachTinhThanh = _context.Provinces
                .OrderBy(p => p.FullName)
                .ToList();
            
            // Lấy logo cho các công ty
            var companyNames = ketQuaTimKiem.Select(j => j.CongTy).Distinct().ToList();
            var companyLogos = GetCompanyLogos(companyNames);
            
            // Lấy danh sách ngành nghề từ database
            var danhSachNganhNghe = _context.NganhNghes
                .OrderBy(n => n.TenNganhNghe)
                .ToList();
            
            // Lấy danh sách loại công việc từ database
            var danhSachLoaiCongViec = _context.LoaiCongViecs
                .OrderBy(l => l.TenLoaiCongViec)
                .ToList();
            
            // Lấy danh sách cấp bậc (vị trí) từ DropdownOptions
            var danhSachCapBac = DropdownOptions.ViTri;
            
            // Lấy danh sách kinh nghiệm từ DropdownOptions
            var danhSachKinhNghiem = DropdownOptions.KinhNghiem;
            
            // Truyền các tham số tìm kiếm vào ViewBag để hiển thị lại trong form
            ViewBag.Keyword = keyword;
            ViewBag.TinhThanh = tinhThanh;
            ViewBag.QuanHuyen = quanHuyen;
            ViewBag.NganhNghe = nganhNghe;
            ViewBag.LoaiCongViec = loaiCongViec;
            ViewBag.CapBac = capBac;
            ViewBag.KinhNghiem = kinhNghiem;
            ViewBag.MucLuong = mucLuong;
            ViewBag.Sort = sort;
            ViewBag.SoLuongViecLamTheoTinh = soLuongViecLamTheoTinh;
            ViewBag.DanhSachTinhThanh = danhSachTinhThanh;
            ViewBag.CompanyLogos = companyLogos;
            ViewBag.DanhSachNganhNghe = danhSachNganhNghe;
            ViewBag.DanhSachLoaiCongViec = danhSachLoaiCongViec;
            ViewBag.DanhSachCapBac = danhSachCapBac;
            ViewBag.DanhSachKinhNghiem = danhSachKinhNghiem;
            
            // Tạo danh sách gợi ý tìm kiếm từ dữ liệu thực tế
            var danhSachGoiYTimKiem = new List<GoiYTimKiem>();
            
            // Top ngành nghề có nhiều việc làm nhất (lấy top 3)
            var topNganhNghe = tatCaTinTuyenDung
                .Where(t => !string.IsNullOrEmpty(t.NganhNghe))
                .GroupBy(t => t.NganhNghe)
                .Select(g => new { Ten = g.Key, SoLuong = g.Count() })
                .OrderByDescending(x => x.SoLuong)
                .Take(3)
                .ToList();
            
            foreach (var nganhNgheItem in topNganhNghe)
            {
                danhSachGoiYTimKiem.Add(new GoiYTimKiem 
                { 
                    Text = $"Việc làm {nganhNgheItem.Ten}", 
                    Url = $"/Home/TimViec?nganhNghe={Uri.EscapeDataString(nganhNgheItem.Ten)}" 
                });
            }
            
            // Top loại công việc phổ biến (lấy top 2)
            var topLoaiCongViec = tatCaTinTuyenDung
                .Where(t => !string.IsNullOrEmpty(t.LoaiCongViec))
                .GroupBy(t => t.LoaiCongViec)
                .Select(g => new { Ten = g.Key, SoLuong = g.Count() })
                .OrderByDescending(x => x.SoLuong)
                .Take(2)
                .ToList();
            
            foreach (var loaiCongViecItem in topLoaiCongViec)
            {
                danhSachGoiYTimKiem.Add(new GoiYTimKiem 
                { 
                    Text = $"Việc làm {loaiCongViecItem.Ten}", 
                    Url = $"/Home/TimViec?loaiCongViec={Uri.EscapeDataString(loaiCongViecItem.Ten)}" 
                });
            }
            
            // Thêm gợi ý cố định
            danhSachGoiYTimKiem.Add(new GoiYTimKiem { Text = "Việc làm thực tập", Url = "/Home/ThucTap" });
            danhSachGoiYTimKiem.Add(new GoiYTimKiem { Text = "Việc làm mới nhất", Url = "/Home/TimViec?sort=mới nhất" });
            
            ViewBag.DanhSachGoiYTimKiem = danhSachGoiYTimKiem;
            
            return View(ketQuaTimKiem);
        }

        public IActionResult ThucTap(string keyword, string tinhThanh, string quanHuyen, string nganhNghe, string loaiCongViec, string capBac, string kinhNghiem, string mucLuong)
        {
            // Query trực tiếp từ database để tối ưu hiệu suất - chỉ lấy các tin thực tập
            var danhSachThucTap = _context.TinTuyenDungs
                .Where(t => t.LoaiCongViec != null && t.LoaiCongViec.ToLower().Contains("thực tập") && t.TrangThaiDuyet == "Da duyet")
                .AsQueryable();
            
            // Tìm kiếm theo từ khóa (keyword) - tìm trong TuKhoa, TenViecLam, CongTy, MoTa, YeuCau, KyNang
            if (!string.IsNullOrEmpty(keyword))
            {
                var keywordTrimmed = keyword.Trim();
                danhSachThucTap = danhSachThucTap.Where(t =>
                    (!string.IsNullOrEmpty(t.TuKhoa) && t.TuKhoa.Contains(keywordTrimmed)) ||
                    (!string.IsNullOrEmpty(t.TenViecLam) && t.TenViecLam.Contains(keywordTrimmed)) ||
                    (!string.IsNullOrEmpty(t.CongTy) && t.CongTy.Contains(keywordTrimmed)) ||
                    (!string.IsNullOrEmpty(t.MoTa) && t.MoTa.Contains(keywordTrimmed)) ||
                    (!string.IsNullOrEmpty(t.YeuCau) && t.YeuCau.Contains(keywordTrimmed)) ||
                    (!string.IsNullOrEmpty(t.KyNang) && t.KyNang.Contains(keywordTrimmed)) ||
                    (!string.IsNullOrEmpty(t.NganhNghe) && t.NganhNghe.Contains(keywordTrimmed)) ||
                    (!string.IsNullOrEmpty(t.NganhNgheChiTiet) && t.NganhNgheChiTiet.Contains(keywordTrimmed))
                );
            }
            
            // Lọc theo tỉnh/thành phố
            if (!string.IsNullOrEmpty(tinhThanh))
            {
                danhSachThucTap = danhSachThucTap.Where(t => 
                    !string.IsNullOrEmpty(t.TinhThanhPho) && t.TinhThanhPho.Contains(tinhThanh));
            }
            
            // Lọc theo quận/huyện
            if (!string.IsNullOrEmpty(quanHuyen))
            {
                danhSachThucTap = danhSachThucTap.Where(t => 
                    !string.IsNullOrEmpty(t.PhuongXa) && t.PhuongXa.Contains(quanHuyen));
            }
            
            // Lọc theo ngành nghề
            if (!string.IsNullOrEmpty(nganhNghe))
            {
                danhSachThucTap = danhSachThucTap.Where(t => 
                    !string.IsNullOrEmpty(t.NganhNghe) && t.NganhNghe == nganhNghe);
            }
            
            // Lọc theo loại công việc
            if (!string.IsNullOrEmpty(loaiCongViec))
            {
                danhSachThucTap = danhSachThucTap.Where(t => 
                    !string.IsNullOrEmpty(t.LoaiCongViec) && t.LoaiCongViec == loaiCongViec);
            }
            
            // Lọc theo cấp bậc (vị trí)
            if (!string.IsNullOrEmpty(capBac))
            {
                danhSachThucTap = danhSachThucTap.Where(t => 
                    !string.IsNullOrEmpty(t.ViTri) && t.ViTri == capBac);
            }
            
            // Lọc theo kinh nghiệm
            if (!string.IsNullOrEmpty(kinhNghiem))
            {
                danhSachThucTap = danhSachThucTap.Where(t => 
                    !string.IsNullOrEmpty(t.KinhNghiem) && t.KinhNghiem == kinhNghiem);
            }
            
            // Lọc theo mức lương (phụ cấp)
            if (!string.IsNullOrEmpty(mucLuong))
            {
                if (mucLuong == "Dưới 5 triệu")
                {
                    danhSachThucTap = danhSachThucTap.Where(t => 
                        t.MucLuongCaoNhat.HasValue && t.MucLuongCaoNhat < 5);
                }
                else if (mucLuong == "5-7 triệu")
                {
                    danhSachThucTap = danhSachThucTap.Where(t => 
                        (t.MucLuongThapNhat.HasValue && t.MucLuongThapNhat >= 5 && t.MucLuongThapNhat <= 7) ||
                        (t.MucLuongCaoNhat.HasValue && t.MucLuongCaoNhat >= 5 && t.MucLuongCaoNhat <= 7) ||
                        (t.MucLuongThapNhat.HasValue && t.MucLuongCaoNhat.HasValue && 
                         t.MucLuongThapNhat <= 7 && t.MucLuongCaoNhat >= 5));
                }
                else if (mucLuong == "7-10 triệu")
                {
                    danhSachThucTap = danhSachThucTap.Where(t => 
                        (t.MucLuongThapNhat.HasValue && t.MucLuongThapNhat >= 7 && t.MucLuongThapNhat <= 10) ||
                        (t.MucLuongCaoNhat.HasValue && t.MucLuongCaoNhat >= 7 && t.MucLuongCaoNhat <= 10) ||
                        (t.MucLuongThapNhat.HasValue && t.MucLuongCaoNhat.HasValue && 
                         t.MucLuongThapNhat <= 10 && t.MucLuongCaoNhat >= 7));
                }
                else if (mucLuong == "Trên 10 triệu")
                {
                    danhSachThucTap = danhSachThucTap.Where(t => 
                        t.MucLuongThapNhat.HasValue && t.MucLuongThapNhat >= 10);
                }
            }
            
            // Sắp xếp theo số lượng ứng tuyển và ngày đăng
            danhSachThucTap = danhSachThucTap
                .OrderByDescending(t => t.SoLuongUngTuyen ?? 0)
                .ThenByDescending(t => t.NgayDang);
            
            var ketQuaTimKiem = danhSachThucTap.ToList();
            
            // Đếm số lượng việc làm thực tập theo tỉnh/thành phố và sắp xếp từ cao đến thấp
            var tatCaTinThucTapForCount = _context.TinTuyenDungs
                .Where(t => t.LoaiCongViec != null && t.LoaiCongViec.ToLower().Contains("thực tập") && t.TrangThaiDuyet == "Da duyet");
            var soLuongViecLamTheoTinh = tatCaTinThucTapForCount
                .GroupBy(job => job.TinhThanhPho)
                .Select(g => new { TinhThanhPho = g.Key, SoLuong = g.Count() })
                .OrderByDescending(x => x.SoLuong)
                .ToList();
            
            // Lấy danh sách tỉnh/thành phố
            var danhSachTinhThanh = _context.Provinces
                .OrderBy(p => p.FullName)
                .ToList();
            
            // Lấy logo cho các công ty
            var companyNames = ketQuaTimKiem.Select(j => j.CongTy).Distinct().ToList();
            var companyLogos = GetCompanyLogos(companyNames);
            
            // Lấy danh sách ngành nghề từ database
            var danhSachNganhNghe = _context.NganhNghes
                .OrderBy(n => n.TenNganhNghe)
                .ToList();
            
            // Lấy danh sách loại công việc từ database
            var danhSachLoaiCongViec = _context.LoaiCongViecs
                .OrderBy(l => l.TenLoaiCongViec)
                .ToList();
            
            // Lấy danh sách cấp bậc (vị trí) từ DropdownOptions
            var danhSachCapBac = DropdownOptions.ViTri;
            
            // Lấy danh sách kinh nghiệm từ DropdownOptions
            var danhSachKinhNghiem = DropdownOptions.KinhNghiem;
            
            // Lấy tất cả tin thực tập để tạo gợi ý
            var tatCaTinThucTap = _tinTuyenDungRepository.LayDanhSachThucTap()
                .Where(t => t.TrangThaiDuyet == "Da duyet")
                .ToList();
            
            // Tạo danh sách gợi ý tìm kiếm từ dữ liệu thực tế
            var danhSachGoiYTimKiem = new List<GoiYTimKiem>();
            
            // Top ngành nghề có nhiều việc thực tập nhất (lấy top 3)
            var topNganhNghe = tatCaTinThucTap
                .Where(t => !string.IsNullOrEmpty(t.NganhNghe))
                .GroupBy(t => t.NganhNghe)
                .Select(g => new { Ten = g.Key, SoLuong = g.Count() })
                .OrderByDescending(x => x.SoLuong)
                .Take(3)
                .ToList();
            
            foreach (var nganhNgheItem in topNganhNghe)
            {
                danhSachGoiYTimKiem.Add(new GoiYTimKiem 
                { 
                    Text = $"Thực tập {nganhNgheItem.Ten}", 
                    Url = $"/Home/ThucTap?nganhNghe={Uri.EscapeDataString(nganhNgheItem.Ten)}" 
                });
            }
            
            // Top tỉnh/thành phố có nhiều việc thực tập nhất (lấy top 2)
            var topTinhThanh = tatCaTinThucTap
                .Where(t => !string.IsNullOrEmpty(t.TinhThanhPho))
                .GroupBy(t => t.TinhThanhPho)
                .Select(g => new { Ten = g.Key, SoLuong = g.Count() })
                .OrderByDescending(x => x.SoLuong)
                .Take(2)
                .ToList();
            
            foreach (var tinhThanhItem in topTinhThanh)
            {
                danhSachGoiYTimKiem.Add(new GoiYTimKiem 
                { 
                    Text = $"Thực tập tại {tinhThanhItem.Ten}", 
                    Url = $"/Home/ThucTap?tinhThanh={Uri.EscapeDataString(tinhThanhItem.Ten)}" 
                });
            }
            
            // Thêm gợi ý cố định
            danhSachGoiYTimKiem.Add(new GoiYTimKiem { Text = "Thực tập có lương", Url = "/Home/ThucTap?mucLuong=Trên 10 triệu" });
            
            // Truyền các tham số tìm kiếm vào ViewBag để hiển thị lại trong form
            ViewBag.Keyword = keyword;
            ViewBag.TinhThanh = tinhThanh;
            ViewBag.QuanHuyen = quanHuyen;
            ViewBag.NganhNghe = nganhNghe;
            ViewBag.LoaiCongViec = loaiCongViec;
            ViewBag.CapBac = capBac;
            ViewBag.KinhNghiem = kinhNghiem;
            ViewBag.MucLuong = mucLuong;
            ViewBag.SoLuongViecLamTheoTinh = soLuongViecLamTheoTinh;
            ViewBag.DanhSachTinhThanh = danhSachTinhThanh;
            ViewBag.CompanyLogos = companyLogos;
            ViewBag.DanhSachNganhNghe = danhSachNganhNghe;
            ViewBag.DanhSachLoaiCongViec = danhSachLoaiCongViec;
            ViewBag.DanhSachCapBac = danhSachCapBac;
            ViewBag.DanhSachKinhNghiem = danhSachKinhNghiem;
            ViewBag.DanhSachGoiYTimKiem = danhSachGoiYTimKiem;
            
            return View(ketQuaTimKiem);
        }

        public IActionResult CongTy()
        {
            var danhSachCongTy = _nhaTuyenDungRepository.LayDanhSachNhaTuyenDung();
            
            // Lấy tất cả tin tuyển dụng và nhóm theo tên công ty để tính số tin đã đăng
            var tatCaTinTuyenDung = _tinTuyenDungRepository.LayDanhSachTinTuyenDung(onlyApproved: true);
            var soTinTheoCongTy = tatCaTinTuyenDung
                .Where(t => !string.IsNullOrEmpty(t.CongTy))
                .GroupBy(t => t.CongTy.ToLower().Trim())
                .ToDictionary(g => g.Key, g => g.Count());
            
            // Lấy tất cả đơn ứng tuyển và nhóm theo công ty để tính tổng số đơn ứng tuyển
            var tatCaTinUngTuyen = _context.TinUngTuyens.ToList();
            var soDonUngTuyenTheoCongTy = tatCaTinUngTuyen
                .Where(t => !string.IsNullOrEmpty(t.CongTy))
                .GroupBy(t => t.CongTy.ToLower().Trim())
                .ToDictionary(g => g.Key, g => g.Count());
            
            // Tạo dictionary để lưu thống kê cho mỗi công ty
            var thongKeCongTy = new Dictionary<int, (int SoTinDaDang, int SoUngVienNhan)>();
            
            foreach (var congTy in danhSachCongTy)
            {
                var tenCongTyLower = congTy.TenCongTy?.ToLower().Trim() ?? "";
                
                // Tìm số tin đã đăng - kiểm tra cả exact match và partial match
                var soTinDaDang = 0;
                if (soTinTheoCongTy.ContainsKey(tenCongTyLower))
                {
                    soTinDaDang = soTinTheoCongTy[tenCongTyLower];
                }
                else
                {
                    // Thử tìm với partial match
                    var matchingKey = soTinTheoCongTy.Keys
                        .FirstOrDefault(k => k.Contains(tenCongTyLower) || tenCongTyLower.Contains(k));
                    if (matchingKey != null)
                    {
                        soTinDaDang = soTinTheoCongTy[matchingKey];
                    }
                }
                
                // Tìm tổng số đơn ứng tuyển mà công ty nhận được - kiểm tra cả exact match và partial match
                var soDonUngTuyen = 0;
                if (soDonUngTuyenTheoCongTy.ContainsKey(tenCongTyLower))
                {
                    soDonUngTuyen = soDonUngTuyenTheoCongTy[tenCongTyLower];
                }
                else
                {
                    // Thử tìm với partial match (trường hợp tên công ty trong TinUngTuyen có thể khác một chút)
                    var matchingKey = soDonUngTuyenTheoCongTy.Keys
                        .FirstOrDefault(k => k.Contains(tenCongTyLower) || tenCongTyLower.Contains(k));
                    if (matchingKey != null)
                    {
                        soDonUngTuyen = soDonUngTuyenTheoCongTy[matchingKey];
                    }
                }
                
                thongKeCongTy[congTy.MaNhaTuyenDung] = (soTinDaDang, soDonUngTuyen);
            }
            
            // Lấy danh sách tỉnh/thành phố
            var danhSachTinhThanh = _context.Provinces
                .OrderBy(p => p.FullName)
                .ToList();
            
            ViewBag.DanhSachTinhThanh = danhSachTinhThanh;
            ViewBag.ThongKeCongTy = thongKeCongTy;
            return View(danhSachCongTy);
        }

        public IActionResult UngVien(string keyword, string tinhThanh, string nganhNghe, string trangThaiTimViec, string mucLuong)
        {
            // Chỉ cho phép nhà tuyển dụng và admin xem
            if (!User.IsInRole(Unicareer.Models.SD.Role_NhaTuyenDung) && !User.IsInRole(Unicareer.Models.SD.Role_Admin))
            {
                return RedirectToAction("Index");
            }

            // Query trực tiếp từ database
            var danhSachUngVien = _context.UngViens
                .Include(u => u.User)
                .Include(u => u.ChuyenNganh)
                    .ThenInclude(c => c!.NganhNghe)
                .Where(u => u.HienThiCongKhai && !string.IsNullOrEmpty(u.HoTen))
                .AsQueryable();

            // Tìm kiếm theo từ khóa
            if (!string.IsNullOrEmpty(keyword))
            {
                var keywordTrimmed = keyword.Trim();
                danhSachUngVien = danhSachUngVien.Where(u =>
                    (!string.IsNullOrEmpty(u.HoTen) && u.HoTen.Contains(keywordTrimmed)) ||
                    (!string.IsNullOrEmpty(u.ViTriMongMuon) && u.ViTriMongMuon.Contains(keywordTrimmed)) ||
                    (!string.IsNullOrEmpty(u.MoTaBanThan) && u.MoTaBanThan.Contains(keywordTrimmed)) ||
                    (!string.IsNullOrEmpty(u.KyNangChiTiet) && u.KyNangChiTiet.Contains(keywordTrimmed)) ||
                    (u.ChuyenNganh != null && !string.IsNullOrEmpty(u.ChuyenNganh.TenChuyenNganh) && u.ChuyenNganh.TenChuyenNganh.Contains(keywordTrimmed))
                );
            }

            // Lọc theo tỉnh/thành phố
            if (!string.IsNullOrEmpty(tinhThanh))
            {
                danhSachUngVien = danhSachUngVien.Where(u =>
                    !string.IsNullOrEmpty(u.NoiLamViecMongMuon) && u.NoiLamViecMongMuon.Contains(tinhThanh));
            }

            // Lọc theo chuyên ngành
            if (!string.IsNullOrEmpty(nganhNghe))
            {
                danhSachUngVien = danhSachUngVien.Where(u =>
                    u.ChuyenNganh != null && u.ChuyenNganh.NganhNghe != null && u.ChuyenNganh.NganhNghe.TenNganhNghe == nganhNghe);
            }

            // Lọc theo trạng thái tìm việc
            if (!string.IsNullOrEmpty(trangThaiTimViec))
            {
                danhSachUngVien = danhSachUngVien.Where(u => u.TrangThaiTimViec == trangThaiTimViec);
            }

            // Lọc theo mức lương kỳ vọng
            if (!string.IsNullOrEmpty(mucLuong))
            {
                if (mucLuong == "Dưới 10 triệu")
                {
                    danhSachUngVien = danhSachUngVien.Where(u => u.MucLuongKyVong.HasValue && u.MucLuongKyVong < 10);
                }
                else if (mucLuong == "10-15 triệu")
                {
                    danhSachUngVien = danhSachUngVien.Where(u => u.MucLuongKyVong.HasValue && u.MucLuongKyVong >= 10 && u.MucLuongKyVong <= 15);
                }
                else if (mucLuong == "15-25 triệu")
                {
                    danhSachUngVien = danhSachUngVien.Where(u => u.MucLuongKyVong.HasValue && u.MucLuongKyVong >= 15 && u.MucLuongKyVong <= 25);
                }
                else if (mucLuong == "Trên 25 triệu")
                {
                    danhSachUngVien = danhSachUngVien.Where(u => u.MucLuongKyVong.HasValue && u.MucLuongKyVong >= 25);
                }
            }

            // Sắp xếp theo ngày đăng ký mới nhất
            danhSachUngVien = danhSachUngVien.OrderByDescending(u => u.NgayDangKy);

            var ketQuaTimKiem = danhSachUngVien.ToList();

            // Lấy danh sách tỉnh/thành phố
            var danhSachTinhThanh = _context.Provinces
                .OrderBy(p => p.FullName)
                .ToList();

            // Lấy danh sách ngành nghề
            var danhSachNganhNghe = _context.NganhNghes
                .OrderBy(n => n.TenNganhNghe)
                .ToList();

            // Truyền các tham số tìm kiếm vào ViewBag
            ViewBag.Keyword = keyword;
            ViewBag.TinhThanh = tinhThanh;
            ViewBag.NganhNghe = nganhNghe;
            ViewBag.TrangThaiTimViec = trangThaiTimViec;
            ViewBag.MucLuong = mucLuong;
            ViewBag.DanhSachTinhThanh = danhSachTinhThanh;
            ViewBag.DanhSachNganhNghe = danhSachNganhNghe;
            ViewBag.TrangThaiTimViecOptions = new List<string> { "Đang tìm việc", "Đang thực tập", "Đã có việc" };

            return View(ketQuaTimKiem);
        }

        public IActionResult ChiTietCongTy(int id)
        {
            var congTy = _nhaTuyenDungRepository.LayNhaTuyenDungTheoId(id);
            if (congTy == null)
            {
                return NotFound();
            }

            // Lấy danh sách công việc của công ty
            var danhSachViecLam = _tinTuyenDungRepository.LayDanhSachTheoCongTy(congTy.TenCongTy);
            
            // Tính toán số tin đã đăng từ dữ liệu thật
            var soTinDaDang = danhSachViecLam.Count;
            
            // Tính tổng số đơn ứng tuyển mà công ty nhận được (tất cả trạng thái)
            var tenCongTyLower = congTy.TenCongTy?.ToLower().Trim() ?? "";
            var soUngVienNhan = _context.TinUngTuyens
                .Where(t => t.CongTy != null && 
                    (t.CongTy.ToLower().Trim() == tenCongTyLower || 
                     t.CongTy.ToLower().Trim().Contains(tenCongTyLower) ||
                     tenCongTyLower.Contains(t.CongTy.ToLower().Trim())))
                .Count();
            
            // Tính toán số tháng tham gia
            var soThangThamGia = congTy.NgayDangKy <= DateTime.Now 
                ? Math.Max(1, (DateTime.Now - congTy.NgayDangKy).Days / 30) 
                : 0;
            
            ViewBag.DanhSachViecLam = danhSachViecLam;
            ViewBag.SoTinDaDang = soTinDaDang;
            ViewBag.SoUngVienNhan = soUngVienNhan;
            ViewBag.SoThangThamGia = soThangThamGia;
            return View(congTy);
        }

        public async Task<IActionResult> ChiTietViecLam(int id)
        {
            var tinTuyenDung = _tinTuyenDungRepository.LayTinTuyenDungTheoId(id);
            if (tinTuyenDung == null)
            {
                return NotFound();
            }
            
            if (tinTuyenDung.TrangThaiDuyet != "Da duyet")
            {
                return NotFound();
            }
            
            // Tự động cập nhật trạng thái "Het han" nếu đã quá hạn nộp và chưa được set
            if (tinTuyenDung.HanNop < DateTime.Now.Date && tinTuyenDung.TrangThai != "Het han" && tinTuyenDung.TrangThai != "Da dong")
            {
                // Tạo object TinTuyenDung tạm thời với trạng thái mới để sử dụng AutoMapper
                var tinTuyenDungNguon = new TinTuyenDung
                {
                    TrangThai = "Het han"
                };

                // Cập nhật trạng thái bằng AutoMapper
                // AutoMapper sẽ tự động map các thuộc tính và bỏ qua các thuộc tính đặc biệt
                _mapper.Map(tinTuyenDungNguon, tinTuyenDung);
                _context.SaveChanges();
            }
            
            // Lấy logo công ty
            var nhaTuyenDung = _context.NhaTuyenDungs
                .FirstOrDefault(n => n.TenCongTy == tinTuyenDung.CongTy);
            ViewBag.CompanyLogo = nhaTuyenDung?.Logo;
            
            // Lấy danh sách trường đại học từ database
            var danhSachTruongDaiHoc = _truongDaiHocRepository.LayDanhSachTruongDaiHoc();
            ViewBag.DanhSachTruongDaiHoc = danhSachTruongDaiHoc;
            
            // Lấy danh sách ngành nghề từ database
            var danhSachNganhNghe = _nganhNgheRepository.LayDanhSachNganhNghe();
            ViewBag.DanhSachNganhNghe = danhSachNganhNghe;
            
            // Lấy danh sách việc làm khác từ cùng công ty (loại trừ tin hiện tại)
            var danhSachViecLamKhac = _tinTuyenDungRepository.LayDanhSachTheoCongTy(tinTuyenDung.CongTy ?? "")
                .Where(t => t.TrangThaiDuyet == "Da duyet" &&
                           t.MaTinTuyenDung != id && 
                           t.TrangThai != "Da dong" && 
                           t.TrangThai != "Het han" &&
                           t.HanNop >= DateTime.Now.Date)
                .OrderByDescending(t => t.NgayDang)
                .Take(10)
                .ToList();
            ViewBag.DanhSachViecLamKhac = danhSachViecLamKhac;
            
            // Lấy thông tin ứng viên nếu đã đăng nhập và là ứng viên
            UngVien? ungVien = null;
            if (User.Identity?.IsAuthenticated == true && User.IsInRole(SD.Role_UngVien))
            {
                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser != null)
                {
                    ungVien = _ungVienRepository.LayUngVienTheoUserId(currentUser.Id);
                }
            }
            ViewBag.UngVien = ungVien;
            
            return View(tinTuyenDung);
        }

        // POST: Ung tuyen
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UngTuyen(
            int jobId, 
            string fullName, 
            string email, 
            string phone, 
            int? maTruong, 
            string? major, 
            string content,
            IFormFile? cvFile)
        {
            try
            {
                // Validate required fields
                if (string.IsNullOrWhiteSpace(fullName) || 
                    string.IsNullOrWhiteSpace(email) || 
                    string.IsNullOrWhiteSpace(phone) || 
                    string.IsNullOrWhiteSpace(content))
                {
                    return Json(new { success = false, message = "Vui lòng điền đầy đủ các trường bắt buộc" });
                }

                // Validate email format
                if (!email.Contains("@") || !email.Contains("."))
                {
                    return Json(new { success = false, message = "Email không hợp lệ" });
                }

                // Validate phone format (10-11 digits)
                var phoneDigits = phone.Replace(" ", "").Replace("-", "");
                if (phoneDigits.Length < 10 || phoneDigits.Length > 11 || !phoneDigits.All(char.IsDigit))
                {
                    return Json(new { success = false, message = "Số điện thoại không hợp lệ" });
                }

                // Kiểm tra tin tuyển dụng có tồn tại không
                var tinTuyenDung = _tinTuyenDungRepository.LayTinTuyenDungTheoId(jobId);
                if (tinTuyenDung == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy tin tuyển dụng" });
                }

                if (tinTuyenDung.TrangThaiDuyet != "Da duyet")
                {
                    var notificationMessage = tinTuyenDung.TrangThaiDuyet switch
                    {
                        "Tu choi" => $"Tin tuyển dụng đã bị từ chối{(!string.IsNullOrWhiteSpace(tinTuyenDung.LyDoTuChoi) ? $": {tinTuyenDung.LyDoTuChoi}" : ".")}",
                        _ => "Tin tuyển dụng này đang chờ admin duyệt."
                    };

                    return Json(new { success = false, message = notificationMessage });
                }

                // Kiểm tra trạng thái tin tuyển dụng
                if (tinTuyenDung.TrangThai == "Da dong")
                {
                    return Json(new { success = false, message = "Tin tuyển dụng này đã được đóng. Không thể ứng tuyển nữa." });
                }

                // Kiểm tra trạng thái hết hạn
                if (tinTuyenDung.TrangThai == "Het han")
                {
                    return Json(new { success = false, message = "Tin tuyển dụng đã hết hạn. Không thể ứng tuyển nữa." });
                }

                // Kiểm tra hạn nộp (nếu trạng thái chưa được set là "Het han" nhưng đã quá hạn)
                if (tinTuyenDung.HanNop < DateTime.Now.Date)
                {
                    // Tự động cập nhật trạng thái thành "Het han" nếu chưa được set
                    if (tinTuyenDung.TrangThai != "Het han")
                    {
                        // Tạo object TinTuyenDung tạm thời với trạng thái mới để sử dụng AutoMapper
                        var tinTuyenDungNguon = new TinTuyenDung
                        {
                            TrangThai = "Het han"
                        };

                        // Cập nhật trạng thái bằng AutoMapper
                        // AutoMapper sẽ tự động map các thuộc tính và bỏ qua các thuộc tính đặc biệt
                        _mapper.Map(tinTuyenDungNguon, tinTuyenDung);
                        _context.SaveChanges();
                    }
                    return Json(new { success = false, message = "Tin tuyển dụng đã hết hạn nộp hồ sơ. Không thể ứng tuyển nữa." });
                }

                // Upload CV file nếu có
                string? cvPath = null;
                if (cvFile != null && cvFile.Length > 0)
                {
                    cvPath = await UploadCVFileAsync(cvFile);
                    if (string.IsNullOrEmpty(cvPath))
                    {
                        return Json(new { success = false, message = "Không thể upload file CV" });
                    }
                }

                // Lấy UserId (bắt buộc đăng nhập nên user luôn có)
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy thông tin người dùng. Vui lòng đăng nhập lại." });
                }
                string userId = user.Id;

                // Lấy thông tin trường (nếu có) để lưu kèm
                string? tenTruong = null;
                if (maTruong.HasValue)
                {
                    var truong = _truongDaiHocRepository.LayTruongDaiHocTheoId(maTruong.Value);
                    tenTruong = truong?.TenTruong;
                }

                // Tạo tin ứng tuyển
                var tinUngTuyen = new TinUngTuyen
                {
                    UserId = userId, // Bắt buộc đăng nhập nên UserId luôn có giá trị
                    HoTen = fullName.Trim(),
                    Email = email.Trim(),
                    SoDienThoai = phone.Trim(),
                    MaTruong = maTruong,
                    ViTriUngTuyen = tinTuyenDung.TenViecLam ?? "",
                    CongTy = tinTuyenDung.CongTy ?? "",
                    MaTinTuyenDung = jobId.ToString(),
                    TrangThaiXuLy = TrangThaiXuLyHelper.ToString(TrangThaiXuLy.DangXemXet),
                    LinkCV = cvPath ?? "",
                    GhiChu = $"Trường: {tenTruong ?? "Không có"}; Ngành: {major ?? "Không có"}; Nội dung: {content.Trim()}",
                    NgayUngTuyen = DateTime.Now
                };

                // Lưu vào database
                _context.TinUngTuyens.Add(tinUngTuyen);

                // Cập nhật số lượng ứng tuyển
                if (tinTuyenDung.SoLuongUngTuyen == null)
                {
                    tinTuyenDung.SoLuongUngTuyen = 0;
                }
                tinTuyenDung.SoLuongUngTuyen++;

                _context.SaveChanges();

                // Gửi email xác nhận cho ứng viên
                try
                {
                    // Ưu tiên sử dụng email từ tin ứng tuyển, nếu không có thì dùng email từ tài khoản
                    string candidateEmail = !string.IsNullOrWhiteSpace(email) ? email.Trim() : (user.Email ?? "");
                    
                    if (!string.IsNullOrWhiteSpace(candidateEmail))
                    {
                        _ = Task.Run(async () =>
                        {
                            await _emailService.SendApplicationConfirmationAsync(
                                candidateEmail,
                                fullName.Trim(),
                                tinTuyenDung.TenViecLam ?? "",
                                tinTuyenDung.CongTy ?? ""
                            );
                        });
                    }
                }
                catch (Exception emailEx)
                {
                    _logger.LogError(emailEx, "Lỗi khi gửi email xác nhận đơn ứng tuyển cho ứng viên");
                }

                // Gửi email thông báo cho nhà tuyển dụng
                try
                {
                    if (tinTuyenDung.MaNhaTuyenDung.HasValue)
                    {
                        var nhaTuyenDung = _nhaTuyenDungRepository.LayNhaTuyenDungTheoId(tinTuyenDung.MaNhaTuyenDung.Value);
                        if (nhaTuyenDung != null && nhaTuyenDung.User != null && !string.IsNullOrEmpty(nhaTuyenDung.User.Email))
                        {
                            _ = Task.Run(async () =>
                            {
                                await _emailService.SendNewApplicationNotificationAsync(
                                    nhaTuyenDung.User.Email,
                                    fullName.Trim(),
                                    tinTuyenDung.TenViecLam ?? "",
                                    tinTuyenDung.CongTy ?? ""
                                );
                            });
                        }
                    }
                }
                catch (Exception emailEx)
                {
                    _logger.LogError(emailEx, "Lỗi khi gửi email thông báo đơn ứng tuyển mới cho nhà tuyển dụng");
                }

                return Json(new { success = true, message = "Đã gửi đơn ứng tuyển thành công! Chúng tôi sẽ liên hệ với bạn sớm nhất." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        // Helper method để upload CV file
        private async Task<string?> UploadCVFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            // Kiểm tra định dạng file (cho phép PDF, DOC, DOCX, PNG, JPEG, JPG)
            var allowedExtensions = new[] { ".pdf", ".doc", ".docx", ".png", ".jpeg", ".jpg" };
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            
            if (!allowedExtensions.Contains(fileExtension))
            {
                throw new Exception("Định dạng file không hợp lệ. Chỉ chấp nhận PDF, DOC, DOCX, PNG, JPEG, JPG.");
            }

            // Kiểm tra kích thước file (tối đa 2048KB = 2MB)
            var maxSize = 2048 * 1024; // 2MB
            if (file.Length > maxSize)
            {
                throw new Exception("Kích thước file quá lớn. Tối đa 2048KB.");
            }

            // Tạo tên file unique
            var fileName = $"CV_{DateTime.Now:yyyyMMddHHmmss}_{Guid.NewGuid():N}{fileExtension}";
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

        public IActionResult ChiTietUngVien(int id)
        {
            // Chỉ cho phép nhà tuyển dụng và admin xem
            if (!User.IsInRole(Unicareer.Models.SD.Role_NhaTuyenDung) && !User.IsInRole(Unicareer.Models.SD.Role_Admin))
            {
                return RedirectToAction("Index");
            }

            var ungVien = _context.UngViens
                .Include(u => u.User)
                .Include(u => u.ChuyenNganh)
                    .ThenInclude(c => c!.NganhNghe)
                .FirstOrDefault(u => u.MaUngVien == id);

            if (ungVien == null)
            {
                return NotFound();
            }

            // Kiểm tra xem ứng viên có cho phép hiển thị công khai không
            if (!ungVien.HienThiCongKhai)
            {
                TempData["Loi"] = "Ứng viên này không cho phép hiển thị hồ sơ công khai.";
                return RedirectToAction("UngVien");
            }

            return View(ungVien);
        }

        public IActionResult Blog(int? categoryId)
        {
            var danhSachBlog = _blogRepository.LayDanhSachBlogHienThi();
            if (categoryId.HasValue)
            {
                danhSachBlog = danhSachBlog
                    .Where(b => b.MaTheLoai.HasValue && b.MaTheLoai.Value == categoryId.Value)
                    .ToList();
            }
            var danhSachTheLoaiBlog = _context.TheLoaiBlogs
                .Where(t => t.HienThi)
                .OrderBy(t => t.ThuTu)
                .ThenBy(t => t.TenTheLoai)
                .ToList();

            ViewBag.DanhSachTheLoaiBlog = danhSachTheLoaiBlog;
            ViewBag.SelectedCategoryId = categoryId;
            return View(danhSachBlog);
        }

        public IActionResult BlogDetail(int? id, string? permalink)
        {
            Blog? blog = null;

            // Tìm blog theo ID hoặc permalink
            if (id.HasValue && id.Value > 0)
            {
                blog = _blogRepository.LayBlogTheoId(id.Value);
            }
            else if (!string.IsNullOrEmpty(permalink))
            {
                // Loại bỏ phần .html nếu có
                var cleanPermalink = permalink.Replace(".html", "").Trim();
                
                // Tìm blog trong danh sách blog đã hiển thị (đã đăng, đã duyệt, đã hiển thị)
                // Điều này đảm bảo chỉ tìm trong các blog công khai, tránh trường hợp có nhiều blog cùng permalink
                var allBlogs = _blogRepository.LayDanhSachBlogHienThi();
                
                // Thử tìm blog theo permalink chính xác (không phân biệt hoa thường)
                blog = allBlogs.FirstOrDefault(b => 
                    !string.IsNullOrEmpty(b.Permalink) && 
                    b.Permalink.Equals(cleanPermalink, StringComparison.OrdinalIgnoreCase));
                
                // Nếu không tìm thấy, thử tìm với .html ở cuối
                if (blog == null)
                {
                    blog = allBlogs.FirstOrDefault(b => 
                        !string.IsNullOrEmpty(b.Permalink) && 
                        b.Permalink.Equals(cleanPermalink + ".html", StringComparison.OrdinalIgnoreCase));
                }
                
                // Nếu vẫn không tìm thấy, thử loại bỏ .html từ permalink trong database
                if (blog == null)
                {
                    blog = allBlogs.FirstOrDefault(b => 
                        !string.IsNullOrEmpty(b.Permalink) && 
                        b.Permalink.Replace(".html", "").Equals(cleanPermalink, StringComparison.OrdinalIgnoreCase));
                }
            }

            if (blog == null)
            {
                return NotFound();
            }

            // Kiểm tra lại trạng thái blog (đã được filter ở trên, nhưng kiểm tra lại để đảm bảo)
            if (!blog.DaDang || !blog.HienThi)
            {
                return NotFound();
            }

            // Tăng lượt xem
            _blogRepository.TangLuotXem(blog.MaBlog);

            // Lấy blog mới nhất để hiển thị ở sidebar (tối đa 5 bài)
            var danhSachBlogLienQuan = _blogRepository.LayDanhSachBlogHienThi()
                .Where(b => b.MaBlog != blog.MaBlog)
                .Take(5)
                .ToList();

            ViewBag.DanhSachBlogLienQuan = danhSachBlogLienQuan;

            return View(blog);
        }

        public IActionResult BlogDetailPartial(int? id, string? permalink)
        {
            Blog? blog = null;

            // Tìm blog theo ID hoặc permalink (sử dụng logic tương tự BlogDetail)
            if (id.HasValue && id.Value > 0)
            {
                blog = _blogRepository.LayBlogTheoId(id.Value);
            }
            else if (!string.IsNullOrEmpty(permalink))
            {
                var cleanPermalink = permalink.Replace(".html", "").Trim();
                var allBlogs = _blogRepository.LayDanhSachBlogHienThi();
                
                blog = allBlogs.FirstOrDefault(b => 
                    !string.IsNullOrEmpty(b.Permalink) && 
                    b.Permalink.Equals(cleanPermalink, StringComparison.OrdinalIgnoreCase));
                
                if (blog == null)
                {
                    blog = allBlogs.FirstOrDefault(b => 
                        !string.IsNullOrEmpty(b.Permalink) && 
                        b.Permalink.Equals(cleanPermalink + ".html", StringComparison.OrdinalIgnoreCase));
                }
                
                if (blog == null)
                {
                    blog = allBlogs.FirstOrDefault(b => 
                        !string.IsNullOrEmpty(b.Permalink) && 
                        b.Permalink.Replace(".html", "").Equals(cleanPermalink, StringComparison.OrdinalIgnoreCase));
                }
            }

            if (blog == null || !blog.DaDang || !blog.HienThi)
            {
                return NotFound();
            }

            // Tăng lượt xem
            _blogRepository.TangLuotXem(blog.MaBlog);

            // Cập nhật sidebar với danh sách blog liên quan mới
            var danhSachBlogLienQuan = _blogRepository.LayDanhSachBlogHienThi()
                .Where(b => b.MaBlog != blog.MaBlog)
                .Take(5)
                .ToList();

            ViewBag.DanhSachBlogLienQuan = danhSachBlogLienQuan;
            ViewBag.CurrentBlogId = blog.MaBlog;
            ViewBag.CurrentBlogTitle = blog.TieuDe;

            return PartialView("_BlogDetailMain", blog);
        }

        public IActionResult BlogDetailSidebar(int? excludeBlogId)
        {
            var danhSachBlogLienQuan = _blogRepository.LayDanhSachBlogHienThi();
            
            if (excludeBlogId.HasValue)
            {
                danhSachBlogLienQuan = danhSachBlogLienQuan
                    .Where(b => b.MaBlog != excludeBlogId.Value)
                    .ToList();
            }
            
            danhSachBlogLienQuan = danhSachBlogLienQuan.Take(5).ToList();
            
            ViewBag.DanhSachBlogLienQuan = danhSachBlogLienQuan;
            
            return PartialView("_BlogDetailSidebar");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
