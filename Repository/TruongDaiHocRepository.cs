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

        public TruongDaiHoc? ThemTruongDaiHoc(TruongDaiHoc truongDaiHoc)
        {
            try
            {
                truongDaiHoc.NgayTao = DateTime.Now;
                _context.TruongDaiHocs.Add(truongDaiHoc);
                _context.SaveChanges();
                return truongDaiHoc;
            }
            catch
            {
                return null;
            }
        }

        public TruongDaiHoc? CapNhatTruongDaiHoc(TruongDaiHoc truongDaiHoc)
        {
            try
            {
                var truongHienTai = _context.TruongDaiHocs.Find(truongDaiHoc.MaTruong);
                if (truongHienTai == null)
                {
                    return null;
                }

                truongHienTai.TenTruong = truongDaiHoc.TenTruong;
                truongHienTai.MoTa = truongDaiHoc.MoTa;
                _context.SaveChanges();
                return truongHienTai;
            }
            catch
            {
                return null;
            }
        }

        public bool XoaTruongDaiHoc(int id)
        {
            try
            {
                var truongDaiHoc = _context.TruongDaiHocs.Find(id);
                if (truongDaiHoc == null)
                {
                    return false;
                }

                _context.TruongDaiHocs.Remove(truongDaiHoc);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

