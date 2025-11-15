using Unicareer.Models;

namespace Unicareer.Repository
{
    public interface INganhNgheRepository
    {
        List<NganhNghe> LayDanhSachNganhNghe();
        NganhNghe? LayNganhNgheTheoId(int id);
        NganhNghe? ThemNganhNghe(NganhNghe nganhNghe);
        NganhNghe? CapNhatNganhNghe(NganhNghe nganhNghe);
        bool XoaNganhNghe(int id);
    }
}

