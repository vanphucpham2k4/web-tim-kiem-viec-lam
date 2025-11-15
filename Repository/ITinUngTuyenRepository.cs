using Unicareer.Models;

namespace Unicareer.Repository
{
    public interface ITinUngTuyenRepository
    {
        List<TinUngTuyen> LayDanhSachTinUngTuyen();
        List<TinUngTuyen> LayDanhSachTinUngTuyenTheoEmail(string email);
        List<TinUngTuyen> LayDanhSachTinUngTuyenTheoUserId(string userId);
        TinUngTuyen? LayTinUngTuyenTheoId(int id);
        TinUngTuyen ThemTinUngTuyen(TinUngTuyen tinUngTuyen);
        TinUngTuyen? CapNhatTrangThai(int id, string trangThai, string? ghiChu = null);
    }
}

