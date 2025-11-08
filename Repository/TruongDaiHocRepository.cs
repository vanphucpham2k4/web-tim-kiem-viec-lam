using Microsoft.EntityFrameworkCore;
using Unicareer.Data;
using Unicareer.Models;

namespace Unicareer.Repository
{
    public class TruongDaiHocRepository : ITruongDaiHocRepository
    {
        private readonly ApplicationDbContext _context;

        public TruongDaiHocRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<TruongDaiHoc> LayDanhSachTruongDaiHoc()
        {
            return _context.TruongDaiHocs
                .OrderBy(t => t.TenTruong)
                .ToList();
        }

        public TruongDaiHoc? LayTruongDaiHocTheoId(int id)
        {
            return _context.TruongDaiHocs
                .FirstOrDefault(t => t.MaTruong == id);
        }
    }
}

