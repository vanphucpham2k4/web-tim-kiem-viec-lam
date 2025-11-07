using Unicareer.Models;

namespace Unicareer.Repository
{
    public interface INganhNgheRepository
    {
        List<NganhNghe> LayDanhSachNganhNghe();
        NganhNghe? LayNganhNgheTheoId(int id);
    }
}

