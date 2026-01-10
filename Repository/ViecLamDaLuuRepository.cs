using Microsoft.EntityFrameworkCore;
using Unicareer.Data;
using Unicareer.Models;

namespace Unicareer.Repository
{
    public class ViecLamDaLuuRepository : IViecLamDaLuuRepository
    {
        private readonly ApplicationDbContext _context;

        public ViecLamDaLuuRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<ViecLamDaLuu> LayDanhSachViecLamDaLuuTheoUserId(string userId)
        {
            return _context.ViecLamDaLuus
                .Include(v => v.TinTuyenDung)
                .Where(v => v.UserId == userId)
                .OrderByDescending(v => v.NgayLuu)
                .ToList();
        }

        public ViecLamDaLuu? LayViecLamDaLuuTheoId(int id)
        {
            return _context.ViecLamDaLuus
                .Include(v => v.TinTuyenDung)
                .FirstOrDefault(v => v.MaViecLamDaLuu == id);
        }

        public ViecLamDaLuu? LayViecLamDaLuuTheoUserIdVaMaTin(string userId, int maTinTuyenDung)
        {
            return _context.ViecLamDaLuus
                .Include(v => v.TinTuyenDung)
                .FirstOrDefault(v => v.UserId == userId && v.MaTinTuyenDung == maTinTuyenDung);
        }

        public bool KiemTraDaLuu(string userId, int maTinTuyenDung)
        {
            return _context.ViecLamDaLuus
                .Any(v => v.UserId == userId && v.MaTinTuyenDung == maTinTuyenDung);
        }

        public ViecLamDaLuu LuuViecLam(string userId, int maTinTuyenDung)
        {
            // Kiểm tra xem đã lưu chưa
            var existing = LayViecLamDaLuuTheoUserIdVaMaTin(userId, maTinTuyenDung);
            if (existing != null)
            {
                return existing; // Đã lưu rồi, trả về bản ghi hiện có
            }

            var viecLamDaLuu = new ViecLamDaLuu
            {
                UserId = userId,
                MaTinTuyenDung = maTinTuyenDung,
                NgayLuu = DateTime.Now
            };

            _context.ViecLamDaLuus.Add(viecLamDaLuu);
            _context.SaveChanges();
            return viecLamDaLuu;
        }

        public bool BoLuuViecLam(string userId, int maTinTuyenDung)
        {
            var viecLamDaLuu = LayViecLamDaLuuTheoUserIdVaMaTin(userId, maTinTuyenDung);
            if (viecLamDaLuu == null)
            {
                return false; // Không tìm thấy bản ghi
            }

            _context.ViecLamDaLuus.Remove(viecLamDaLuu);
            _context.SaveChanges();
            return true;
        }

        public bool XoaViecLamDaLuu(int id)
        {
            var viecLamDaLuu = LayViecLamDaLuuTheoId(id);
            if (viecLamDaLuu == null)
            {
                return false;
            }

            _context.ViecLamDaLuus.Remove(viecLamDaLuu);
            _context.SaveChanges();
            return true;
        }
    }
}

