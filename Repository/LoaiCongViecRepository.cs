using Microsoft.EntityFrameworkCore;
using Unicareer.Data;
using Unicareer.Models;

namespace Unicareer.Repository
{
    public class LoaiCongViecRepository : ILoaiCongViecRepository
    {
        private readonly ApplicationDbContext _context;

        public LoaiCongViecRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<LoaiCongViec> LayDanhSachLoaiCongViec()
        {
            return _context.LoaiCongViecs
                .OrderBy(l => l.TenLoaiCongViec)
                .ToList();
        }

        public LoaiCongViec? LayLoaiCongViecTheoId(int id)
        {
            return _context.LoaiCongViecs
                .FirstOrDefault(l => l.MaLoaiCongViec == id);
        }
    }
}

