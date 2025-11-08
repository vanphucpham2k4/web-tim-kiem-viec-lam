using Microsoft.EntityFrameworkCore;
using Unicareer.Data;
using Unicareer.Models;

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

        public TinTuyenDung ThemTinTuyenDung(TinTuyenDung tinTuyenDung)
        {
            tinTuyenDung.NgayDang = DateTime.Now;
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
    }
}

