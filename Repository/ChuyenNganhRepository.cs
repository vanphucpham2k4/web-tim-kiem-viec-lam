using Microsoft.EntityFrameworkCore;
using Unicareer.Data;
using Unicareer.Models;

namespace Unicareer.Repository
{
    public class ChuyenNganhRepository : IChuyenNganhRepository
    {
        private readonly ApplicationDbContext _context;

        public ChuyenNganhRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<ChuyenNganh> LayDanhSachChuyenNganh()
        {
            return _context.ChuyenNganhs
                .Include(c => c.NganhNghe)
                .OrderBy(c => c.NganhNghe != null ? c.NganhNghe.TenNganhNghe : "")
                .ThenBy(c => c.TenChuyenNganh)
                .ToList();
        }

        public List<ChuyenNganh> LayChuyenNganhTheoNganhNghe(int maNganhNghe)
        {
            return _context.ChuyenNganhs
                .Where(c => c.MaNganhNghe == maNganhNghe)
                .OrderBy(c => c.TenChuyenNganh)
                .ToList();
        }

        public ChuyenNganh? LayChuyenNganhTheoId(int id)
        {
            return _context.ChuyenNganhs
                .Include(c => c.NganhNghe)
                .FirstOrDefault(c => c.MaChuyenNganh == id);
        }

        public ChuyenNganh? ThemChuyenNganh(ChuyenNganh chuyenNganh)
        {
            try
            {
                chuyenNganh.NgayTao = DateTime.Now;
                _context.ChuyenNganhs.Add(chuyenNganh);
                _context.SaveChanges();
                return chuyenNganh;
            }
            catch
            {
                return null;
            }
        }

        public ChuyenNganh? CapNhatChuyenNganh(ChuyenNganh chuyenNganh)
        {
            try
            {
                _context.ChuyenNganhs.Update(chuyenNganh);
                _context.SaveChanges();
                return chuyenNganh;
            }
            catch
            {
                return null;
            }
        }

        public bool XoaChuyenNganh(int id)
        {
            try
            {
                var chuyenNganh = _context.ChuyenNganhs.Find(id);
                if (chuyenNganh == null)
                    return false;

                _context.ChuyenNganhs.Remove(chuyenNganh);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool XoaChuyenNganhTheoNganhNghe(int maNganhNghe)
        {
            try
            {
                var danhSachChuyenNganh = _context.ChuyenNganhs
                    .Where(c => c.MaNganhNghe == maNganhNghe)
                    .ToList();

                if (danhSachChuyenNganh.Any())
                {
                    _context.ChuyenNganhs.RemoveRange(danhSachChuyenNganh);
                    _context.SaveChanges();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

