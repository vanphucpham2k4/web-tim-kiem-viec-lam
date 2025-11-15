using Microsoft.EntityFrameworkCore;
using Unicareer.Data;
using Unicareer.Models;
using System.IO;

namespace Unicareer.Repository
{
    public class TinTuyenDungRepository : ITinTuyenDungRepository
    {
        private readonly ApplicationDbContext _context;

        public TinTuyenDungRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<TinTuyenDung> LayDanhSachTinTuyenDung()
        {
            return _context.TinTuyenDungs
                .OrderByDescending(t => t.NgayDang)
                .ToList();
        }

        public TinTuyenDung? LayTinTuyenDungTheoId(int id)
        {
            return _context.TinTuyenDungs
                .FirstOrDefault(t => t.MaTinTuyenDung == id);
        }

        public List<TinTuyenDung> LayDanhSachThucTap()
        {
            return _context.TinTuyenDungs
                .Where(t => t.LoaiCongViec != null && t.LoaiCongViec.ToLower().Contains("thực tập"))
                .OrderByDescending(t => t.NgayDang)
                .ToList();
        }

        public List<TinTuyenDung> LayDanhSachTheoCongTy(string tenCongTy)
        {
            return _context.TinTuyenDungs
                .Where(t => t.CongTy != null && t.CongTy.ToLower() == tenCongTy.ToLower())
                .OrderByDescending(t => t.NgayDang)
                .ToList();
        }

        public List<TinTuyenDung> LayDanhSachTheoMaNhaTuyenDung(int maNhaTuyenDung)
        {
            return _context.TinTuyenDungs
                .Where(t => t.MaNhaTuyenDung == maNhaTuyenDung)
                .OrderByDescending(t => t.NgayDang)
                .ToList();
        }

        public TinTuyenDung ThemTinTuyenDung(TinTuyenDung tinTuyenDung)
        {
            tinTuyenDung.NgayDang = DateTime.Now;
            // Set trạng thái mặc định nếu chưa có
            if (string.IsNullOrEmpty(tinTuyenDung.TrangThai))
            {
                tinTuyenDung.TrangThai = "Dang tuyen";
            }
            _context.TinTuyenDungs.Add(tinTuyenDung);
            _context.SaveChanges();
            return tinTuyenDung;
        }

        public TinTuyenDung? CapNhatTinTuyenDung(int id, TinTuyenDung tinTuyenDung)
        {
            var tinHienTai = _context.TinTuyenDungs.Find(id);
            if (tinHienTai == null)
            {
                return null;
            }

            // Cập nhật các trường
            tinHienTai.TenViecLam = tinTuyenDung.TenViecLam;
            tinHienTai.NganhNghe = tinTuyenDung.NganhNghe;
            tinHienTai.NganhNgheChiTiet = tinTuyenDung.NganhNgheChiTiet;
            tinHienTai.LoaiCongViec = tinTuyenDung.LoaiCongViec;
            tinHienTai.KinhNghiem = tinTuyenDung.KinhNghiem;
            tinHienTai.ViTri = tinTuyenDung.ViTri;
            tinHienTai.NgoaiNgu = tinTuyenDung.NgoaiNgu;
            tinHienTai.KyNang = tinTuyenDung.KyNang;
            tinHienTai.MoTa = tinTuyenDung.MoTa;
            tinHienTai.YeuCau = tinTuyenDung.YeuCau;
            tinHienTai.QuyenLoi = tinTuyenDung.QuyenLoi;
            tinHienTai.MucLuongThapNhat = tinTuyenDung.MucLuongThapNhat;
            tinHienTai.MucLuongCaoNhat = tinTuyenDung.MucLuongCaoNhat;
            tinHienTai.DiaChiLamViec = tinTuyenDung.DiaChiLamViec;
            tinHienTai.PhuongXa = tinTuyenDung.PhuongXa;
            tinHienTai.TinhThanhPho = tinTuyenDung.TinhThanhPho;
            tinHienTai.NguoiLienHe = tinTuyenDung.NguoiLienHe;
            tinHienTai.EmailLienHe = tinTuyenDung.EmailLienHe;
            tinHienTai.SDTLienHe = tinTuyenDung.SDTLienHe;
            tinHienTai.TuKhoa = tinTuyenDung.TuKhoa;
            tinHienTai.HanNop = tinTuyenDung.HanNop;
            tinHienTai.Latitude = tinTuyenDung.Latitude;
            tinHienTai.Longitude = tinTuyenDung.Longitude;
            tinHienTai.AnhVanPhong = tinTuyenDung.AnhVanPhong;

            _context.TinTuyenDungs.Update(tinHienTai);
            _context.SaveChanges();
            return tinHienTai;
        }

        public TinTuyenDung? CapNhatTrangThai(int id, string trangThai)
        {
            var tinTuyenDung = _context.TinTuyenDungs.Find(id);
            if (tinTuyenDung == null)
            {
                return null;
            }

            tinTuyenDung.TrangThai = trangThai;
            _context.TinTuyenDungs.Update(tinTuyenDung);
            _context.SaveChanges();
            return tinTuyenDung;
        }

        public bool XoaTinTuyenDung(int id)
        {
            var tinTuyenDung = _context.TinTuyenDungs.Find(id);
            if (tinTuyenDung == null)
            {
                return false;
            }

            // Xóa các đơn ứng tuyển liên quan
            var danhSachTinUngTuyen = _context.TinUngTuyens
                .Where(t => t.MaTinTuyenDung == id.ToString())
                .ToList();
            
            if (danhSachTinUngTuyen.Any())
            {
                _context.TinUngTuyens.RemoveRange(danhSachTinUngTuyen);
            }

            // Xóa ảnh văn phòng nếu có
            if (!string.IsNullOrEmpty(tinTuyenDung.AnhVanPhong))
            {
                try
                {
                    var danhSachAnh = new List<string>();
                    if (tinTuyenDung.AnhVanPhong.Trim().StartsWith("["))
                    {
                        var jsonArray = System.Text.Json.JsonSerializer.Deserialize<string[]>(tinTuyenDung.AnhVanPhong);
                        if (jsonArray != null)
                        {
                            danhSachAnh = jsonArray.ToList();
                        }
                    }
                    else
                    {
                        danhSachAnh = tinTuyenDung.AnhVanPhong
                            .Split(',', StringSplitOptions.RemoveEmptyEntries)
                            .Select(s => s.Trim())
                            .Where(s => !string.IsNullOrEmpty(s))
                            .ToList();
                    }

                    // Xóa các file ảnh
                    foreach (var anhPath in danhSachAnh)
                    {
                        if (!string.IsNullOrEmpty(anhPath) && anhPath.StartsWith("/uploads/"))
                        {
                            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", anhPath.TrimStart('/'));
                            if (System.IO.File.Exists(filePath))
                            {
                                System.IO.File.Delete(filePath);
                            }
                        }
                    }
                }
                catch
                {
                    // Bỏ qua lỗi khi xóa file
                }
            }

            // Xóa tin tuyển dụng
            _context.TinTuyenDungs.Remove(tinTuyenDung);
            _context.SaveChanges();
            return true;
        }
    }
}

