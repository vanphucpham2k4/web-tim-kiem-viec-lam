using Unicareer.Models;

namespace Unicareer.Repository
{
    public interface ITinUngTuyenRepository
    {
        List<TinUngTuyen> LayDanhSachTinUngTuyen();
        TinUngTuyen? LayTinUngTuyenTheoId(int id);
        TinUngTuyen ThemTinUngTuyen(TinUngTuyen tinUngTuyen);
    }
}

