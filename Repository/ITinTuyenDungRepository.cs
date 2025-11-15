using Unicareer.Models;

namespace Unicareer.Repository
{
    public interface ITinTuyenDungRepository
    {
        List<TinTuyenDung> LayDanhSachTinTuyenDung();
        TinTuyenDung? LayTinTuyenDungTheoId(int id);
        List<TinTuyenDung> LayDanhSachThucTap();
        List<TinTuyenDung> LayDanhSachTheoCongTy(string tenCongTy);
        List<TinTuyenDung> LayDanhSachTheoMaNhaTuyenDung(int maNhaTuyenDung);
        TinTuyenDung ThemTinTuyenDung(TinTuyenDung tinTuyenDung);
        TinTuyenDung? CapNhatTinTuyenDung(int id, TinTuyenDung tinTuyenDung);
        TinTuyenDung? CapNhatTrangThai(int id, string trangThai);
        bool XoaTinTuyenDung(int id);
    }
}

