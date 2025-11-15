using Unicareer.Models;

namespace Unicareer.Repository
{
    public class MockChuyenNganhRepository : IChuyenNganhRepository
    {
        private readonly List<ChuyenNganh> _chuyenNganhs;
        private readonly List<NganhNghe> _nganhNghes;

        public MockChuyenNganhRepository()
        {
            // Mock data cho ngành nghề
            _nganhNghes = new List<NganhNghe>
            {
                new NganhNghe { MaNganhNghe = 1, TenNganhNghe = "Công nghệ thông tin", NgayTao = DateTime.Now },
                new NganhNghe { MaNganhNghe = 2, TenNganhNghe = "Kinh doanh", NgayTao = DateTime.Now },
                new NganhNghe { MaNganhNghe = 3, TenNganhNghe = "Kế toán - Tài chính", NgayTao = DateTime.Now }
            };

            // Mock data cho chuyên ngành
            _chuyenNganhs = new List<ChuyenNganh>
            {
                // CNTT
                new ChuyenNganh { MaChuyenNganh = 1, TenChuyenNganh = "Kỹ thuật phần mềm", MaNganhNghe = 1, NgayTao = DateTime.Now, IsActive = true },
                new ChuyenNganh { MaChuyenNganh = 2, TenChuyenNganh = "An toàn thông tin", MaNganhNghe = 1, NgayTao = DateTime.Now, IsActive = true },
                new ChuyenNganh { MaChuyenNganh = 3, TenChuyenNganh = "Khoa học máy tính", MaNganhNghe = 1, NgayTao = DateTime.Now, IsActive = true },
                new ChuyenNganh { MaChuyenNganh = 4, TenChuyenNganh = "Mạng máy tính", MaNganhNghe = 1, NgayTao = DateTime.Now, IsActive = true },
                // Kinh doanh
                new ChuyenNganh { MaChuyenNganh = 5, TenChuyenNganh = "Quản trị kinh doanh", MaNganhNghe = 2, NgayTao = DateTime.Now, IsActive = true },
                new ChuyenNganh { MaChuyenNganh = 6, TenChuyenNganh = "Marketing", MaNganhNghe = 2, NgayTao = DateTime.Now, IsActive = true },
                new ChuyenNganh { MaChuyenNganh = 7, TenChuyenNganh = "Thương mại điện tử", MaNganhNghe = 2, NgayTao = DateTime.Now, IsActive = true },
                // Kế toán - Tài chính
                new ChuyenNganh { MaChuyenNganh = 8, TenChuyenNganh = "Kế toán", MaNganhNghe = 3, NgayTao = DateTime.Now, IsActive = true },
                new ChuyenNganh { MaChuyenNganh = 9, TenChuyenNganh = "Kiểm toán", MaNganhNghe = 3, NgayTao = DateTime.Now, IsActive = true },
                new ChuyenNganh { MaChuyenNganh = 10, TenChuyenNganh = "Tài chính ngân hàng", MaNganhNghe = 3, NgayTao = DateTime.Now, IsActive = true }
            };
        }

        public List<ChuyenNganh> LayDanhSachChuyenNganh()
        {
            return _chuyenNganhs
                .OrderBy(c => c.MaNganhNghe)
                .ThenBy(c => c.TenChuyenNganh)
                .ToList();
        }

        public List<ChuyenNganh> LayChuyenNganhTheoNganhNghe(int maNganhNghe)
        {
            return _chuyenNganhs
                .Where(c => c.MaNganhNghe == maNganhNghe)
                .OrderBy(c => c.TenChuyenNganh)
                .ToList();
        }

        public ChuyenNganh? LayChuyenNganhTheoId(int id)
        {
            return _chuyenNganhs.FirstOrDefault(c => c.MaChuyenNganh == id);
        }

        public ChuyenNganh? ThemChuyenNganh(ChuyenNganh chuyenNganh)
        {
            var maxId = _chuyenNganhs.Any() ? _chuyenNganhs.Max(c => c.MaChuyenNganh) : 0;
            chuyenNganh.MaChuyenNganh = maxId + 1;
            chuyenNganh.NgayTao = DateTime.Now;
            _chuyenNganhs.Add(chuyenNganh);
            return chuyenNganh;
        }

        public ChuyenNganh? CapNhatChuyenNganh(ChuyenNganh chuyenNganh)
        {
            var existing = _chuyenNganhs.FirstOrDefault(c => c.MaChuyenNganh == chuyenNganh.MaChuyenNganh);
            if (existing == null)
                return null;

            existing.TenChuyenNganh = chuyenNganh.TenChuyenNganh;
            existing.MoTa = chuyenNganh.MoTa;
            existing.MaNganhNghe = chuyenNganh.MaNganhNghe;
            return existing;
        }

        public bool XoaChuyenNganh(int id)
        {
            var chuyenNganh = _chuyenNganhs.FirstOrDefault(c => c.MaChuyenNganh == id);
            if (chuyenNganh == null)
                return false;

            _chuyenNganhs.Remove(chuyenNganh);
            return true;
        }

        public bool XoaChuyenNganhTheoNganhNghe(int maNganhNghe)
        {
            var danhSachChuyenNganh = _chuyenNganhs
                .Where(c => c.MaNganhNghe == maNganhNghe)
                .ToList();

            foreach (var chuyenNganh in danhSachChuyenNganh)
            {
                _chuyenNganhs.Remove(chuyenNganh);
            }
            return true;
        }
    }
}

