using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Unicareer.Models;
using Unicareer.Models.ViewModels;
using Unicareer.Repository;
using Unicareer.Data;
using Microsoft.EntityFrameworkCore;

namespace Unicareer.Areas.Recruiter.Controllers
{
    [Area("Recruiter")]
    [Authorize(Roles = $"{SD.Role_NhaTuyenDung}")]
    public class RecruiterController : Controller
    {
        private readonly ITinTuyenDungRepository _tinTuyenDungRepository;
        private readonly ITinUngTuyenRepository _tinUngTuyenRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public RecruiterController(
            ITinTuyenDungRepository tinTuyenDungRepository, 
            ITinUngTuyenRepository tinUngTuyenRepository,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            _tinTuyenDungRepository = tinTuyenDungRepository;
            _tinUngTuyenRepository = tinUngTuyenRepository;
            _userManager = userManager;
            _context = context;
        }
        // GET: Trang chu nha tuyen dung
        public IActionResult Index()
        {
            // TODO: Lay tin theo nha tuyen dung dang nhap
            var danhSachTinTuyenDung = _tinTuyenDungRepository.LayDanhSachTinTuyenDung();
            var danhSachTinUngTuyen = _tinUngTuyenRepository.LayDanhSachTinUngTuyen();
            
            // Thống kê
            var tongTinDaDang = danhSachTinTuyenDung.Count;
            var tongDonUngTuyen = danhSachTinUngTuyen.Count;
            var tongLuotXem = danhSachTinTuyenDung.Sum(t => t.SoLuongUngTuyen * 10); // Ước tính lượt xem
            var tongUngVienPhuHop = danhSachTinUngTuyen.Count(t => t.TrangThaiXuLy == "Tuyen dung" || t.TrangThaiXuLy == "Cho phong van");
            
            ViewBag.TongTinDaDang = tongTinDaDang;
            ViewBag.TongDonUngTuyen = tongDonUngTuyen;
            ViewBag.TongLuotXem = tongLuotXem;
            ViewBag.TongUngVienPhuHop = tongUngVienPhuHop;
            
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
            
            ViewBag.DanhSachTinhThanh = danhSachTinhThanh;
            return View();
        }

        // POST: Xu ly dang tin tuyen dung
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DangTinTuyenDung(TinTuyenDung model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Lấy thông tin công ty từ user đăng nhập (tạm thời dùng mock)
                    // TODO: Lấy từ User.Identity hoặc từ NhaTuyenDung table
                    model.CongTy = "Công ty của bạn"; // Sẽ thay bằng thông tin thực tế sau
                    
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
            
            ViewBag.DanhSachTinhThanh = danhSachTinhThanh;
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

        // GET: Danh sach tin da dang
        public IActionResult TinDaDang()
        {
            ViewData["Title"] = "Tin da dang";
            // TODO: Lay tin theo nha tuyen dung dang nhap
            var danhSach = _tinTuyenDungRepository.LayDanhSachTinTuyenDung();
            return View(danhSach);
        }

        // GET: Chi tiet tin da dang
        public IActionResult ChiTietTinDaDang(int id)
        {
            ViewData["Title"] = "Chi tiết tin đã đăng";
            
            var tinTuyenDung = _tinTuyenDungRepository.LayTinTuyenDungTheoId(id);
            if (tinTuyenDung == null)
            {
                TempData["Loi"] = "Không tìm thấy tin tuyển dụng!";
                return RedirectToAction("TinDaDang");
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
                return RedirectToAction("TinDaDang");
            }

            if (ModelState.IsValid)
            {
                try
                {
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
            
            ViewData["Title"] = "Chi tiết tin tuyển dụng";
            var danhSachUngTuyen = _tinUngTuyenRepository.LayDanhSachTinUngTuyen()
                .Where(t => t.MaTinTuyenDung == id.ToString())
                .ToList();
            
            // Lấy danh sách tỉnh/thành phố
            var danhSachTinhThanh = _context.Provinces
                .OrderBy(p => p.FullName)
                .ToList();
            
            ViewBag.DanhSachUngTuyen = danhSachUngTuyen;
            ViewBag.DanhSachTinhThanh = danhSachTinhThanh;
            return View("ChiTietTinDaDang", tinHienTai);
        }

        // GET: Danh sach don ung tuyen
        public IActionResult DonUngTuyen()
        {
            ViewData["Title"] = "Đơn ứng tuyển";
            // TODO: Lay don ung tuyen theo nha tuyen dung dang nhap
            var danhSachDonUngTuyen = _tinUngTuyenRepository.LayDanhSachTinUngTuyen();
            
            // Lấy thông tin tin tuyển dụng cho mỗi đơn ứng tuyển
            var danhSachTinTuyenDung = _tinTuyenDungRepository.LayDanhSachTinTuyenDung();
            ViewBag.DanhSachTinTuyenDung = danhSachTinTuyenDung;
            
            return View(danhSachDonUngTuyen);
        }

        // GET: Thong tin tai khoan (redirect to CaiDat)
        public IActionResult ThongTinTaiKhoan()
        {
            return RedirectToAction("CaiDat");
        }

        // GET: Thong ke
        public IActionResult ThongKe()
        {
            ViewData["Title"] = "Thống kê";
            // TODO: Lay tin theo nha tuyen dung dang nhap
            var danhSachTinTuyenDung = _tinTuyenDungRepository.LayDanhSachTinTuyenDung();
            var danhSachTinUngTuyen = _tinUngTuyenRepository.LayDanhSachTinUngTuyen();
            
            // Thống kê tổng quan
            var tongTinDaDang = danhSachTinTuyenDung.Count;
            var tongDonUngTuyen = danhSachTinUngTuyen.Count;
            var tongLuotXem = danhSachTinTuyenDung.Sum(t => t.SoLuongUngTuyen * 10);
            var tyLePhuHop = tongDonUngTuyen > 0 
                ? (danhSachTinUngTuyen.Count(t => t.TrangThaiXuLy == "Tuyen dung" || t.TrangThaiXuLy == "Cho phong van") * 100.0 / tongDonUngTuyen) 
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

        // POST: Cap nhat thong tin nha tuyen dung
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CapNhatThongTin(
            string tenCongTy, string email, string soDienThoai, string? website,
            string? tinhThanhPho, string? quanHuyen, string? diaChi,
            string? nguoiDaiDien, string? chucVu, string? soDienThoaiNguoiDaiDien,
            string? logo, string? moTa, string? latitude, string? longitude)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Json(new { success = false, message = "Không tìm thấy người dùng" });
            }

            try
            {
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
                        Logo = logo,
                        MoTa = moTa,
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
                    nhaTuyenDung.Logo = logo;
                    nhaTuyenDung.MoTa = moTa;
                    _context.NhaTuyenDungs.Update(nhaTuyenDung);
                }

                // Cập nhật thông tin user
                user.Email = email;
                user.PhoneNumber = soDienThoai;
                user.HoTen = nguoiDaiDien;
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
    }
}

