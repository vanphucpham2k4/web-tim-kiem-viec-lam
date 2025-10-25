namespace Unicareer.Models
{
    public class TinTuyenDung
    {
        public int MaTinTuyenDung { get; set; }
        public string TieuDe { get; set; } = string.Empty;
        public string CongTy { get; set; } = string.Empty;
        public string ViTri { get; set; } = string.Empty;
        public string MucLuong { get; set; } = string.Empty;
        public string DiaDiem { get; set; } = string.Empty;
        public string LoaiCongViec { get; set; } = string.Empty;
        public string NganhNghe { get; set; } = string.Empty;
        public string MoTa { get; set; } = string.Empty;
        public int SoLuongUngTuyen { get; set; }
        public DateTime NgayDang { get; set; }
        public DateTime HanNop { get; set; }

        // Mockdata - Danh sach tin tuyen dung
        public static List<TinTuyenDung> LayDanhSachTinTuyenDung()
        {
            return new List<TinTuyenDung>
            {
                new TinTuyenDung
                {
                    MaTinTuyenDung = 1,
                    TieuDe = "Senior Full-stack Developer",
                    CongTy = "FPT Software",
                    ViTri = "Senior Developer",
                    MucLuong = "25-40 trieu",
                    DiaDiem = "Ha Noi",
                    LoaiCongViec = "Full-time",
                    NganhNghe = "Cong nghe thong tin",
                    MoTa = "Phat trien ung dung web full-stack voi React va .NET Core",
                    SoLuongUngTuyen = 45,
                    NgayDang = DateTime.Now.AddDays(-5),
                    HanNop = DateTime.Now.AddDays(25)
                },
                new TinTuyenDung
                {
                    MaTinTuyenDung = 2,
                    TieuDe = "Marketing Manager",
                    CongTy = "Vinamilk",
                    ViTri = "Marketing Manager",
                    MucLuong = "20-35 trieu",
                    DiaDiem = "TP Ho Chi Minh",
                    LoaiCongViec = "Full-time",
                    NganhNghe = "Kinh doanh",
                    MoTa = "Quan ly chien luoc marketing va phat trien thuong hieu",
                    SoLuongUngTuyen = 32,
                    NgayDang = DateTime.Now.AddDays(-3),
                    HanNop = DateTime.Now.AddDays(27)
                },
                new TinTuyenDung
                {
                    MaTinTuyenDung = 3,
                    TieuDe = "Accountant",
                    CongTy = "Deloitte Vietnam",
                    ViTri = "Accountant",
                    MucLuong = "12-18 trieu",
                    DiaDiem = "Ha Noi",
                    LoaiCongViec = "Full-time",
                    NganhNghe = "Ke toan - Tai chinh",
                    MoTa = "Xu ly cac nghiep vu ke toan tong hop, bao cao tai chinh",
                    SoLuongUngTuyen = 28,
                    NgayDang = DateTime.Now.AddDays(-7),
                    HanNop = DateTime.Now.AddDays(23)
                },
                new TinTuyenDung
                {
                    MaTinTuyenDung = 4,
                    TieuDe = "Frontend Developer (ReactJS)",
                    CongTy = "Tiki",
                    ViTri = "Frontend Developer",
                    MucLuong = "18-30 trieu",
                    DiaDiem = "TP Ho Chi Minh",
                    LoaiCongViec = "Full-time",
                    NganhNghe = "Cong nghe thong tin",
                    MoTa = "Phat trien giao dien nguoi dung cho website thuong mai dien tu",
                    SoLuongUngTuyen = 52,
                    NgayDang = DateTime.Now.AddDays(-2),
                    HanNop = DateTime.Now.AddDays(28)
                },
                new TinTuyenDung
                {
                    MaTinTuyenDung = 5,
                    TieuDe = "UI/UX Designer",
                    CongTy = "VNG Corporation",
                    ViTri = "UI/UX Designer",
                    MucLuong = "15-25 trieu",
                    DiaDiem = "TP Ho Chi Minh",
                    LoaiCongViec = "Full-time",
                    NganhNghe = "Thiet ke do hoa",
                    MoTa = "Thiet ke giao dien va trai nghiem nguoi dung cho cac san pham game va app",
                    SoLuongUngTuyen = 38,
                    NgayDang = DateTime.Now.AddDays(-4),
                    HanNop = DateTime.Now.AddDays(26)
                },
                new TinTuyenDung
                {
                    MaTinTuyenDung = 6,
                    TieuDe = "Data Analyst",
                    CongTy = "Lazada Vietnam",
                    ViTri = "Data Analyst",
                    MucLuong = "15-28 trieu",
                    DiaDiem = "Ha Noi",
                    LoaiCongViec = "Full-time",
                    NganhNghe = "Cong nghe thong tin",
                    MoTa = "Phan tich du lieu kinh doanh, tao bao cao va dashboard",
                    SoLuongUngTuyen = 25,
                    NgayDang = DateTime.Now.AddDays(-6),
                    HanNop = DateTime.Now.AddDays(24)
                },
                new TinTuyenDung
                {
                    MaTinTuyenDung = 7,
                    TieuDe = "HR Manager",
                    CongTy = "Viettel Group",
                    ViTri = "HR Manager",
                    MucLuong = "20-30 trieu",
                    DiaDiem = "Ha Noi",
                    LoaiCongViec = "Full-time",
                    NganhNghe = "Nhan su",
                    MoTa = "Quan ly tuyen dung, dao tao va phat trien nhan su",
                    SoLuongUngTuyen = 18,
                    NgayDang = DateTime.Now.AddDays(-8),
                    HanNop = DateTime.Now.AddDays(22)
                },
                new TinTuyenDung
                {
                    MaTinTuyenDung = 8,
                    TieuDe = "Content Writer",
                    CongTy = "VnExpress",
                    ViTri = "Content Writer",
                    MucLuong = "10-15 trieu",
                    DiaDiem = "Ha Noi",
                    LoaiCongViec = "Full-time",
                    NganhNghe = "Truyen thong - Media",
                    MoTa = "Viet bai va bien tap noi dung cho website tin tuc",
                    SoLuongUngTuyen = 42,
                    NgayDang = DateTime.Now.AddDays(-1),
                    HanNop = DateTime.Now.AddDays(29)
                },
                new TinTuyenDung
                {
                    MaTinTuyenDung = 9,
                    TieuDe = "Business Analyst",
                    CongTy = "Momo",
                    ViTri = "Business Analyst",
                    MucLuong = "18-32 trieu",
                    DiaDiem = "TP Ho Chi Minh",
                    LoaiCongViec = "Full-time",
                    NganhNghe = "Kinh doanh",
                    MoTa = "Phan tich yeu cau kinh doanh va toi uu quy trinh",
                    SoLuongUngTuyen = 35,
                    NgayDang = DateTime.Now.AddDays(-3),
                    HanNop = DateTime.Now.AddDays(27)
                },
                new TinTuyenDung
                {
                    MaTinTuyenDung = 10,
                    TieuDe = "Backend Developer (Java)",
                    CongTy = "Samsung Vietnam",
                    ViTri = "Backend Developer",
                    MucLuong = "22-38 trieu",
                    DiaDiem = "Ha Noi",
                    LoaiCongViec = "Full-time",
                    NganhNghe = "Cong nghe thong tin",
                    MoTa = "Phat trien he thong backend cho cac ung dung di dong",
                    SoLuongUngTuyen = 29,
                    NgayDang = DateTime.Now.AddDays(-9),
                    HanNop = DateTime.Now.AddDays(21)
                },
                new TinTuyenDung
                {
                    MaTinTuyenDung = 11,
                    TieuDe = "Intern - Software Developer",
                    CongTy = "Shopee Vietnam",
                    ViTri = "Intern Developer",
                    MucLuong = "5-7 trieu",
                    DiaDiem = "TP Ho Chi Minh",
                    LoaiCongViec = "Internship",
                    NganhNghe = "Cong nghe thong tin",
                    MoTa = "Thuc tap phat trien phan mem, ho tro team backend",
                    SoLuongUngTuyen = 68,
                    NgayDang = DateTime.Now.AddDays(-2),
                    HanNop = DateTime.Now.AddDays(28)
                },
                new TinTuyenDung
                {
                    MaTinTuyenDung = 12,
                    TieuDe = "Sales Manager",
                    CongTy = "Highlands Coffee",
                    ViTri = "Sales Manager",
                    MucLuong = "15-25 trieu + hoa hong",
                    DiaDiem = "Ha Noi",
                    LoaiCongViec = "Full-time",
                    NganhNghe = "Kinh doanh",
                    MoTa = "Quan ly doi ngu ban hang va phat trien kenh phan phoi",
                    SoLuongUngTuyen = 22,
                    NgayDang = DateTime.Now.AddDays(-5),
                    HanNop = DateTime.Now.AddDays(25)
                }
            };
        }
    }
}

