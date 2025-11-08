using Microsoft.EntityFrameworkCore;
using Unicareer.Data;
using Unicareer.Models;

namespace Unicareer.Repository
{
    public class NganhNgheRepository : INganhNgheRepository
    {
        private readonly ApplicationDbContext _context;

        public NganhNgheRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<NganhNghe> LayDanhSachNganhNghe()
        {
            return _context.NganhNghes
                .OrderBy(n => n.TenNganhNghe)
                .ToList();
        }

        public NganhNghe? LayNganhNgheTheoId(int id)
        {
            return _context.NganhNghes
                .FirstOrDefault(n => n.MaNganhNghe == id);
        }
    }
}

