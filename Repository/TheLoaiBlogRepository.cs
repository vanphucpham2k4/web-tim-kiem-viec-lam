using Microsoft.EntityFrameworkCore;
using Unicareer.Data;
using Unicareer.Models;

namespace Unicareer.Repository
{
    public class TheLoaiBlogRepository : ITheLoaiBlogRepository
    {
        private readonly ApplicationDbContext _context;

        public TheLoaiBlogRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<TheLoaiBlog> LayDanhSachTheLoai()
        {
            return _context.TheLoaiBlogs
                .OrderBy(t => t.ThuTu)
                .ThenBy(t => t.TenTheLoai)
                .ToList();
        }

        public List<TheLoaiBlog> LayDanhSachTheLoaiHienThi()
        {
            return _context.TheLoaiBlogs
                .Where(t => t.HienThi)
                .OrderBy(t => t.ThuTu)
                .ThenBy(t => t.TenTheLoai)
                .ToList();
        }

        public TheLoaiBlog? LayTheLoaiTheoId(int id)
        {
            return _context.TheLoaiBlogs
                .FirstOrDefault(t => t.MaTheLoai == id);
        }

        public TheLoaiBlog? ThemTheLoai(TheLoaiBlog theLoai)
        {
            try
            {
                theLoai.NgayTao = DateTime.Now;
                theLoai.SoLuongBlog = 0;
                _context.TheLoaiBlogs.Add(theLoai);
                _context.SaveChanges();
                return theLoai;
            }
            catch
            {
                return null;
            }
        }

        public TheLoaiBlog? CapNhatTheLoai(TheLoaiBlog theLoai)
        {
            try
            {
                var theLoaiHienTai = _context.TheLoaiBlogs
                    .FirstOrDefault(t => t.MaTheLoai == theLoai.MaTheLoai);
                
                if (theLoaiHienTai == null)
                    return null;

                theLoaiHienTai.TenTheLoai = theLoai.TenTheLoai;
                // Các trường khác giữ nguyên (không cập nhật)
                _context.SaveChanges();
                return theLoaiHienTai;
            }
            catch
            {
                return null;
            }
        }

        public bool XoaTheLoai(int id)
        {
            try
            {
                var theLoai = _context.TheLoaiBlogs.FirstOrDefault(t => t.MaTheLoai == id);
                if (theLoai == null)
                    return false;

                // Kiểm tra xem có blog nào đang sử dụng thể loại này không
                var soLuongBlog = _context.Blogs.Count(b => b.MaTheLoai == id);
                if (soLuongBlog > 0)
                {
                    return false; // Không cho phép xóa nếu còn blog đang sử dụng
                }

                _context.TheLoaiBlogs.Remove(theLoai);
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

