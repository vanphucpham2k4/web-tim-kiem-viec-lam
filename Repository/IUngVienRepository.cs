using Unicareer.Models;

namespace Unicareer.Repository
{
    public interface IUngVienRepository
    {
        List<UngVien> LayDanhSachUngVien();
        UngVien? LayUngVienTheoId(int id);
        UngVien? LayUngVienTheoUserId(string userId);
        UngVien? ThemUngVien(UngVien ungVien);
        UngVien? CapNhatUngVien(UngVien ungVien);
    }
}

