using Unicareer.Models;

namespace Unicareer.Repository
{
    public interface IBlogRepository
    {
        List<Blog> LayDanhSachBlog();
        List<Blog> LayDanhSachBlogHienThi(); // Chỉ lấy blog đã duyệt và hiển thị
        Blog? LayBlogTheoId(int id);
        Blog? LayBlogTheoPermalink(string permalink); // Lấy blog theo permalink
        Blog? ThemBlog(Blog blog);
        Blog? CapNhatBlog(Blog blog);
        bool XoaBlog(int id);
        bool TangLuotXem(int id);
    }
}

