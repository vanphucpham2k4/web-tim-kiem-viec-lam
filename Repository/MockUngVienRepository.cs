using Unicareer.Models;

namespace Unicareer.Repository
{
    public class MockUngVienRepository : IUngVienRepository
    {
        private readonly List<UngVien> _danhSachUngVien;

        public MockUngVienRepository()
        {
            _danhSachUngVien = new List<UngVien>
            {
                new UngVien
                {
                    MaUngVien = 1,
                    HoTen = "Nguyen Van A",
                    Email = "nguyenvana@gmail.com",
                    SoDienThoai = "0912345678",
                    NgaySinh = new DateTime(1998, 5, 15),
                    GioiTinh = "Nam",
                    DiaChi = "Ha Noi",
                    HocVan = "Dai hoc - Cong nghe thong tin",
                    KinhNghiem = "3 nam",
                    KyNang = "C#, .NET Core, ReactJS, SQL Server",
                    NganhNghe = "Cong nghe thong tin",
                    LinkCV = "/uploads/cv_nguyenvana.pdf",
                    SoLanUngTuyen = 8,
                    NgayDangKy = DateTime.Now.AddMonths(-6)
                },
                new UngVien
                {
                    MaUngVien = 2,
                    HoTen = "Tran Thi B",
                    Email = "tranthib@gmail.com",
                    SoDienThoai = "0987654321",
                    NgaySinh = new DateTime(1995, 8, 20),
                    GioiTinh = "Nu",
                    DiaChi = "TP Ho Chi Minh",
                    HocVan = "Dai hoc - Marketing",
                    KinhNghiem = "5 nam",
                    KyNang = "Digital Marketing, SEO, Content Marketing, Facebook Ads",
                    NganhNghe = "Kinh doanh",
                    LinkCV = "/uploads/cv_tranthib.pdf",
                    SoLanUngTuyen = 12,
                    NgayDangKy = DateTime.Now.AddMonths(-8)
                },
                new UngVien
                {
                    MaUngVien = 3,
                    HoTen = "Le Van C",
                    Email = "levanc@gmail.com",
                    SoDienThoai = "0901234567",
                    NgaySinh = new DateTime(1999, 3, 10),
                    GioiTinh = "Nam",
                    DiaChi = "Da Nang",
                    HocVan = "Dai hoc - Cong nghe thong tin",
                    KinhNghiem = "2 nam",
                    KyNang = "ReactJS, NodeJS, MongoDB, HTML/CSS",
                    NganhNghe = "Cong nghe thong tin",
                    LinkCV = "/uploads/cv_levanc.pdf",
                    SoLanUngTuyen = 15,
                    NgayDangKy = DateTime.Now.AddMonths(-3)
                },
                new UngVien
                {
                    MaUngVien = 4,
                    HoTen = "Pham Thi D",
                    Email = "phamthid@gmail.com",
                    SoDienThoai = "0923456789",
                    NgaySinh = new DateTime(1997, 12, 5),
                    GioiTinh = "Nu",
                    DiaChi = "Ha Noi",
                    HocVan = "Dai hoc - Thiet ke do hoa",
                    KinhNghiem = "4 nam",
                    KyNang = "Figma, Adobe XD, Photoshop, Illustrator, UI/UX",
                    NganhNghe = "Thiet ke do hoa",
                    LinkCV = "/uploads/cv_phamthid.pdf",
                    SoLanUngTuyen = 6,
                    NgayDangKy = DateTime.Now.AddMonths(-4)
                },
                new UngVien
                {
                    MaUngVien = 5,
                    HoTen = "Hoang Van E",
                    Email = "hoangvane@gmail.com",
                    SoDienThoai = "0934567890",
                    NgaySinh = new DateTime(1996, 7, 25),
                    GioiTinh = "Nam",
                    DiaChi = "Ha Noi",
                    HocVan = "Dai hoc - Kinh te",
                    KinhNghiem = "3 nam",
                    KyNang = "Excel, Power BI, SQL, Python, Data Analysis",
                    NganhNghe = "Ke toan - Tai chinh",
                    LinkCV = "/uploads/cv_hoangvane.pdf",
                    SoLanUngTuyen = 9,
                    NgayDangKy = DateTime.Now.AddMonths(-7)
                }
            };
        }

        public List<UngVien> LayDanhSachUngVien()
        {
            return _danhSachUngVien;
        }

        public UngVien? LayUngVienTheoId(int id)
        {
            return _danhSachUngVien.FirstOrDefault(u => u.MaUngVien == id);
        }
    }
}

