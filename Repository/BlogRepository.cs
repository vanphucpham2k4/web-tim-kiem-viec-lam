using Microsoft.EntityFrameworkCore;
using Unicareer.Data;
using Unicareer.Models;

namespace Unicareer.Repository
{
    public class BlogRepository : IBlogRepository
    {
        private readonly ApplicationDbContext _context;

        public BlogRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Blog> LayDanhSachBlog()
        {
            try
            {
                return _context.Blogs
                    .Include(b => b.User)
                    .Include(b => b.TheLoaiBlog)
                    .OrderByDescending(b => b.NgayDang)
                    .ToList();
            }
            catch (Exception)
            {
                return _context.Blogs
                    .Include(b => b.TheLoaiBlog)
                    .OrderByDescending(b => b.NgayDang)
                    .ToList();
            }
        }

        public List<Blog> LayDanhSachBlogHienThi()
        {
            try
            {
                return _context.Blogs
                    .Include(b => b.User)
                    .Include(b => b.TheLoaiBlog)
                    .Where(b => b.DaDang && b.HienThi)
                    .OrderByDescending(b => b.NgayDang)
                    .ToList();
            }
            catch (Exception)
            {
                return _context.Blogs
                    .Include(b => b.TheLoaiBlog)
                    .Where(b => b.DaDang && b.HienThi)
                    .OrderByDescending(b => b.NgayDang)
                    .ToList();
            }
        }

        public Blog? LayBlogTheoId(int id)
        {
            return _context.Blogs
                .Include(b => b.User)
                .Include(b => b.TheLoaiBlog)
                .FirstOrDefault(b => b.MaBlog == id);
        }

        public Blog? LayBlogTheoPermalink(string permalink)
        {
            return _context.Blogs
                .Include(b => b.User)
                .Include(b => b.TheLoaiBlog)
                .FirstOrDefault(b => b.Permalink == permalink);
        }

        public Blog? ThemBlog(Blog blog)
        {
            try
            {
                blog.NgayDang = DateTime.Now;
                blog.LuotXem = 0;
                _context.Blogs.Add(blog);
                _context.SaveChanges();
                return blog;
            }
            catch
            {
                return null;
            }
        }

        public Blog? CapNhatBlog(Blog blog)
        {
            try
            {
                var blogHienTai = _context.Blogs
                    .FirstOrDefault(b => b.MaBlog == blog.MaBlog);
                
                if (blogHienTai == null)
                    return null;

                blogHienTai.TieuDe = blog.TieuDe;
                blogHienTai.MoTaNgan = blog.MoTaNgan;
                blogHienTai.NoiDung = blog.NoiDung;
                if (!string.IsNullOrEmpty(blog.HinhAnh))
                {
                    blogHienTai.HinhAnh = blog.HinhAnh;
                }
                blogHienTai.TheLoai = blog.TheLoai;
                blogHienTai.TacGia = blog.TacGia;
                blogHienTai.MaTheLoai = blog.MaTheLoai;
                blogHienTai.TheLoai = blog.TheLoai; // Giữ lại để tương thích
                blogHienTai.Permalink = blog.Permalink;
                blogHienTai.Tags = blog.Tags;
                blogHienTai.IsPermalinkAuto = blog.IsPermalinkAuto;
                blogHienTai.DaDang = blog.DaDang;
                blogHienTai.HienThi = blog.HienThi;
                blogHienTai.NgayCapNhat = DateTime.Now;
                _context.SaveChanges();
                return blogHienTai;
            }
            catch
            {
                return null;
            }
        }

        public bool XoaBlog(int id)
        {
            try
            {
                var blog = _context.Blogs.FirstOrDefault(b => b.MaBlog == id);
                if (blog == null)
                    return false;

                _context.Blogs.Remove(blog);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool TangLuotXem(int id)
        {
            try
            {
                var blog = _context.Blogs.FirstOrDefault(b => b.MaBlog == id);
                if (blog == null)
                    return false;

                blog.LuotXem++;
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Blog? LayBlogTheoApiArticleId(string apiArticleId)
        {
            return _context.Blogs
                .FirstOrDefault(b => b.ApiArticleId == apiArticleId);
        }

        public List<Blog> LayDanhSachBlogChoDuyet()
        {
            try
            {
                return _context.Blogs
                    .Include(b => b.User)
                    .Include(b => b.TheLoaiBlog)
                    .Where(b => !b.DaDang && !string.IsNullOrEmpty(b.NguonBaiViet))
                    .OrderByDescending(b => b.NgayDang)
                    .ToList();
            }
            catch (Exception)
            {
                return _context.Blogs
                    .Include(b => b.TheLoaiBlog)
                    .Where(b => !b.DaDang && !string.IsNullOrEmpty(b.NguonBaiViet))
                    .OrderByDescending(b => b.NgayDang)
                    .ToList();
            }
        }
    }
}

