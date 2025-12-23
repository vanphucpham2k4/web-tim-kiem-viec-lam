using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Unicareer.Data;
using Unicareer.Models;

namespace Unicareer.Repository
{
    public class DanhGiaCongTyRepository : IDanhGiaCongTyRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;
        private const int CACHE_EXPIRATION_MINUTES = 30;

        public DanhGiaCongTyRepository(ApplicationDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public DanhGiaCongTy? LayDanhGiaTheoId(int id)
        {
            return _context.DanhGiaCongTys
                .Include(d => d.User)
                .Include(d => d.NhaTuyenDung)
                .Include(d => d.TinUngTuyen)
                .FirstOrDefault(d => d.MaDanhGia == id);
        }

        public DanhGiaCongTy? LayDanhGiaTheoMaTinUngTuyen(int maTinUngTuyen)
        {
            return _context.DanhGiaCongTys
                .Include(d => d.User)
                .Include(d => d.NhaTuyenDung)
                .Include(d => d.TinUngTuyen)
                .FirstOrDefault(d => d.MaTinUngTuyen == maTinUngTuyen);
        }

        public List<DanhGiaCongTy> LayDanhSachDanhGiaTheoNhaTuyenDung(int maNhaTuyenDung, bool chiLayDaDuyet = true)
        {
            var query = _context.DanhGiaCongTys
                .Include(d => d.User)
                .Include(d => d.TinUngTuyen)
                .Where(d => d.MaNhaTuyenDung == maNhaTuyenDung);

            if (chiLayDaDuyet)
            {
                query = query.Where(d => d.TrangThai == "Da duyet");
            }

            return query
                .OrderByDescending(d => d.NgayTao)
                .ToList();
        }

        public List<DanhGiaCongTy> LayDanhSachDanhGiaTheoUserId(string userId)
        {
            return _context.DanhGiaCongTys
                .Include(d => d.NhaTuyenDung)
                .Include(d => d.TinUngTuyen)
                .Where(d => d.UserId == userId)
                .OrderByDescending(d => d.NgayTao)
                .ToList();
        }

        public DanhGiaCongTy ThemDanhGia(DanhGiaCongTy danhGia)
        {
            danhGia.NgayTao = DateTime.Now;
            _context.DanhGiaCongTys.Add(danhGia);
            _context.SaveChanges();
            
            // Xóa cache Trust Score khi có đánh giá mới
            XoaCacheTrustScore(danhGia.MaNhaTuyenDung);
            
            return danhGia;
        }

        public DanhGiaCongTy? CapNhatDanhGia(DanhGiaCongTy danhGia)
        {
            var existing = _context.DanhGiaCongTys.Find(danhGia.MaDanhGia);
            if (existing == null)
                return null;

            // Chỉ cho phép sửa trong 24h
            if ((DateTime.Now - existing.NgayTao).TotalHours > 24)
                return null;

            existing.DiemMinhBachLuong = danhGia.DiemMinhBachLuong;
            existing.DiemTocDoPhanHoi = danhGia.DiemTocDoPhanHoi;
            existing.DiemTonTrongUngVien = danhGia.DiemTonTrongUngVien;
            existing.Tags = danhGia.Tags;
            existing.NoiDung = danhGia.NoiDung;
            existing.IsAnDanh = danhGia.IsAnDanh;
            existing.NgayCapNhat = DateTime.Now;
            existing.TrangThai = "Cho duyet"; // Reset về chờ duyệt khi sửa

            _context.SaveChanges();
            
            // Xóa cache Trust Score khi có thay đổi
            XoaCacheTrustScore(existing.MaNhaTuyenDung);
            
            return existing;
        }

        public bool XoaDanhGia(int id)
        {
            var danhGia = _context.DanhGiaCongTys.Find(id);
            if (danhGia == null)
                return false;

            // Chỉ cho phép xóa trong 24h
            if ((DateTime.Now - danhGia.NgayTao).TotalHours > 24)
                return false;

            var maNhaTuyenDung = danhGia.MaNhaTuyenDung;
            _context.DanhGiaCongTys.Remove(danhGia);
            _context.SaveChanges();
            
            // Xóa cache Trust Score khi xóa đánh giá
            XoaCacheTrustScore(maNhaTuyenDung);
            
            return true;
        }

        public bool ThemPhanHoi(int maDanhGia, string phanHoi)
        {
            var danhGia = _context.DanhGiaCongTys.Find(maDanhGia);
            if (danhGia == null)
                return false;

            danhGia.PhanHoi = phanHoi;
            danhGia.NgayPhanHoi = DateTime.Now;
            _context.SaveChanges();
            return true;
        }

        public bool TangLuotThich(int maDanhGia, string userId)
        {
            var danhGia = _context.DanhGiaCongTys.Find(maDanhGia);
            if (danhGia == null)
                return false;

            // Kiểm tra user có phải là người đăng review không
            if (danhGia.UserId == userId)
                return false;

            // Kiểm tra user đã like chưa
            var daLike = _context.DanhGiaCongTyLikes
                .Any(l => l.MaDanhGia == maDanhGia && l.UserId == userId);
            
            if (daLike)
                return false;

            // Thêm like mới
            var like = new DanhGiaCongTyLike
            {
                MaDanhGia = maDanhGia,
                UserId = userId,
                NgayLike = DateTime.Now
            };
            _context.DanhGiaCongTyLikes.Add(like);

            // Tăng số lượt thích
            danhGia.SoLuotThich++;
            _context.SaveChanges();
            return true;
        }

        public bool DaLike(int maDanhGia, string userId)
        {
            return _context.DanhGiaCongTyLikes
                .Any(l => l.MaDanhGia == maDanhGia && l.UserId == userId);
        }

        public double TinhTrustScore(int maNhaTuyenDung)
        {
            var cacheKey = $"TrustScore_{maNhaTuyenDung}";
            
            // Thử lấy từ cache
            if (_cache.TryGetValue(cacheKey, out double cachedScore))
            {
                return cachedScore;
            }

            // Tính toán Trust Score
            var danhSach = _context.DanhGiaCongTys
                .Where(d => d.MaNhaTuyenDung == maNhaTuyenDung && d.TrangThai == "Da duyet")
                .ToList();

            double score = 0;
            if (danhSach.Any())
            {
                score = Math.Round((
                    danhSach.Average(d => d.DiemMinhBachLuong) +
                    danhSach.Average(d => d.DiemTocDoPhanHoi) +
                    danhSach.Average(d => d.DiemTonTrongUngVien)
                ) / 3.0, 1);
            }

            // Lưu vào cache với thời gian hết hạn
            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(CACHE_EXPIRATION_MINUTES),
                SlidingExpiration = TimeSpan.FromMinutes(10)
            };
            _cache.Set(cacheKey, score, cacheOptions);

            return score;
        }

        /// <summary>
        /// Xóa cache Trust Score khi có thay đổi
        /// </summary>
        public void XoaCacheTrustScore(int maNhaTuyenDung)
        {
            var cacheKey = $"TrustScore_{maNhaTuyenDung}";
            _cache.Remove(cacheKey);
        }

        public double? TinhTyLePhanHoi(int maNhaTuyenDung)
        {
            // Logic này cần dữ liệu từ TinUngTuyen để tính thời gian phản hồi
            // Tạm thời return null, sẽ implement sau
            return null;
        }

        public bool DaDanhGia(int maTinUngTuyen)
        {
            return _context.DanhGiaCongTys
                .Any(d => d.MaTinUngTuyen == maTinUngTuyen);
        }
    }
}
