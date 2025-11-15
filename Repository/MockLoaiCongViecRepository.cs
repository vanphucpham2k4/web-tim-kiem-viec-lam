using Unicareer.Models;

namespace Unicareer.Repository
{
    public class MockLoaiCongViecRepository : ILoaiCongViecRepository
    {
        private readonly List<LoaiCongViec> _danhSachLoaiCongViec;

        public MockLoaiCongViecRepository()
        {
            _danhSachLoaiCongViec = new List<LoaiCongViec>
            {
                new LoaiCongViec
                {
                    MaLoaiCongViec = 1,
                    TenLoaiCongViec = "Full-time",
                    MoTa = "Lam viec toan thoi gian, 8 gio/ngay, 5-6 ngay/tuan",
                    SoLuongViTri = 3250,
                    NgayTao = DateTime.Now.AddMonths(-12)
                },
                new LoaiCongViec
                {
                    MaLoaiCongViec = 2,
                    TenLoaiCongViec = "Part-time",
                    MoTa = "Lam viec ban thoi gian, linh hoat gio gio lam viec",
                    SoLuongViTri = 890,
                    NgayTao = DateTime.Now.AddMonths(-10)
                },
                new LoaiCongViec
                {
                    MaLoaiCongViec = 3,
                    TenLoaiCongViec = "Remote",
                    MoTa = "Lam viec tu xa, khong can den van phong",
                    SoLuongViTri = 1450,
                    NgayTao = DateTime.Now.AddMonths(-8)
                },
                new LoaiCongViec
                {
                    MaLoaiCongViec = 4,
                    TenLoaiCongViec = "Internship",
                    MoTa = "Thuc tap, danh cho sinh vien va nguoi moi tot nghiep",
                    SoLuongViTri = 620,
                    NgayTao = DateTime.Now.AddMonths(-6)
                },
                new LoaiCongViec
                {
                    MaLoaiCongViec = 5,
                    TenLoaiCongViec = "Freelance",
                    MoTa = "Tu do, lam theo du an, khong rang buoc thoi gian",
                    SoLuongViTri = 780,
                    NgayTao = DateTime.Now.AddMonths(-9)
                },
                new LoaiCongViec
                {
                    MaLoaiCongViec = 6,
                    TenLoaiCongViec = "Contract",
                    MoTa = "Hop dong co thoi han, thuong tu 6-12 thang",
                    SoLuongViTri = 540,
                    NgayTao = DateTime.Now.AddMonths(-7)
                },
                new LoaiCongViec
                {
                    MaLoaiCongViec = 7,
                    TenLoaiCongViec = "Temporary",
                    MoTa = "Lam viec tam thoi, ngan han duoi 6 thang",
                    SoLuongViTri = 320,
                    NgayTao = DateTime.Now.AddMonths(-5)
                },
                new LoaiCongViec
                {
                    MaLoaiCongViec = 8,
                    TenLoaiCongViec = "Seasonal",
                    MoTa = "Theo mua vu, thoi diem cao diem trong nam",
                    SoLuongViTri = 210,
                    NgayTao = DateTime.Now.AddMonths(-11)
                },
                new LoaiCongViec
                {
                    MaLoaiCongViec = 9,
                    TenLoaiCongViec = "Volunteer",
                    MoTa = "Tinh nguyen, khong luong hoac luong thap",
                    SoLuongViTri = 150,
                    NgayTao = DateTime.Now.AddMonths(-4)
                },
                new LoaiCongViec
                {
                    MaLoaiCongViec = 10,
                    TenLoaiCongViec = "Commission",
                    MoTa = "Luong hoa hong, phu thuoc vao ket qua kinh doanh",
                    SoLuongViTri = 420,
                    NgayTao = DateTime.Now.AddMonths(-3)
                },
                new LoaiCongViec
                {
                    MaLoaiCongViec = 11,
                    TenLoaiCongViec = "Hybrid",
                    MoTa = "Ket hop van phong va lam viec tu xa",
                    SoLuongViTri = 980,
                    NgayTao = DateTime.Now.AddMonths(-2)
                },
                new LoaiCongViec
                {
                    MaLoaiCongViec = 12,
                    TenLoaiCongViec = "Shift Work",
                    MoTa = "Lam viec theo ca, co the ban dem hoac cuoi tuan",
                    SoLuongViTri = 560,
                    NgayTao = DateTime.Now.AddMonths(-6)
                }
            };
        }

        public List<LoaiCongViec> LayDanhSachLoaiCongViec()
        {
            return _danhSachLoaiCongViec;
        }

        public LoaiCongViec? LayLoaiCongViecTheoId(int id)
        {
            return _danhSachLoaiCongViec.FirstOrDefault(l => l.MaLoaiCongViec == id);
        }

        public LoaiCongViec? ThemLoaiCongViec(LoaiCongViec loaiCongViec)
        {
            try
            {
                loaiCongViec.MaLoaiCongViec = _danhSachLoaiCongViec.Count > 0 
                    ? _danhSachLoaiCongViec.Max(l => l.MaLoaiCongViec) + 1 
                    : 1;
                loaiCongViec.NgayTao = DateTime.Now;
                loaiCongViec.SoLuongViTri = 0;
                _danhSachLoaiCongViec.Add(loaiCongViec);
                return loaiCongViec;
            }
            catch
            {
                return null;
            }
        }

        public LoaiCongViec? CapNhatLoaiCongViec(LoaiCongViec loaiCongViec)
        {
            try
            {
                var loaiCongViecHienTai = _danhSachLoaiCongViec
                    .FirstOrDefault(l => l.MaLoaiCongViec == loaiCongViec.MaLoaiCongViec);
                
                if (loaiCongViecHienTai == null)
                    return null;

                loaiCongViecHienTai.TenLoaiCongViec = loaiCongViec.TenLoaiCongViec;
                loaiCongViecHienTai.MoTa = loaiCongViec.MoTa;
                return loaiCongViecHienTai;
            }
            catch
            {
                return null;
            }
        }

        public bool XoaLoaiCongViec(int id)
        {
            try
            {
                var loaiCongViec = _danhSachLoaiCongViec
                    .FirstOrDefault(l => l.MaLoaiCongViec == id);
                
                if (loaiCongViec == null)
                    return false;

                _danhSachLoaiCongViec.Remove(loaiCongViec);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

