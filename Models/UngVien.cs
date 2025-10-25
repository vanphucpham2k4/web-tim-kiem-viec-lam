namespace Unicareer.Models
{
    public class UngVien
    {
        public int MaUngVien { get; set; }
        public string HoTen { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string SoDienThoai { get; set; } = string.Empty;
        public DateTime NgaySinh { get; set; }
        public string GioiTinh { get; set; } = string.Empty;
        public string DiaChi { get; set; } = string.Empty;
        public string HocVan { get; set; } = string.Empty;
        public string KinhNghiem { get; set; } = string.Empty;
        public string KyNang { get; set; } = string.Empty;
        public string NganhNghe { get; set; } = string.Empty;
        public string LinkCV { get; set; } = string.Empty;
        public int SoLanUngTuyen { get; set; }
        public DateTime NgayDangKy { get; set; }

        // Mockdata - Danh sach ung vien
        public static List<UngVien> LayDanhSachUngVien()
        {
            return new List<UngVien>
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
                },
                new UngVien
                {
                    MaUngVien = 6,
                    HoTen = "Vo Thi F",
                    Email = "vothif@gmail.com",
                    SoDienThoai = "0945678901",
                    NgaySinh = new DateTime(1994, 4, 18),
                    GioiTinh = "Nu",
                    DiaChi = "TP Ho Chi Minh",
                    HocVan = "Dai hoc - Quan tri nhan luc",
                    KinhNghiem = "6 nam",
                    KyNang = "Tuyen dung, Dao tao, Quan ly nhan su",
                    NganhNghe = "Nhan su",
                    LinkCV = "/uploads/cv_vothif.pdf",
                    SoLanUngTuyen = 5,
                    NgayDangKy = DateTime.Now.AddMonths(-10)
                },
                new UngVien
                {
                    MaUngVien = 7,
                    HoTen = "Dang Van G",
                    Email = "dangvang@gmail.com",
                    SoDienThoai = "0956789012",
                    NgaySinh = new DateTime(2000, 1, 30),
                    GioiTinh = "Nam",
                    DiaChi = "Ha Noi",
                    HocVan = "Dai hoc - Bao chi truyen thong",
                    KinhNghiem = "1 nam",
                    KyNang = "Viet bai, Bien tap, Chup anh, Dung video",
                    NganhNghe = "Truyen thong - Media",
                    LinkCV = "/uploads/cv_dangvang.pdf",
                    SoLanUngTuyen = 18,
                    NgayDangKy = DateTime.Now.AddMonths(-2)
                },
                new UngVien
                {
                    MaUngVien = 8,
                    HoTen = "Bui Thi H",
                    Email = "buithih@gmail.com",
                    SoDienThoai = "0967890123",
                    NgaySinh = new DateTime(1998, 9, 12),
                    GioiTinh = "Nu",
                    DiaChi = "Da Nang",
                    HocVan = "Dai hoc - Quan tri kinh doanh",
                    KinhNghiem = "2 nam",
                    KyNang = "Phan tich kinh doanh, Excel, PowerPoint, Communication",
                    NganhNghe = "Kinh doanh",
                    LinkCV = "/uploads/cv_buithih.pdf",
                    SoLanUngTuyen = 11,
                    NgayDangKy = DateTime.Now.AddMonths(-5)
                },
                new UngVien
                {
                    MaUngVien = 9,
                    HoTen = "Ngo Van I",
                    Email = "ngovani@gmail.com",
                    SoDienThoai = "0978901234",
                    NgaySinh = new DateTime(1997, 6, 8),
                    GioiTinh = "Nam",
                    DiaChi = "Ha Noi",
                    HocVan = "Dai hoc - Cong nghe thong tin",
                    KinhNghiem = "4 nam",
                    KyNang = "Java, Spring Boot, Microservices, Docker, Kubernetes",
                    NganhNghe = "Cong nghe thong tin",
                    LinkCV = "/uploads/cv_ngovani.pdf",
                    SoLanUngTuyen = 7,
                    NgayDangKy = DateTime.Now.AddMonths(-9)
                },
                new UngVien
                {
                    MaUngVien = 10,
                    HoTen = "Duong Thi K",
                    Email = "duongthik@gmail.com",
                    SoDienThoai = "0989012345",
                    NgaySinh = new DateTime(2001, 11, 22),
                    GioiTinh = "Nu",
                    DiaChi = "TP Ho Chi Minh",
                    HocVan = "Dai hoc - Cong nghe thong tin (nam 4)",
                    KinhNghiem = "Fresher",
                    KyNang = "Python, Java, HTML/CSS, Git",
                    NganhNghe = "Cong nghe thong tin",
                    LinkCV = "/uploads/cv_duongthik.pdf",
                    SoLanUngTuyen = 22,
                    NgayDangKy = DateTime.Now.AddMonths(-1)
                },
                new UngVien
                {
                    MaUngVien = 11,
                    HoTen = "Ly Van L",
                    Email = "lyvanl@gmail.com",
                    SoDienThoai = "0990123456",
                    NgaySinh = new DateTime(1993, 2, 14),
                    GioiTinh = "Nam",
                    DiaChi = "Ha Noi",
                    HocVan = "Dai hoc - Quan tri kinh doanh",
                    KinhNghiem = "7 nam",
                    KyNang = "Ban hang, Quan ly doi nhom, Dam phan, CRM",
                    NganhNghe = "Kinh doanh",
                    LinkCV = "/uploads/cv_lyvanl.pdf",
                    SoLanUngTuyen = 4,
                    NgayDangKy = DateTime.Now.AddMonths(-12)
                },
                new UngVien
                {
                    MaUngVien = 12,
                    HoTen = "Trinh Thi M",
                    Email = "trinhthim@gmail.com",
                    SoDienThoai = "0901234560",
                    NgaySinh = new DateTime(1996, 10, 3),
                    GioiTinh = "Nu",
                    DiaChi = "Ha Noi",
                    HocVan = "Dai hoc - Ke toan",
                    KinhNghiem = "4 nam",
                    KyNang = "Ke toan tong hop, Thue, Excel, SAP",
                    NganhNghe = "Ke toan - Tai chinh",
                    LinkCV = "/uploads/cv_trinhthim.pdf",
                    SoLanUngTuyen = 10,
                    NgayDangKy = DateTime.Now.AddMonths(-6)
                }
            };
        }
    }
}

