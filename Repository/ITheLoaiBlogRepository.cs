using Unicareer.Models;

namespace Unicareer.Repository
{
    public interface ITheLoaiBlogRepository
    {
        List<TheLoaiBlog> LayDanhSachTheLoai();
        List<TheLoaiBlog> LayDanhSachTheLoaiHienThi(); // Chỉ lấy thể loại đang hiển thị
        TheLoaiBlog? LayTheLoaiTheoId(int id);
        TheLoaiBlog? ThemTheLoai(TheLoaiBlog theLoai);
        TheLoaiBlog? CapNhatTheLoai(TheLoaiBlog theLoai);
        bool XoaTheLoai(int id);
    }
}

