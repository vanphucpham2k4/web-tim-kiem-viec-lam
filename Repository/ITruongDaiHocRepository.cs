using Unicareer.Models;

namespace Unicareer.Repository
{
    public interface ITruongDaiHocRepository
    {
        List<TruongDaiHoc> LayDanhSachTruongDaiHoc();
        TruongDaiHoc? LayTruongDaiHocTheoId(int id);
    }
}

