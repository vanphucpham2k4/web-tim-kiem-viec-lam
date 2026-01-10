using Microsoft.EntityFrameworkCore;
using Unicareer.Data;
using Unicareer.Models;

namespace Unicareer.Repository
{
    public class UngVienRepository : IUngVienRepository
    {
        private readonly ApplicationDbContext _context;

        public UngVienRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<UngVien> LayDanhSachUngVien()
        {
            return _context.UngViens
                .Include(u => u.User)
                .Include(u => u.ChuyenNganh)
                    .ThenInclude(c => c!.NganhNghe)
                .ToList();
        }

        public UngVien? LayUngVienTheoId(int id)
        {
            return _context.UngViens
                .Include(u => u.User)
                .Include(u => u.ChuyenNganh)
                    .ThenInclude(c => c!.NganhNghe)
                .FirstOrDefault(u => u.MaUngVien == id);
        }

        public UngVien? LayUngVienTheoUserId(string userId)
        {
            return _context.UngViens
                .Include(u => u.User)
                .Include(u => u.ChuyenNganh)
                    .ThenInclude(c => c!.NganhNghe)
                .FirstOrDefault(u => u.UserId == userId);
        }

        public UngVien? ThemUngVien(UngVien ungVien)
        {
            try
            {
                _context.UngViens.Add(ungVien);
                _context.SaveChanges();
                return ungVien;
            }
            catch
            {
                return null;
            }
        }

        public UngVien? CapNhatUngVien(UngVien ungVien)
        {
            try
            {
                _context.UngViens.Update(ungVien);
                _context.SaveChanges();
                return ungVien;
            }
            catch
            {
                return null;
            }
        }
    }
}

