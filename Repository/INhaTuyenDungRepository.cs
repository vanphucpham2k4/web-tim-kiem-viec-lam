using Unicareer.Models;

namespace Unicareer.Repository
{
    public interface INhaTuyenDungRepository
    {
        List<NhaTuyenDung> LayDanhSachNhaTuyenDung();
        NhaTuyenDung? LayNhaTuyenDungTheoId(int id);
        NhaTuyenDung? LayNhaTuyenDungTheoTen(string tenCongTy);
    }
}

