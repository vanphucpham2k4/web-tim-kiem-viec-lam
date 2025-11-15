using Unicareer.Models;

namespace Unicareer.Repository
{
    public interface IChuyenNganhRepository
    {
        List<ChuyenNganh> LayDanhSachChuyenNganh();
        List<ChuyenNganh> LayChuyenNganhTheoNganhNghe(int maNganhNghe);
        ChuyenNganh? LayChuyenNganhTheoId(int id);
        ChuyenNganh? ThemChuyenNganh(ChuyenNganh chuyenNganh);
        ChuyenNganh? CapNhatChuyenNganh(ChuyenNganh chuyenNganh);
        bool XoaChuyenNganh(int id);
        bool XoaChuyenNganhTheoNganhNghe(int maNganhNghe);
    }
}

