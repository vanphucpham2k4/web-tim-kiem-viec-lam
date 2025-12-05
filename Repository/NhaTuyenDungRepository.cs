using Microsoft.EntityFrameworkCore;
using Unicareer.Data;
using Unicareer.Models;

namespace Unicareer.Repository
{
    public class NhaTuyenDungRepository : INhaTuyenDungRepository
    {
        private readonly ApplicationDbContext _context;

        public NhaTuyenDungRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<NhaTuyenDung> LayDanhSachNhaTuyenDung()
        {
            return _context.NhaTuyenDungs
                .Include(n => n.User)
                .ToList();
        }

        public NhaTuyenDung? LayNhaTuyenDungTheoId(int id)
        {
            return _context.NhaTuyenDungs
                .Include(n => n.User)
                .FirstOrDefault(n => n.MaNhaTuyenDung == id);
        }

        public NhaTuyenDung? LayNhaTuyenDungTheoTen(string tenCongTy)
        {
            return _context.NhaTuyenDungs
                .Include(n => n.User)
                .FirstOrDefault(n => n.TenCongTy != null && n.TenCongTy.ToLower() == tenCongTy.ToLower());
        }

        public NhaTuyenDung? LayNhaTuyenDungTheoUserId(string userId)
        {
            return _context.NhaTuyenDungs
                .Include(n => n.User)
                .FirstOrDefault(n => n.UserId == userId);
        }

        public NhaTuyenDung? ThemNhaTuyenDung(NhaTuyenDung nhaTuyenDung)
        {
            try
            {
                _context.NhaTuyenDungs.Add(nhaTuyenDung);
                _context.SaveChanges();
                return nhaTuyenDung;
            }
            catch
            {
                return null;
            }
        }

        public NhaTuyenDung? CapNhatNhaTuyenDung(NhaTuyenDung nhaTuyenDung)
        {
            try
            {
                _context.NhaTuyenDungs.Update(nhaTuyenDung);
                _context.SaveChanges();
                return nhaTuyenDung;
            }
            catch
            {
                return null;
            }
        }
    }
}

