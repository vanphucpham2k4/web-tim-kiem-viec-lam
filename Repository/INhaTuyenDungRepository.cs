using Unicareer.Models;

namespace Unicareer.Repository
{
    public interface INhaTuyenDungRepository
    {
        List<NhaTuyenDung> LayDanhSachNhaTuyenDung();
        NhaTuyenDung? LayNhaTuyenDungTheoId(int id);
        NhaTuyenDung? LayNhaTuyenDungTheoTen(string tenCongTy);
        NhaTuyenDung? LayNhaTuyenDungTheoUserId(string userId);
        NhaTuyenDung? CapNhatNhaTuyenDung(NhaTuyenDung nhaTuyenDung);
    }
}

