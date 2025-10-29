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
        
        // Geolocation
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

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
                    NganhNghe = "Công nghệ thông tin",
                    NganhNgheChiTiet = "Web Developer",
                    LoaiCongViec = "Full-time",
                    KinhNghiem = "Tren 5 nam",
                    ViTri = "Truong nhom",
                    NgoaiNgu = "Tieng Anh",
                    TuKhoa = "React, .NET Core, SQL",
                    KyNang = "Teamwork, Problem Solving, Leadership",
                    MoTa = @"<h4>Mô tả công việc:</h4>
                        <ul>
                            <li>Phát triển và duy trì các ứng dụng web full-stack sử dụng React và .NET Core</li>
                            <li>Thiết kế và triển khai RESTful APIs</li>
                            <li>Tối ưu hóa hiệu suất ứng dụng và database</li>
                            <li>Code review và mentoring các junior developers</li>
                            <li>Tham gia vào các buổi họp kỹ thuật và planning sessions</li>
                        </ul>",
                    YeuCau = @"<h4>Yêu cầu ứng viên:</h4>
                        <ul>
                            <li>Có ít nhất 5 năm kinh nghiệm làm việc với React và .NET Core</li>
                            <li>Thành thạo JavaScript/TypeScript, C#, SQL Server</li>
                            <li>Có kinh nghiệm với RESTful API, Microservices</li>
                            <li>Hiểu biết về Git, CI/CD, Docker</li>
                            <li>Kỹ năng giao tiếp tốt và làm việc nhóm</li>
                            <li>Tiếng Anh tốt (đọc hiểu tài liệu kỹ thuật)</li>
                        </ul>",
                    MucLuongThapNhat = 25,
                    MucLuongCaoNhat = 40,
                    QuyenLoi = @"<ul>
                        <li>Bảo hiểm sức khỏe đầy đủ cho nhân viên và gia đình</li>
                        <li>Thưởng Tết, thưởng hiệu suất hàng quý</li>
                        <li>Du lịch hàng năm trong và ngoài nước</li>
                        <li>Chế độ làm việc linh hoạt, work from home 2 ngày/tuần</li>
                        <li>Đào tạo và phát triển kỹ năng chuyên môn</li>
                    </ul>",
                    NguoiLienHe = "Nguyen Van An",
                    EmailLienHe = "recruit@fpt.com.vn",
                    SDTLienHe = "024-37912345",
                    TinhThanhPho = "Ha Noi",
                    PhuongXa = "Cau Giay",
                    DiaChiLamViec = "Keangnam Landmark Tower, Ha Noi",
                    AnhVanPhong = "",
                    SoLuongUngTuyen = 45,
                    NgayDang = DateTime.Now.AddDays(-5),
                    HanNop = DateTime.Now.AddDays(25),
                    Latitude = 21.028511,
                    Longitude = 105.804817
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
                    HanNop = DateTime.Now.AddDays(27),
                    Latitude = 10.762622,
                    Longitude = 106.660172
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
                    NganhNghe = "Công nghệ thông tin",
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
                    NganhNghe = "Thiết kế đồ họa",
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
                    NganhNghe = "Công nghệ thông tin",
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
                    NganhNghe = "Nhân sự",
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
                    NganhNghe = "Truyền thông - Media",
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
                    NganhNghe = "Công nghệ thông tin",
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
                    NganhNghe = "Công nghệ thông tin",
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
                },
                new TinTuyenDung
                {
                    MaTinTuyenDung = 13,
                    TenViecLam = "Intern - Marketing & Social Media",
                    CongTy = "VNG Corporation",
                    NganhNghe = "Truyền thông - Media",
                    NganhNgheChiTiet = "Marketing",
                    LoaiCongViec = "Internship",
                    KinhNghiem = "Khong yeu cau",
                    ViTri = "Nhan vien",
                    NgoaiNgu = "Tieng Anh",
                    TuKhoa = "Social Media, Content Marketing, Facebook Ads",
                    KyNang = "Creative Thinking, Communication, Teamwork",
                    MoTa = @"<h4>Mô tả công việc:</h4>
                        <ul>
                            <li>Hỗ trợ xây dựng và triển khai các chiến dịch marketing trên social media</li>
                            <li>Viết content cho Facebook, Instagram, TikTok</li>
                            <li>Theo dõi và phân tích hiệu quả các chiến dịch marketing</li>
                            <li>Hỗ trợ tổ chức các sự kiện, event của công ty</li>
                        </ul>",
                    YeuCau = @"<h4>Yêu cầu ứng viên:</h4>
                        <ul>
                            <li>Sinh viên năm 3, năm 4 hoặc mới tốt nghiệp các ngành Marketing, Truyền thông, Báo chí</li>
                            <li>Có kiến thức cơ bản về Marketing, Social Media</li>
                            <li>Có khả năng viết content, sáng tạo ý tưởng</li>
                            <li>Yêu thích và thường xuyên sử dụng các mạng xã hội</li>
                            <li>Có thể làm việc full-time tối thiểu 3 tháng</li>
                        </ul>",
                    MucLuongThapNhat = 4,
                    MucLuongCaoNhat = 6,
                    QuyenLoi = @"<ul>
                        <li>Thực tập tại môi trường công ty công nghệ hàng đầu Việt Nam</li>
                        <li>Được đào tạo bài bản về Marketing và Social Media</li>
                        <li>Cơ hội trở thành nhân viên chính thức sau thực tập</li>
                        <li>Phụ cấp ăn trưa, xe bus đưa đón</li>
                    </ul>",
                    NguoiLienHe = "Tran Thanh Tam",
                    EmailLienHe = "intern@vng.com.vn",
                    SDTLienHe = "028-54458888",
                    TinhThanhPho = "TP Ho Chi Minh",
                    PhuongXa = "Tan Binh",
                    DiaChiLamViec = "Z06, Street No. 13, Tan Thuan Dong Ward, District 7, HCMC",
                    AnhVanPhong = "",
                    SoLuongUngTuyen = 142,
                    NgayDang = DateTime.Now.AddDays(-1),
                    HanNop = DateTime.Now.AddDays(29)
                },
                new TinTuyenDung
                {
                    MaTinTuyenDung = 14,
                    TenViecLam = "Intern - UI/UX Designer",
                    CongTy = "Tiki Corporation",
                    NganhNghe = "Thiết kế đồ họa",
                    NganhNgheChiTiet = "UI/UX Designer",
                    LoaiCongViec = "Internship",
                    KinhNghiem = "Khong yeu cau",
                    ViTri = "Nhan vien",
                    NgoaiNgu = "Tieng Anh",
                    TuKhoa = "Figma, Adobe XD, UI Design, UX Research",
                    KyNang = "Design Thinking, Attention to Detail, Creative",
                    MoTa = @"<h4>Mô tả công việc:</h4>
                        <ul>
                            <li>Hỗ trợ thiết kế giao diện cho ứng dụng mobile và web</li>
                            <li>Tham gia nghiên cứu trải nghiệm người dùng (UX Research)</li>
                            <li>Tạo wireframe, prototype cho các tính năng mới</li>
                            <li>Làm việc với team Product và Developer để hoàn thiện sản phẩm</li>
                        </ul>",
                    YeuCau = @"<h4>Yêu cầu ứng viên:</h4>
                        <ul>
                            <li>Sinh viên năm cuối hoặc mới tốt nghiệp các ngành Thiết kế, Mỹ thuật, IT</li>
                            <li>Có kiến thức về UI/UX Design</li>
                            <li>Thành thạo Figma hoặc Adobe XD, Photoshop, Illustrator</li>
                            <li>Có portfolio thể hiện các dự án đã làm</li>
                            <li>Có thể làm việc full-time tối thiểu 4 tháng</li>
                        </ul>",
                    MucLuongThapNhat = 5,
                    MucLuongCaoNhat = 8,
                    QuyenLoi = @"<ul>
                        <li>Làm việc với team Design chuyên nghiệp, học hỏi từ các Senior Designer</li>
                        <li>Tham gia các dự án thực tế của Tiki</li>
                        <li>Cơ hội trở thành nhân viên chính thức</li>
                        <li>Được đào tạo về Design System, UX Research</li>
                        <li>Môi trường làm việc năng động, sáng tạo</li>
                    </ul>",
                    NguoiLienHe = "Hoang Minh Chau",
                    EmailLienHe = "recruitment@tiki.vn",
                    SDTLienHe = "024-73007777",
                    TinhThanhPho = "Ha Noi",
                    PhuongXa = "Cau Giay",
                    DiaChiLamViec = "Tiki Building, 52 Ut Tich, Tan Thinh, Tan Binh, HCMC",
                    AnhVanPhong = "",
                    SoLuongUngTuyen = 89,
                    NgayDang = DateTime.Now.AddDays(-3),
                    HanNop = DateTime.Now.AddDays(27)
                },
                new TinTuyenDung
                {
                    MaTinTuyenDung = 15,
                    TenViecLam = "Intern - Data Analyst",
                    CongTy = "Grab Vietnam",
                    NganhNghe = "Công nghệ thông tin",
                    NganhNgheChiTiet = "Data Analyst",
                    LoaiCongViec = "Internship",
                    KinhNghiem = "Khong yeu cau",
                    ViTri = "Nhan vien",
                    NgoaiNgu = "Tieng Anh",
                    TuKhoa = "Python, SQL, Excel, Data Analysis",
                    KyNang = "Analytical Thinking, Problem Solving, Teamwork",
                    MoTa = @"<h4>Mô tả công việc:</h4>
                        <ul>
                            <li>Thu thập, xử lý và phân tích dữ liệu kinh doanh</li>
                            <li>Tạo báo cáo, dashboard để hỗ trợ ra quyết định</li>
                            <li>Hỗ trợ team Data trong các dự án phân tích dữ liệu</li>
                            <li>Tìm hiểu và áp dụng các công cụ phân tích dữ liệu mới</li>
                        </ul>",
                    YeuCau = @"<h4>Yêu cầu ứng viên:</h4>
                        <ul>
                            <li>Sinh viên năm 3, năm 4 các ngành CNTT, Toán, Thống kê, Kinh tế</li>
                            <li>Có kiến thức cơ bản về SQL, Python hoặc R</li>
                            <li>Biết sử dụng Excel ở mức độ nâng cao</li>
                            <li>Có tư duy logic, phân tích tốt</li>
                            <li>Ưu tiên có kinh nghiệm với Power BI, Tableau</li>
                        </ul>",
                    MucLuongThapNhat = 6,
                    MucLuongCaoNhat = 9,
                    QuyenLoi = @"<ul>
                        <li>Được đào tạo về Data Analysis, Big Data</li>
                        <li>Làm việc với dữ liệu thực tế quy mô lớn</li>
                        <li>Môi trường làm việc quốc tế, chuyên nghiệp</li>
                        <li>Cơ hội chuyển đổi thành nhân viên chính thức</li>
                        <li>Hỗ trợ ăn trưa, team building</li>
                    </ul>",
                    NguoiLienHe = "Le Thi Hong",
                    EmailLienHe = "careers.vn@grab.com",
                    SDTLienHe = "028-66639999",
                    TinhThanhPho = "TP Ho Chi Minh",
                    PhuongXa = "Binh Thanh",
                    DiaChiLamViec = "Flemington Tower, 182 Le Dai Hanh, Ward 15, District 11, HCMC",
                    AnhVanPhong = "",
                    SoLuongUngTuyen = 95,
                    NgayDang = DateTime.Now.AddDays(-4),
                    HanNop = DateTime.Now.AddDays(26)
                },
                new TinTuyenDung
                {
                    MaTinTuyenDung = 16,
                    TenViecLam = "Intern - Business Analyst",
                    CongTy = "Momo E-Wallet",
                    NganhNghe = "Kinh doanh",
                    NganhNgheChiTiet = "Business Analyst",
                    LoaiCongViec = "Internship",
                    KinhNghiem = "Khong yeu cau",
                    ViTri = "Nhan vien",
                    NgoaiNgu = "Tieng Anh",
                    TuKhoa = "Business Analysis, Market Research, Strategy",
                    KyNang = "Critical Thinking, Communication, Presentation",
                    MoTa = @"<h4>Mô tả công việc:</h4>
                        <ul>
                            <li>Phân tích thị trường, nghiên cứu đối thủ cạnh tranh</li>
                            <li>Hỗ trợ xây dựng và đánh giá các chiến lược kinh doanh</li>
                            <li>Làm việc với các team Product để hiểu yêu cầu nghiệp vụ</li>
                            <li>Tạo báo cáo phân tích và trình bày kết quả cho leadership</li>
                        </ul>",
                    YeuCau = @"<h4>Yêu cầu ứng viên:</h4>
                        <ul>
                            <li>Sinh viên năm cuối các ngành Kinh tế, Quản trị kinh doanh, CNTT</li>
                            <li>Có tư duy phân tích, logic tốt</li>
                            <li>Kỹ năng Excel, PowerPoint thành thạo</li>
                            <li>Tiếng Anh giao tiếp tốt</li>
                            <li>Có khả năng làm việc độc lập và teamwork</li>
                        </ul>",
                    MucLuongThapNhat = 5,
                    MucLuongCaoNhat = 7,
                    QuyenLoi = @"<ul>
                        <li>Được đào tạo về Business Analysis, Product Development</li>
                        <li>Làm việc trong lĩnh vực Fintech hàng đầu Việt Nam</li>
                        <li>Cơ hội networking với các chuyên gia trong ngành</li>
                        <li>Được xem xét trở thành nhân viên chính thức</li>
                        <li>Chế độ phúc lợi tốt, môi trường năng động</li>
                    </ul>",
                    NguoiLienHe = "Pham Duc Anh",
                    EmailLienHe = "hr@momo.vn",
                    SDTLienHe = "028-71098888",
                    TinhThanhPho = "TP Ho Chi Minh",
                    PhuongXa = "Binh Thanh",
                    DiaChiLamViec = "Lim Tower 3, 29A Nguyen Dinh Chieu, Da Kao, District 1, HCMC",
                    AnhVanPhong = "",
                    SoLuongUngTuyen = 76,
                    NgayDang = DateTime.Now.AddDays(-6),
                    HanNop = DateTime.Now.AddDays(24)
                },
                new TinTuyenDung
                {
                    MaTinTuyenDung = 17,
                    TenViecLam = "Intern - Mobile App Developer",
                    CongTy = "Zalo - VNG",
                    NganhNghe = "Công nghệ thông tin",
                    NganhNgheChiTiet = "Mobile Developer",
                    LoaiCongViec = "Internship",
                    KinhNghiem = "Khong yeu cau",
                    ViTri = "Nhan vien",
                    NgoaiNgu = "Tieng Anh",
                    TuKhoa = "Flutter, React Native, iOS, Android",
                    KyNang = "Programming, Problem Solving, Learning Agility",
                    MoTa = @"<h4>Mô tả công việc:</h4>
                        <ul>
                            <li>Phát triển tính năng mới cho ứng dụng Zalo</li>
                            <li>Sửa lỗi, tối ưu hiệu năng ứng dụng mobile</li>
                            <li>Làm việc với team để tích hợp API và xây dựng giao diện</li>
                            <li>Tham gia code review và học hỏi từ các senior developers</li>
                        </ul>",
                    YeuCau = @"<h4>Yêu cầu ứng viên:</h4>
                        <ul>
                            <li>Sinh viên năm 3, năm 4 hoặc mới tốt nghiệp các ngành CNTT</li>
                            <li>Có kiến thức về lập trình mobile (Flutter, React Native hoặc Native iOS/Android)</li>
                            <li>Có kiến thức cơ bản về OOP, Data Structures, Algorithms</li>
                            <li>Có passion với mobile development</li>
                            <li>Có thể làm việc full-time tối thiểu 5 tháng</li>
                        </ul>",
                    MucLuongThapNhat = 7,
                    MucLuongCaoNhat = 10,
                    QuyenLoi = @"<ul>
                        <li>Được đào tạo bài bản về Mobile Development</li>
                        <li>Tham gia phát triển ứng dụng có hàng chục triệu người dùng</li>
                        <li>Làm việc với các công nghệ hiện đại nhất</li>
                        <li>Cơ hội cao trở thành nhân viên chính thức</li>
                        <li>Môi trường làm việc trẻ trung, sáng tạo</li>
                    </ul>",
                    NguoiLienHe = "Nguyen Hoang Nam",
                    EmailLienHe = "intern.zalo@vng.com.vn",
                    SDTLienHe = "028-54458888",
                    TinhThanhPho = "TP Ho Chi Minh",
                    PhuongXa = "Tan Binh",
                    DiaChiLamViec = "VNG Campus, Z06 Street No. 13, Tan Thuan Dong, District 7, HCMC",
                    AnhVanPhong = "",
                    SoLuongUngTuyen = 158,
                    NgayDang = DateTime.Now.AddDays(-2),
                    HanNop = DateTime.Now.AddDays(28)
                }
            };
        }

        public static TinTuyenDung? LayTinTuyenDungTheoId(int id)
        {
            var danhSach = LayDanhSachTinTuyenDung();
            return danhSach.FirstOrDefault(t => t.MaTinTuyenDung == id);
        }

        // Method to filter internship positions
        public static List<TinTuyenDung> LayDanhSachThucTap()
        {
            var danhSach = LayDanhSachTinTuyenDung();
            return danhSach.Where(t => t.LoaiCongViec.Equals("Internship", StringComparison.OrdinalIgnoreCase)).ToList();
        }

        // Method to get jobs by company name
        public static List<TinTuyenDung> LayDanhSachTheoCongTy(string tenCongTy)
        {
            var danhSach = LayDanhSachTinTuyenDung();
            return danhSach.Where(t => t.CongTy.Equals(tenCongTy, StringComparison.OrdinalIgnoreCase)).ToList();
        }
    }
}

