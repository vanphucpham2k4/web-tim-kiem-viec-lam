namespace Unicareer.Models
{
    public class TinTuyenDung
    {
        public int MaTinTuyenDung { get; set; }
        public string TenViecLam { get; set; } = string.Empty; // Changed from TieuDe
        public string CongTy { get; set; } = string.Empty; // Ten cong ty (lien ket voi NhaTuyenDung)
        
        // Mo ta cong viec
        public string NganhNghe { get; set; } = string.Empty;
        public string NganhNgheChiTiet { get; set; } = string.Empty;
        public string LoaiCongViec { get; set; } = string.Empty;
        public string KinhNghiem { get; set; } = string.Empty;
        public string ViTri { get; set; } = string.Empty;
        public string NgoaiNgu { get; set; } = string.Empty;
        public string TuKhoa { get; set; } = string.Empty;
        public string KyNang { get; set; } = string.Empty;
        public string MoTa { get; set; } = string.Empty;
        
        // Yeu cau cong viec
        public string YeuCau { get; set; } = string.Empty;
        
        // Quyen loi
        public decimal MucLuongThapNhat { get; set; }
        public decimal MucLuongCaoNhat { get; set; }
        public string QuyenLoi { get; set; } = string.Empty;
        
        // Thong tin lien he
        public string NguoiLienHe { get; set; } = string.Empty;
        public string EmailLienHe { get; set; } = string.Empty;
        public string SDTLienHe { get; set; } = string.Empty;
        public string TinhThanhPho { get; set; } = string.Empty;
        public string PhuongXa { get; set; } = string.Empty;
        public string DiaChiLamViec { get; set; } = string.Empty;
        
        // Anh van phong
        public string AnhVanPhong { get; set; } = string.Empty;
        
        // Other fields
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
                    TenViecLam = "Senior Full-stack Developer",
                    CongTy = "FPT Software",
                    NganhNghe = "Cong nghe thong tin",
                    NganhNgheChiTiet = "Web Developer",
                    LoaiCongViec = "Full-time",
                    KinhNghiem = "Tren 5 nam",
                    ViTri = "Truong nhom",
                    NgoaiNgu = "Tieng Anh",
                    TuKhoa = "React, .NET Core, SQL",
                    KyNang = "Teamwork, Problem Solving, Leadership",
                    MoTa = "Phat trien ung dung web full-stack voi React va .NET Core",
                    YeuCau = "Co kinh nghiem lam viec voi React, .NET Core va SQL Server",
                    MucLuongThapNhat = 25,
                    MucLuongCaoNhat = 40,
                    QuyenLoi = "Bao hiem day du, thuong tet, du lich hang nam",
                    NguoiLienHe = "Nguyen Van An",
                    EmailLienHe = "recruit@fpt.com.vn",
                    SDTLienHe = "024-37912345",
                    TinhThanhPho = "Ha Noi",
                    PhuongXa = "Cau Giay",
                    DiaChiLamViec = "Keangnam Landmark Tower, Ha Noi",
                    AnhVanPhong = "",
                    SoLuongUngTuyen = 45,
                    NgayDang = DateTime.Now.AddDays(-5),
                    HanNop = DateTime.Now.AddDays(25)
                },
                new TinTuyenDung
                {
                    MaTinTuyenDung = 2,
                    TenViecLam = "Marketing Manager",
                    CongTy = "Vinamilk",
                    NganhNghe = "Kinh doanh",
                    NganhNgheChiTiet = "",
                    LoaiCongViec = "Full-time",
                    KinhNghiem = "2-5 nam",
                    ViTri = "Quan ly",
                    NgoaiNgu = "Tieng Anh",
                    TuKhoa = "Marketing, Digital Marketing, Brand Management",
                    KyNang = "Communication, Leadership, Creative Thinking",
                    MoTa = "Quan ly chien luoc marketing va phat trien thuong hieu",
                    YeuCau = "Co kinh nghiem quan ly doi ngu va phat trien chien luoc marketing",
                    MucLuongThapNhat = 20,
                    MucLuongCaoNhat = 35,
                    QuyenLoi = "Bao hiem xa hoi, thuong theo hieu suat",
                    NguoiLienHe = "Tran Thi Binh",
                    EmailLienHe = "hr@vinamilk.com.vn",
                    SDTLienHe = "028-54155555",
                    TinhThanhPho = "TP Ho Chi Minh",
                    PhuongXa = "Tan Phu",
                    DiaChiLamViec = "10 Tan Trao, Tan Phu, TP HCM",
                    AnhVanPhong = "",
                    SoLuongUngTuyen = 32,
                    NgayDang = DateTime.Now.AddDays(-3),
                    HanNop = DateTime.Now.AddDays(27)
                },
                new TinTuyenDung
                {
                    MaTinTuyenDung = 3,
                    TenViecLam = "Accountant",
                    CongTy = "Deloitte Vietnam",
                    NganhNghe = "Ke toan - Tai chinh",
                    NganhNgheChiTiet = "",
                    LoaiCongViec = "Full-time",
                    KinhNghiem = "1-2 nam",
                    ViTri = "Nhan vien",
                    NgoaiNgu = "Tieng Anh",
                    TuKhoa = "Accounting, Financial Report, Excel",
                    KyNang = "Attention to Detail, Analytical Skills",
                    MoTa = "Xu ly cac nghiep vu ke toan tong hop, bao cao tai chinh",
                    YeuCau = "Tot nghiep dai hoc chuyen nganh ke toan, biet tieng Anh",
                    MucLuongThapNhat = 12,
                    MucLuongCaoNhat = 18,
                    QuyenLoi = "Bao hiem y te, thuong cuoi nam",
                    NguoiLienHe = "Trinh Thi Mai",
                    EmailLienHe = "hrvietnam@deloitte.com",
                    SDTLienHe = "028-39101100",
                    TinhThanhPho = "Ha Noi",
                    PhuongXa = "Hoan Kiem",
                    DiaChiLamViec = "Saigon Centre Tower, Ha Noi",
                    AnhVanPhong = "",
                    SoLuongUngTuyen = 28,
                    NgayDang = DateTime.Now.AddDays(-7),
                    HanNop = DateTime.Now.AddDays(23)
                },
                new TinTuyenDung
                {
                    MaTinTuyenDung = 4,
                    TenViecLam = "Frontend Developer (ReactJS)",
                    CongTy = "Tiki",
                    NganhNghe = "Cong nghe thong tin",
                    NganhNgheChiTiet = "Web Developer",
                    LoaiCongViec = "Full-time",
                    KinhNghiem = "2-5 nam",
                    ViTri = "Nhan vien",
                    NgoaiNgu = "Tieng Anh",
                    TuKhoa = "ReactJS, JavaScript, HTML/CSS",
                    KyNang = "Problem Solving, Teamwork",
                    MoTa = "Phat trien giao dien nguoi dung cho website thuong mai dien tu",
                    YeuCau = "Thong thao ReactJS, JavaScript ES6+, co kinh nghiem REST API",
                    MucLuongThapNhat = 18,
                    MucLuongCaoNhat = 30,
                    QuyenLoi = "Bao hiem full, team building, dao tao",
                    NguoiLienHe = "Le Van Cuong",
                    EmailLienHe = "career@tiki.vn",
                    SDTLienHe = "028-62728728",
                    TinhThanhPho = "TP Ho Chi Minh",
                    PhuongXa = "Tan Binh",
                    DiaChiLamViec = "52 Ut Tich, Tan Binh, TP HCM",
                    AnhVanPhong = "",
                    SoLuongUngTuyen = 52,
                    NgayDang = DateTime.Now.AddDays(-2),
                    HanNop = DateTime.Now.AddDays(28)
                },
                new TinTuyenDung
                {
                    MaTinTuyenDung = 5,
                    TenViecLam = "UI/UX Designer",
                    CongTy = "VNG Corporation",
                    NganhNghe = "Thiet ke do hoa",
                    NganhNgheChiTiet = "UI/UX Designer",
                    LoaiCongViec = "Full-time",
                    KinhNghiem = "2-5 nam",
                    ViTri = "Nhan vien",
                    NgoaiNgu = "Tieng Anh",
                    TuKhoa = "Figma, Adobe XD, UI/UX",
                    KyNang = "Creative Thinking, User Research",
                    MoTa = "Thiet ke giao dien va trai nghiem nguoi dung cho cac san pham game va app",
                    YeuCau = "Co portfolio UI/UX tot, thong thao Figma/Adobe XD",
                    MucLuongThapNhat = 15,
                    MucLuongCaoNhat = 25,
                    QuyenLoi = "Bao hiem, thuong du an, moi truong tre trung",
                    NguoiLienHe = "Pham Thi Dung",
                    EmailLienHe = "jobs@vng.com.vn",
                    SDTLienHe = "028-54587262",
                    TinhThanhPho = "TP Ho Chi Minh",
                    PhuongXa = "Quan 7",
                    DiaChiLamViec = "Z06 Street, Tan Thuan EPZ, Q7, TP HCM",
                    AnhVanPhong = "",
                    SoLuongUngTuyen = 38,
                    NgayDang = DateTime.Now.AddDays(-4),
                    HanNop = DateTime.Now.AddDays(26)
                },
                new TinTuyenDung
                {
                    MaTinTuyenDung = 6,
                    TenViecLam = "Data Analyst",
                    CongTy = "Lazada Vietnam",
                    NganhNghe = "Cong nghe thong tin",
                    NganhNgheChiTiet = "Data Analyst",
                    LoaiCongViec = "Full-time",
                    KinhNghiem = "1-2 nam",
                    ViTri = "Nhan vien",
                    NgoaiNgu = "Tieng Anh",
                    TuKhoa = "SQL, Python, Data Visualization",
                    KyNang = "Analytical Skills, Attention to Detail",
                    MoTa = "Phan tich du lieu kinh doanh, tao bao cao va dashboard",
                    YeuCau = "Biet SQL, Python, co kien thuc ve Data Visualization",
                    MucLuongThapNhat = 15,
                    MucLuongCaoNhat = 28,
                    QuyenLoi = "Bao hiem, dao tao chuyen mon",
                    NguoiLienHe = "Hoang Van Dung",
                    EmailLienHe = "recruitment.vn@lazada.com",
                    SDTLienHe = "028-39107788",
                    TinhThanhPho = "Ha Noi",
                    PhuongXa = "Hoan Kiem",
                    DiaChiLamViec = "Keangnam Tower, Ha Noi",
                    AnhVanPhong = "",
                    SoLuongUngTuyen = 25,
                    NgayDang = DateTime.Now.AddDays(-6),
                    HanNop = DateTime.Now.AddDays(24)
                },
                new TinTuyenDung
                {
                    MaTinTuyenDung = 7,
                    TenViecLam = "HR Manager",
                    CongTy = "Viettel Group",
                    NganhNghe = "Nhan su",
                    NganhNgheChiTiet = "",
                    LoaiCongViec = "Full-time",
                    KinhNghiem = "Tren 5 nam",
                    ViTri = "Quan ly",
                    NgoaiNgu = "Tieng Anh",
                    TuKhoa = "HR Management, Recruitment, Training",
                    KyNang = "Leadership, Communication",
                    MoTa = "Quan ly tuyen dung, dao tao va phat trien nhan su",
                    YeuCau = "Co kinh nghiem quan ly nhan su trong cong ty lon",
                    MucLuongThapNhat = 20,
                    MucLuongCaoNhat = 30,
                    QuyenLoi = "Bao hiem full, thuong hieu suat cao",
                    NguoiLienHe = "Vo Thi Huong",
                    EmailLienHe = "tuyendung@viettel.com.vn",
                    SDTLienHe = "024-39779898",
                    TinhThanhPho = "Ha Noi",
                    PhuongXa = "Ba Dinh",
                    DiaChiLamViec = "Toa nha Viettel, Giang Vo, Ba Dinh, Ha Noi",
                    AnhVanPhong = "",
                    SoLuongUngTuyen = 18,
                    NgayDang = DateTime.Now.AddDays(-8),
                    HanNop = DateTime.Now.AddDays(22)
                },
                new TinTuyenDung
                {
                    MaTinTuyenDung = 8,
                    TenViecLam = "Content Writer",
                    CongTy = "VnExpress",
                    NganhNghe = "Truyen thong - Media",
                    NganhNgheChiTiet = "",
                    LoaiCongViec = "Full-time",
                    KinhNghiem = "1-2 nam",
                    ViTri = "Nhan vien",
                    NgoaiNgu = "Tieng Anh",
                    TuKhoa = "Content Writing, SEO, Social Media",
                    KyNang = "Writing Skills, Creativity",
                    MoTa = "Viet bai va bien tap noi dung cho website tin tuc",
                    YeuCau = "Ky nang viet tot, biet SEO co ban",
                    MucLuongThapNhat = 10,
                    MucLuongCaoNhat = 15,
                    QuyenLoi = "Bao hiem, thuong theo bai viet",
                    NguoiLienHe = "Dang Van Giau",
                    EmailLienHe = "hr@vnexpress.net",
                    SDTLienHe = "024-37576666",
                    TinhThanhPho = "Ha Noi",
                    PhuongXa = "Hoan Kiem",
                    DiaChiLamViec = "123 Nguyen Thi Minh Khai, Ha Noi",
                    AnhVanPhong = "",
                    SoLuongUngTuyen = 42,
                    NgayDang = DateTime.Now.AddDays(-1),
                    HanNop = DateTime.Now.AddDays(29)
                },
                new TinTuyenDung
                {
                    MaTinTuyenDung = 9,
                    TenViecLam = "Business Analyst",
                    CongTy = "Momo",
                    NganhNghe = "Kinh doanh",
                    NganhNgheChiTiet = "",
                    LoaiCongViec = "Full-time",
                    KinhNghiem = "2-5 nam",
                    ViTri = "Nhan vien",
                    NgoaiNgu = "Tieng Anh",
                    TuKhoa = "Business Analysis, Requirements, Process Improvement",
                    KyNang = "Analytical Skills, Communication",
                    MoTa = "Phan tich yeu cau kinh doanh va toi uu quy trinh",
                    YeuCau = "Co kinh nghiem phan tich quy trinh va lam viec voi stakeholders",
                    MucLuongThapNhat = 18,
                    MucLuongCaoNhat = 32,
                    QuyenLoi = "Bao hiem, thuong du an, phuc loi tot",
                    NguoiLienHe = "Bui Thi Hoa",
                    EmailLienHe = "career@momo.vn",
                    SDTLienHe = "028-71089999",
                    TinhThanhPho = "TP Ho Chi Minh",
                    PhuongXa = "Quan 4",
                    DiaChiLamViec = "Lim Tower, 9-11 Ton Dan, Q4, TP HCM",
                    AnhVanPhong = "",
                    SoLuongUngTuyen = 35,
                    NgayDang = DateTime.Now.AddDays(-3),
                    HanNop = DateTime.Now.AddDays(27)
                },
                new TinTuyenDung
                {
                    MaTinTuyenDung = 10,
                    TenViecLam = "Backend Developer (Java)",
                    CongTy = "Samsung Vietnam",
                    NganhNghe = "Cong nghe thong tin",
                    NganhNgheChiTiet = "Web Developer",
                    LoaiCongViec = "Full-time",
                    KinhNghiem = "2-5 nam",
                    ViTri = "Nhan vien",
                    NgoaiNgu = "Tieng Anh",
                    TuKhoa = "Java, Spring Boot, Microservices",
                    KyNang = "Problem Solving, System Design",
                    MoTa = "Phat trien he thong backend cho cac ung dung di dong",
                    YeuCau = "Thong thao Java, Spring Boot, co kinh nghiem Microservices",
                    MucLuongThapNhat = 22,
                    MucLuongCaoNhat = 38,
                    QuyenLoi = "Bao hiem full, thuong du an lon, dao tao",
                    NguoiLienHe = "Ngo Van Khanh",
                    EmailLienHe = "recruit.vn@samsung.com",
                    SDTLienHe = "024-39366666",
                    TinhThanhPho = "Ha Noi",
                    PhuongXa = "Cau Giay",
                    DiaChiLamViec = "Keangnam Landmark Tower, Ha Noi",
                    AnhVanPhong = "",
                    SoLuongUngTuyen = 29,
                    NgayDang = DateTime.Now.AddDays(-9),
                    HanNop = DateTime.Now.AddDays(21)
                },
                new TinTuyenDung
                {
                    MaTinTuyenDung = 11,
                    TenViecLam = "Intern - Software Developer",
                    CongTy = "Shopee Vietnam",
                    NganhNghe = "Cong nghe thong tin",
                    NganhNgheChiTiet = "Web Developer",
                    LoaiCongViec = "Internship",
                    KinhNghiem = "Khong yeu cau",
                    ViTri = "Nhan vien",
                    NgoaiNgu = "Tieng Anh",
                    TuKhoa = "Java, Python, C#",
                    KyNang = "Eager to Learn, Teamwork",
                    MoTa = "Thuc tap phat trien phan mem, ho tro team backend",
                    YeuCau = "Sinh vien nam cuoi hoac moi tot nghiep, co kien thuc lap trinh co ban",
                    MucLuongThapNhat = 5,
                    MucLuongCaoNhat = 7,
                    QuyenLoi = "Dao tao, co hoi tro thanh nhan vien chinh thuc",
                    NguoiLienHe = "Duong Thi Lan",
                    EmailLienHe = "careers.vn@shopee.com",
                    SDTLienHe = "028-73088888",
                    TinhThanhPho = "TP Ho Chi Minh",
                    PhuongXa = "Binh Thanh",
                    DiaChiLamViec = "Pearl Plaza, 561A Dien Bien Phu, Binh Thanh, TP HCM",
                    AnhVanPhong = "",
                    SoLuongUngTuyen = 68,
                    NgayDang = DateTime.Now.AddDays(-2),
                    HanNop = DateTime.Now.AddDays(28)
                },
                new TinTuyenDung
                {
                    MaTinTuyenDung = 12,
                    TenViecLam = "Sales Manager",
                    CongTy = "Highlands Coffee",
                    NganhNghe = "Kinh doanh",
                    NganhNgheChiTiet = "",
                    LoaiCongViec = "Full-time",
                    KinhNghiem = "Tren 5 nam",
                    ViTri = "Quan ly",
                    NgoaiNgu = "Tieng Anh",
                    TuKhoa = "Sales Management, Business Development, Customer Relations",
                    KyNang = "Leadership, Negotiation, Communication",
                    MoTa = "Quan ly doi ngu ban hang va phat trien kenh phan phoi",
                    YeuCau = "Co kinh nghiem quan ly doi ngu ban hang, phat trien kenh phan phoi",
                    MucLuongThapNhat = 15,
                    MucLuongCaoNhat = 25,
                    QuyenLoi = "Bao hiem, hoa hong hap dan, thuong theo doanh so",
                    NguoiLienHe = "Ly Van Long",
                    EmailLienHe = "recruitment@highlandscoffee.com.vn",
                    SDTLienHe = "028-39118888",
                    TinhThanhPho = "Ha Noi",
                    PhuongXa = "Hoan Kiem",
                    DiaChiLamViec = "24 Nguyen Hue, Hoan Kiem, Ha Noi",
                    AnhVanPhong = "",
                    SoLuongUngTuyen = 22,
                    NgayDang = DateTime.Now.AddDays(-5),
                    HanNop = DateTime.Now.AddDays(25)
                }
            };
        }
    }
}

