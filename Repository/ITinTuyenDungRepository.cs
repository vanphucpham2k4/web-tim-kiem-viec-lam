using Unicareer.Models;

namespace Unicareer.Repository
{
    public interface ITinTuyenDungRepository
    {
        List<TinTuyenDung> LayDanhSachTinTuyenDung(bool onlyApproved = false);
        TinTuyenDung? LayTinTuyenDungTheoId(int id);
        List<TinTuyenDung> LayDanhSachThucTap();
        List<TinTuyenDung> LayDanhSachTheoCongTy(string tenCongTy);
        List<TinTuyenDung> LayDanhSachTheoMaNhaTuyenDung(int maNhaTuyenDung);
        TinTuyenDung ThemTinTuyenDung(TinTuyenDung tinTuyenDung);
        TinTuyenDung? CapNhatTinTuyenDung(int id, TinTuyenDung tinTuyenDung);
        TinTuyenDung? CapNhatTrangThai(int id, string trangThai);
        TinTuyenDung? CapNhatTrangThaiDuyet(int id, string trangThaiDuyet, string? lyDoTuChoi = null, DateTime? ngayDuyet = null);
        bool XoaTinTuyenDung(int id);
    }
}

