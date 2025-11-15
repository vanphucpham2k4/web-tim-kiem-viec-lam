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

        public LoaiCongViec? ThemLoaiCongViec(LoaiCongViec loaiCongViec)
        {
            try
            {
                loaiCongViec.NgayTao = DateTime.Now;
                loaiCongViec.SoLuongViTri = 0;
                _context.LoaiCongViecs.Add(loaiCongViec);
                _context.SaveChanges();
                return loaiCongViec;
            }
            catch
            {
                return null;
            }
        }

        public LoaiCongViec? CapNhatLoaiCongViec(LoaiCongViec loaiCongViec)
        {
            try
            {
                var loaiCongViecHienTai = _context.LoaiCongViecs
                    .FirstOrDefault(l => l.MaLoaiCongViec == loaiCongViec.MaLoaiCongViec);
                
                if (loaiCongViecHienTai == null)
                    return null;

                loaiCongViecHienTai.TenLoaiCongViec = loaiCongViec.TenLoaiCongViec;
                loaiCongViecHienTai.MoTa = loaiCongViec.MoTa;
                _context.SaveChanges();
                return loaiCongViecHienTai;
            }
            catch
            {
                return null;
            }
        }

        public bool XoaLoaiCongViec(int id)
        {
            try
            {
                var loaiCongViec = _context.LoaiCongViecs
                    .FirstOrDefault(l => l.MaLoaiCongViec == id);
                
                if (loaiCongViec == null)
                    return false;

                _context.LoaiCongViecs.Remove(loaiCongViec);
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

