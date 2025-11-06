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
                    KinhNghiem = "Trên 5 năm",
                    ViTri = "Trưởng nhóm",
                    NgoaiNgu = "Tiếng Anh",
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
                    NguoiLienHe = "Nguyễn Văn An",
                    EmailLienHe = "recruit@fpt.com.vn",
                    SDTLienHe = "024-37912345",
                    TinhThanhPho = "Hà Nội",
                    PhuongXa = "Cầu Giấy",
                    DiaChiLamViec = "Keangnam Landmark Tower, Hà Nội",
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
                    KinhNghiem = "2-5 năm",
                    ViTri = "Quản lý",
                    NgoaiNgu = "Tiếng Anh",
                    TuKhoa = "Marketing, Digital Marketing, Brand Management",
                    KyNang = "Communication, Leadership, Creative Thinking",
                    MoTa = "Quản lý chiến lược marketing và phát triển thương hiệu",
                    YeuCau = "Có kinh nghiệm quản lý đội ngũ và phát triển chiến lược marketing",
                    MucLuongThapNhat = 20,
                    MucLuongCaoNhat = 35,
                    QuyenLoi = "Bảo hiểm xã hội, thưởng theo hiệu suất",
                    NguoiLienHe = "Trần Thị Bình",
                    EmailLienHe = "hr@vinamilk.com.vn",
                    SDTLienHe = "028-54155555",
                    TinhThanhPho = "TP Hồ Chí Minh",
                    PhuongXa = "Tân Phú",
                    DiaChiLamViec = "10 Tân Trào, Tân Phú, TP HCM",
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
                    NganhNghe = "Kế toán - Tài chính",
                    NganhNgheChiTiet = "",
                    LoaiCongViec = "Full-time",
                    KinhNghiem = "1-2 năm",
                    ViTri = "Nhân viên",
                    NgoaiNgu = "Tiếng Anh",
                    TuKhoa = "Accounting, Financial Report, Excel",
                    KyNang = "Attention to Detail, Analytical Skills",
                    MoTa = "Xử lý các nghiệp vụ kế toán tổng hợp, báo cáo tài chính",
                    YeuCau = "Tốt nghiệp đại học chuyên ngành kế toán, biết tiếng Anh",
                    MucLuongThapNhat = 12,
                    MucLuongCaoNhat = 18,
                    QuyenLoi = "Bảo hiểm y tế, thưởng cuối năm",
                    NguoiLienHe = "Trịnh Thị Mai",
                    EmailLienHe = "hrvietnam@deloitte.com",
                    SDTLienHe = "028-39101100",
                    TinhThanhPho = "Hà Nội",
                    PhuongXa = "Hoàn Kiếm",
                    DiaChiLamViec = "Saigon Centre Tower, Hà Nội",
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
                    KinhNghiem = "2-5 năm",
                    ViTri = "Nhân viên",
                    NgoaiNgu = "Tiếng Anh",
                    TuKhoa = "ReactJS, JavaScript, HTML/CSS",
                    KyNang = "Problem Solving, Teamwork",
                    MoTa = "Phát triển giao diện người dùng cho website thương mại điện tử",
                    YeuCau = "Thông thạo ReactJS, JavaScript ES6+, có kinh nghiệm REST API",
                    MucLuongThapNhat = 18,
                    MucLuongCaoNhat = 30,
                    QuyenLoi = "Bảo hiểm full, team building, đào tạo",
                    NguoiLienHe = "Lê Văn Cường",
                    EmailLienHe = "career@tiki.vn",
                    SDTLienHe = "028-62728728",
                    TinhThanhPho = "TP Hồ Chí Minh",
                    PhuongXa = "Tân Bình",
                    DiaChiLamViec = "52 Út Tịch, Tân Bình, TP HCM",
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
                    KinhNghiem = "2-5 năm",
                    ViTri = "Nhân viên",
                    NgoaiNgu = "Tiếng Anh",
                    TuKhoa = "Figma, Adobe XD, UI/UX",
                    KyNang = "Creative Thinking, User Research",
                    MoTa = "Thiết kế giao diện và trải nghiệm người dùng cho các sản phẩm game và app",
                    YeuCau = "Có portfolio UI/UX tốt, thông thạo Figma/Adobe XD",
                    MucLuongThapNhat = 15,
                    MucLuongCaoNhat = 25,
                    QuyenLoi = "Bảo hiểm, thưởng dự án, môi trường trẻ trung",
                    NguoiLienHe = "Phạm Thị Dung",
                    EmailLienHe = "jobs@vng.com.vn",
                    SDTLienHe = "028-54587262",
                    TinhThanhPho = "TP Hồ Chí Minh",
                    PhuongXa = "Quận 7",
                    DiaChiLamViec = "Z06 Street, Tân Thuận EPZ, Q7, TP HCM",
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
                    KinhNghiem = "1-2 năm",
                    ViTri = "Nhân viên",
                    NgoaiNgu = "Tiếng Anh",
                    TuKhoa = "SQL, Python, Data Visualization",
                    KyNang = "Analytical Skills, Attention to Detail",
                    MoTa = "Phân tích dữ liệu kinh doanh, tạo báo cáo và dashboard",
                    YeuCau = "Biết SQL, Python, có kiến thức về Data Visualization",
                    MucLuongThapNhat = 15,
                    MucLuongCaoNhat = 28,
                    QuyenLoi = "Bảo hiểm, đào tạo chuyên môn",
                    NguoiLienHe = "Hoàng Văn Dũng",
                    EmailLienHe = "recruitment.vn@lazada.com",
                    SDTLienHe = "028-39107788",
                    TinhThanhPho = "Hà Nội",
                    PhuongXa = "Hoàn Kiếm",
                    DiaChiLamViec = "Keangnam Tower, Hà Nội",
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
                    KinhNghiem = "Trên 5 năm",
                    ViTri = "Quản lý",
                    NgoaiNgu = "Tiếng Anh",
                    TuKhoa = "HR Management, Recruitment, Training",
                    KyNang = "Leadership, Communication",
                    MoTa = "Quản lý tuyển dụng, đào tạo và phát triển nhân sự",
                    YeuCau = "Có kinh nghiệm quản lý nhân sự trong công ty lớn",
                    MucLuongThapNhat = 20,
                    MucLuongCaoNhat = 30,
                    QuyenLoi = "Bảo hiểm full, thưởng hiệu suất cao",
                    NguoiLienHe = "Võ Thị Hương",
                    EmailLienHe = "tuyendung@viettel.com.vn",
                    SDTLienHe = "024-39779898",
                    TinhThanhPho = "Hà Nội",
                    PhuongXa = "Ba Đình",
                    DiaChiLamViec = "Tòa nhà Viettel, Giảng Võ, Ba Đình, Hà Nội",
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
                    KinhNghiem = "1-2 năm",
                    ViTri = "Nhân viên",
                    NgoaiNgu = "Tiếng Anh",
                    TuKhoa = "Content Writing, SEO, Social Media",
                    KyNang = "Writing Skills, Creativity",
                    MoTa = "Viết bài và biên tập nội dung cho website tin tức",
                    YeuCau = "Kỹ năng viết tốt, biết SEO cơ bản",
                    MucLuongThapNhat = 10,
                    MucLuongCaoNhat = 15,
                    QuyenLoi = "Bảo hiểm, thưởng theo bài viết",
                    NguoiLienHe = "Đặng Văn Giàu",
                    EmailLienHe = "hr@vnexpress.net",
                    SDTLienHe = "024-37576666",
                    TinhThanhPho = "Hà Nội",
                    PhuongXa = "Hoàn Kiếm",
                    DiaChiLamViec = "123 Nguyễn Thị Minh Khai, Hà Nội",
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
                    KinhNghiem = "2-5 năm",
                    ViTri = "Nhân viên",
                    NgoaiNgu = "Tiếng Anh",
                    TuKhoa = "Business Analysis, Requirements, Process Improvement",
                    KyNang = "Analytical Skills, Communication",
                    MoTa = "Phân tích yêu cầu kinh doanh và tối ưu quy trình",
                    YeuCau = "Có kinh nghiệm phân tích quy trình và làm việc với stakeholders",
                    MucLuongThapNhat = 18,
                    MucLuongCaoNhat = 32,
                    QuyenLoi = "Bảo hiểm, thưởng dự án, phúc lợi tốt",
                    NguoiLienHe = "Bùi Thị Hoa",
                    EmailLienHe = "career@momo.vn",
                    SDTLienHe = "028-71089999",
                    TinhThanhPho = "TP Hồ Chí Minh",
                    PhuongXa = "Quận 4",
                    DiaChiLamViec = "Lim Tower, 9-11 Tôn Đản, Q4, TP HCM",
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
                    KinhNghiem = "2-5 năm",
                    ViTri = "Nhân viên",
                    NgoaiNgu = "Tiếng Anh",
                    TuKhoa = "Java, Spring Boot, Microservices",
                    KyNang = "Problem Solving, System Design",
                    MoTa = "Phát triển hệ thống backend cho các ứng dụng di động",
                    YeuCau = "Thông thạo Java, Spring Boot, có kinh nghiệm Microservices",
                    MucLuongThapNhat = 22,
                    MucLuongCaoNhat = 38,
                    QuyenLoi = "Bảo hiểm full, thưởng dự án lớn, đào tạo",
                    NguoiLienHe = "Ngô Văn Khánh",
                    EmailLienHe = "recruit.vn@samsung.com",
                    SDTLienHe = "024-39366666",
                    TinhThanhPho = "Hà Nội",
                    PhuongXa = "Cầu Giấy",
                    DiaChiLamViec = "Keangnam Landmark Tower, Hà Nội",
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
                    KinhNghiem = "Không yêu cầu",
                    ViTri = "Nhân viên",
                    NgoaiNgu = "Tiếng Anh",
                    TuKhoa = "Java, Python, C#",
                    KyNang = "Eager to Learn, Teamwork",
                    MoTa = "Thực tập phát triển phần mềm, hỗ trợ team backend",
                    YeuCau = "Sinh viên năm cuối hoặc mới tốt nghiệp, có kiến thức lập trình cơ bản",
                    MucLuongThapNhat = 5,
                    MucLuongCaoNhat = 7,
                    QuyenLoi = "Đào tạo, cơ hội trở thành nhân viên chính thức",
                    NguoiLienHe = "Dương Thị Lan",
                    EmailLienHe = "careers.vn@shopee.com",
                    SDTLienHe = "028-73088888",
                    TinhThanhPho = "TP Hồ Chí Minh",
                    PhuongXa = "Bình Thạnh",
                    DiaChiLamViec = "Pearl Plaza, 561A Điện Biên Phủ, Bình Thạnh, TP HCM",
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
                    KinhNghiem = "Trên 5 năm",
                    ViTri = "Quản lý",
                    NgoaiNgu = "Tiếng Anh",
                    TuKhoa = "Sales Management, Business Development, Customer Relations",
                    KyNang = "Leadership, Negotiation, Communication",
                    MoTa = "Quản lý đội ngũ bán hàng và phát triển kênh phân phối",
                    YeuCau = "Có kinh nghiệm quản lý đội ngũ bán hàng, phát triển kênh phân phối",
                    MucLuongThapNhat = 15,
                    MucLuongCaoNhat = 25,
                    QuyenLoi = "Bảo hiểm, hoa hồng hấp dẫn, thưởng theo doanh số",
                    NguoiLienHe = "Lý Văn Long",
                    EmailLienHe = "recruitment@highlandscoffee.com.vn",
                    SDTLienHe = "028-39118888",
                    TinhThanhPho = "Hà Nội",
                    PhuongXa = "Hoàn Kiếm",
                    DiaChiLamViec = "24 Nguyễn Huệ, Hoàn Kiếm, Hà Nội",
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
                    KinhNghiem = "Không yêu cầu",
                    ViTri = "Nhân viên",
                    NgoaiNgu = "Tiếng Anh",
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
                    NguoiLienHe = "Trần Thanh Tâm",
                    EmailLienHe = "intern@vng.com.vn",
                    SDTLienHe = "028-54458888",
                    TinhThanhPho = "TP Hồ Chí Minh",
                    PhuongXa = "Tân Bình",
                    DiaChiLamViec = "Z06, Street No. 13, Tân Thuận Đông Ward, District 7, HCMC",
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
                    KinhNghiem = "Không yêu cầu",
                    ViTri = "Nhân viên",
                    NgoaiNgu = "Tiếng Anh",
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
                    NguoiLienHe = "Hoàng Minh Châu",
                    EmailLienHe = "recruitment@tiki.vn",
                    SDTLienHe = "024-73007777",
                    TinhThanhPho = "Hà Nội",
                    PhuongXa = "Cầu Giấy",
                    DiaChiLamViec = "Tiki Building, 52 Út Tịch, Tân Thịnh, Tân Bình, HCMC",
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
                    KinhNghiem = "Không yêu cầu",
                    ViTri = "Nhân viên",
                    NgoaiNgu = "Tiếng Anh",
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
                    NguoiLienHe = "Lê Thị Hồng",
                    EmailLienHe = "careers.vn@grab.com",
                    SDTLienHe = "028-66639999",
                    TinhThanhPho = "TP Hồ Chí Minh",
                    PhuongXa = "Bình Thạnh",
                    DiaChiLamViec = "Flemington Tower, 182 Lê Đại Hành, Ward 15, District 11, HCMC",
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
                    KinhNghiem = "Không yêu cầu",
                    ViTri = "Nhân viên",
                    NgoaiNgu = "Tiếng Anh",
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
                    NguoiLienHe = "Phạm Đức Anh",
                    EmailLienHe = "hr@momo.vn",
                    SDTLienHe = "028-71098888",
                    TinhThanhPho = "TP Hồ Chí Minh",
                    PhuongXa = "Bình Thạnh",
                    DiaChiLamViec = "Lim Tower 3, 29A Nguyễn Đình Chiểu, Đa Kao, District 1, HCMC",
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
                    KinhNghiem = "Không yêu cầu",
                    ViTri = "Nhân viên",
                    NgoaiNgu = "Tiếng Anh",
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
                    NguoiLienHe = "Nguyễn Hoàng Nam",
                    EmailLienHe = "intern.zalo@vng.com.vn",
                    SDTLienHe = "028-54458888",
                    TinhThanhPho = "TP Hồ Chí Minh",
                    PhuongXa = "Tân Bình",
                    DiaChiLamViec = "VNG Campus, Z06 Street No. 13, Tân Thuận Đông, District 7, HCMC",
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

