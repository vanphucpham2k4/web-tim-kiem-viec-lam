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

        public NganhNghe? ThemNganhNghe(NganhNghe nganhNghe)
        {
            try
            {
                nganhNghe.NgayTao = DateTime.Now;
                nganhNghe.SoLuongCongViec = 0;
                _context.NganhNghes.Add(nganhNghe);
                _context.SaveChanges();
                return nganhNghe;
            }
            catch
            {
                return null;
            }
        }

        public NganhNghe? CapNhatNganhNghe(NganhNghe nganhNghe)
        {
            try
            {
                var nganhNgheHienTai = _context.NganhNghes
                    .FirstOrDefault(n => n.MaNganhNghe == nganhNghe.MaNganhNghe);
                
                if (nganhNgheHienTai == null)
                    return null;

                nganhNgheHienTai.TenNganhNghe = nganhNghe.TenNganhNghe;
                nganhNgheHienTai.MoTa = nganhNghe.MoTa;
                _context.SaveChanges();
                return nganhNgheHienTai;
            }
            catch
            {
                return null;
            }
        }

        public bool XoaNganhNghe(int id)
        {
            try
            {
                var nganhNghe = _context.NganhNghes
                    .FirstOrDefault(n => n.MaNganhNghe == id);
                
                if (nganhNghe == null)
                    return false;

                // Chỉ xóa ngành nghề, chuyên ngành vẫn tồn tại
                _context.NganhNghes.Remove(nganhNghe);
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

