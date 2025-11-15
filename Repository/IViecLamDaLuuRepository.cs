using Unicareer.Models;

namespace Unicareer.Repository
{
    public interface IViecLamDaLuuRepository
    {
        List<ViecLamDaLuu> LayDanhSachViecLamDaLuuTheoUserId(string userId);
        ViecLamDaLuu? LayViecLamDaLuuTheoId(int id);
        ViecLamDaLuu? LayViecLamDaLuuTheoUserIdVaMaTin(string userId, int maTinTuyenDung);
        bool KiemTraDaLuu(string userId, int maTinTuyenDung);
        ViecLamDaLuu LuuViecLam(string userId, int maTinTuyenDung);
        bool BoLuuViecLam(string userId, int maTinTuyenDung);
        bool XoaViecLamDaLuu(int id);
    }
}

