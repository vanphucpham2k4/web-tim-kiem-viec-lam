using Unicareer.Models;

namespace Unicareer.Repository
{
    public class MockNganhNgheRepository : INganhNgheRepository
    {
        private readonly List<NganhNghe> _danhSachNganhNghe;

        public MockNganhNgheRepository()
        {
            _danhSachNganhNghe = new List<NganhNghe>
            {
                new NganhNghe
                {
                    MaNganhNghe = 1,
                    TenNganhNghe = "Cong nghe thong tin",
                    MoTa = "Cac vi tri lien quan den phat trien phan mem, lap trinh, bao mat thong tin",
                    SoLuongCongViec = 1250,
                    NgayTao = DateTime.Now.AddMonths(-6)
                },
                new NganhNghe
                {
                    MaNganhNghe = 2,
                    TenNganhNghe = "Kinh doanh",
                    MoTa = "Ban hang, marketing, quan ly kinh doanh, phat trien thi truong",
                    SoLuongCongViec = 890,
                    NgayTao = DateTime.Now.AddMonths(-8)
                },
                new NganhNghe
                {
                    MaNganhNghe = 3,
                    TenNganhNghe = "Ke toan - Tai chinh",
                    MoTa = "Ke toan vien, kiem toan, phan tich tai chinh, ngan hang",
                    SoLuongCongViec = 650,
                    NgayTao = DateTime.Now.AddMonths(-10)
                },
                new NganhNghe
                {
                    MaNganhNghe = 4,
                    TenNganhNghe = "Giao duc - Dao tao",
                    MoTa = "Giang vien, giao vien, dao tao nhan su, phat trien chuong trinh",
                    SoLuongCongViec = 420,
                    NgayTao = DateTime.Now.AddMonths(-5)
                },
                new NganhNghe
                {
                    MaNganhNghe = 5,
                    TenNganhNghe = "Y te - Duoc pham",
                    MoTa = "Bac si, duoc si, dieu duong, nghien cuu y hoc",
                    SoLuongCongViec = 780,
                    NgayTao = DateTime.Now.AddMonths(-12)
                },
                new NganhNghe
                {
                    MaNganhNghe = 6,
                    TenNganhNghe = "Xay dung - Kien truc",
                    MoTa = "Ky su xay dung, kien truc su, quan ly du an xay dung",
                    SoLuongCongViec = 540,
                    NgayTao = DateTime.Now.AddMonths(-7)
                },
                new NganhNghe
                {
                    MaNganhNghe = 7,
                    TenNganhNghe = "Truyen thong - Media",
                    MoTa = "Bien tap vien, nha bao, quang cao, content creator",
                    SoLuongCongViec = 320,
                    NgayTao = DateTime.Now.AddMonths(-4)
                },
                new NganhNghe
                {
                    MaNganhNghe = 8,
                    TenNganhNghe = "Nhan su",
                    MoTa = "Tuyen dung, dao tao, quan ly nhan su, phat trien to chuc",
                    SoLuongCongViec = 410,
                    NgayTao = DateTime.Now.AddMonths(-9)
                },
                new NganhNghe
                {
                    MaNganhNghe = 9,
                    TenNganhNghe = "Thiet ke do hoa",
                    MoTa = "Thiet ke UI/UX, thiet ke do hoa, thiet ke san pham",
                    SoLuongCongViec = 290,
                    NgayTao = DateTime.Now.AddMonths(-3)
                },
                new NganhNghe
                {
                    MaNganhNghe = 10,
                    TenNganhNghe = "Phap ly - Luat su",
                    MoTa = "Luat su, chuyen vien phap ly, tu van luat",
                    SoLuongCongViec = 180,
                    NgayTao = DateTime.Now.AddMonths(-11)
                },
                new NganhNghe
                {
                    MaNganhNghe = 11,
                    TenNganhNghe = "Du lich - Khach san",
                    MoTa = "Huong dan vien du lich, quan ly khach san, dich vu du lich",
                    SoLuongCongViec = 150,
                    NgayTao = DateTime.Now.AddMonths(-2)
                },
                new NganhNghe
                {
                    MaNganhNghe = 12,
                    TenNganhNghe = "Logistics - Van tai",
                    MoTa = "Quan ly chuoi cung ung, van chuyen, kho bai",
                    SoLuongCongViec = 470,
                    NgayTao = DateTime.Now.AddMonths(-6)
                }
            };
        }

        public List<NganhNghe> LayDanhSachNganhNghe()
        {
            return _danhSachNganhNghe;
        }

        public NganhNghe? LayNganhNgheTheoId(int id)
        {
            return _danhSachNganhNghe.FirstOrDefault(n => n.MaNganhNghe == id);
        }
    }
}

