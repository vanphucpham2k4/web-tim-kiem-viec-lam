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
                .Include(t => t.TruongDaiHoc)
                .OrderByDescending(t => t.NgayUngTuyen)
                .ToList();
        }

        public List<TinUngTuyen> LayDanhSachTinUngTuyenTheoEmail(string email)
        {
            return _context.TinUngTuyens
                .Include(t => t.TruongDaiHoc)
                .Where(t => t.Email.ToLower() == email.ToLower())
                .OrderByDescending(t => t.NgayUngTuyen)
                .ToList();
        }

        public List<TinUngTuyen> LayDanhSachTinUngTuyenTheoUserId(string userId)
        {
            return _context.TinUngTuyens
                .Include(t => t.TruongDaiHoc)
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.NgayUngTuyen)
                .ToList();
        }

        public TinUngTuyen? LayTinUngTuyenTheoId(int id)
        {
            return _context.TinUngTuyens
                .Include(t => t.TruongDaiHoc)
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

        public TinUngTuyen? CapNhatTrangThai(int id, string trangThai, string? ghiChu = null)
        {
            var tinUngTuyen = _context.TinUngTuyens.Find(id);
            if (tinUngTuyen == null)
            {
                return null;
            }

            tinUngTuyen.TrangThaiXuLy = trangThai;
            
            // Cập nhật ghi chú nếu có
            if (!string.IsNullOrEmpty(ghiChu))
            {
                // Nếu đã có ghi chú, thêm ghi chú mới vào cuối
                if (!string.IsNullOrEmpty(tinUngTuyen.GhiChu))
                {
                    tinUngTuyen.GhiChu = $"{tinUngTuyen.GhiChu}; {ghiChu}";
                }
                else
                {
                    tinUngTuyen.GhiChu = ghiChu;
                }
            }

            _context.TinUngTuyens.Update(tinUngTuyen);
            _context.SaveChanges();
            return tinUngTuyen;
        }

        public bool XoaTinUngTuyen(int id)
        {
            var tinUngTuyen = _context.TinUngTuyens.Find(id);
            if (tinUngTuyen == null)
            {
                return false;
            }

            _context.TinUngTuyens.Remove(tinUngTuyen);
            _context.SaveChanges();
            return true;
        }
    }
}

