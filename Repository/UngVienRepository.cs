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
                .ToList();
        }

        public UngVien? LayUngVienTheoId(int id)
        {
            return _context.UngViens
                .Include(u => u.User)
                .FirstOrDefault(u => u.MaUngVien == id);
        }
    }
}

