using Unicareer.Models;

namespace Unicareer.Repository
{
    public interface IUngVienRepository
    {
        List<UngVien> LayDanhSachUngVien();
        UngVien? LayUngVienTheoId(int id);
    }
}

