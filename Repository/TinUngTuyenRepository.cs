using Microsoft.EntityFrameworkCore;
using Unicareer.Data;
using Unicareer.Models;
using Unicareer.Models.Enums;

namespace Unicareer.Repository
{
    public class TinUngTuyenRepository : ITinUngTuyenRepository
    {
        private readonly ApplicationDbContext _context;

        public TinUngTuyenRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<TinUngTuyen> LayDanhSachTinUngTuyen()
        {
            return _context.TinUngTuyens
                .OrderByDescending(t => t.NgayUngTuyen)
                .ToList();
        }

        public TinUngTuyen? LayTinUngTuyenTheoId(int id)
        {
            return _context.TinUngTuyens
                .FirstOrDefault(t => t.MaTinUngTuyen == id);
        }

        public TinUngTuyen ThemTinUngTuyen(TinUngTuyen tinUngTuyen)
        {
            tinUngTuyen.NgayUngTuyen = DateTime.Now;
            if (string.IsNullOrEmpty(tinUngTuyen.TrangThaiXuLy))
            {
                tinUngTuyen.TrangThaiXuLy = TrangThaiXuLyHelper.ToString(TrangThaiXuLy.DangXemXet);
            }
            _context.TinUngTuyens.Add(tinUngTuyen);
            _context.SaveChanges();
            return tinUngTuyen;
        }
    }
}

