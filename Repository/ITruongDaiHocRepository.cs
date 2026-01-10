using Unicareer.Models;

namespace Unicareer.Repository
{
    public interface ITruongDaiHocRepository
    {
        List<TruongDaiHoc> LayDanhSachTruongDaiHoc();
        TruongDaiHoc? LayTruongDaiHocTheoId(int id);
        TruongDaiHoc? ThemTruongDaiHoc(TruongDaiHoc truongDaiHoc);
        TruongDaiHoc? CapNhatTruongDaiHoc(TruongDaiHoc truongDaiHoc);
        bool XoaTruongDaiHoc(int id);
    }
}

