using Unicareer.Models;

namespace Unicareer.Repository
{
    public class MockViecLamDaLuuRepository : IViecLamDaLuuRepository
    {
        private static List<ViecLamDaLuu> _viecLamDaLuu = new List<ViecLamDaLuu>();

        public List<ViecLamDaLuu> LayDanhSachViecLamDaLuuTheoUserId(string userId)
        {
            return _viecLamDaLuu
                .Where(v => v.UserId == userId)
                .OrderByDescending(v => v.NgayLuu)
                .ToList();
        }

        public ViecLamDaLuu? LayViecLamDaLuuTheoId(int id)
        {
            return _viecLamDaLuu.FirstOrDefault(v => v.MaViecLamDaLuu == id);
        }

        public ViecLamDaLuu? LayViecLamDaLuuTheoUserIdVaMaTin(string userId, int maTinTuyenDung)
        {
            return _viecLamDaLuu
                .FirstOrDefault(v => v.UserId == userId && v.MaTinTuyenDung == maTinTuyenDung);
        }

        public bool KiemTraDaLuu(string userId, int maTinTuyenDung)
        {
            return _viecLamDaLuu
                .Any(v => v.UserId == userId && v.MaTinTuyenDung == maTinTuyenDung);
        }

        public ViecLamDaLuu LuuViecLam(string userId, int maTinTuyenDung)
        {
            var existing = LayViecLamDaLuuTheoUserIdVaMaTin(userId, maTinTuyenDung);
            if (existing != null)
            {
                return existing;
            }

            var maxId = _viecLamDaLuu.Any() ? _viecLamDaLuu.Max(v => v.MaViecLamDaLuu) : 0;
            var viecLamDaLuu = new ViecLamDaLuu
            {
                MaViecLamDaLuu = maxId + 1,
                UserId = userId,
                MaTinTuyenDung = maTinTuyenDung,
                NgayLuu = DateTime.Now
            };

            _viecLamDaLuu.Add(viecLamDaLuu);
            return viecLamDaLuu;
        }

        public bool BoLuuViecLam(string userId, int maTinTuyenDung)
        {
            var viecLamDaLuu = LayViecLamDaLuuTheoUserIdVaMaTin(userId, maTinTuyenDung);
            if (viecLamDaLuu == null)
            {
                return false;
            }

            _viecLamDaLuu.Remove(viecLamDaLuu);
            return true;
        }

        public bool XoaViecLamDaLuu(int id)
        {
            var viecLamDaLuu = LayViecLamDaLuuTheoId(id);
            if (viecLamDaLuu == null)
            {
                return false;
            }

            _viecLamDaLuu.Remove(viecLamDaLuu);
            return true;
        }
    }
}

