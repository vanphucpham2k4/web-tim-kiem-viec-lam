using Unicareer.Models;

namespace Unicareer.Repository
{
    public class MockTinTuyenDungRepository : ITinTuyenDungRepository
    {
        private readonly List<TinTuyenDung> _danhSachTinTuyenDung;

        public MockTinTuyenDungRepository()
        {
            _danhSachTinTuyenDung = new List<TinTuyenDung>
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
                    NganhNgheChiTiet = "Marketing",
                    LoaiCongViec = "Full-time",
                    KinhNghiem = "2-5 năm",
                    ViTri = "Quản lý",
                    NgoaiNgu = "Tiếng Anh",
                    TuKhoa = "Marketing, Digital Marketing, Brand Management",
                    KyNang = "Communication, Leadership, Creative Thinking",
                    MoTa = @"<h4>Mô tả công việc:</h4>
                        <ul>
                            <li>Quản lý chiến lược marketing và phát triển thương hiệu Vinamilk</li>
                            <li>Xây dựng và triển khai các chiến dịch marketing trên nhiều kênh</li>
                            <li>Phân tích thị trường và đối thủ cạnh tranh</li>
                            <li>Quản lý ngân sách marketing và đánh giá hiệu quả chiến dịch</li>
                            <li>Làm việc với các agency và đối tác truyền thông</li>
                        </ul>",
                    YeuCau = @"<h4>Yêu cầu ứng viên:</h4>
                        <ul>
                            <li>Có kinh nghiệm quản lý đội ngũ và phát triển chiến lược marketing</li>
                            <li>Thành thạo Digital Marketing, SEO, Social Media Marketing</li>
                            <li>Có khả năng phân tích dữ liệu và đưa ra quyết định chiến lược</li>
                            <li>Kỹ năng giao tiếp và thuyết trình tốt</li>
                            <li>Tiếng Anh giao tiếp tốt</li>
                        </ul>",
                    MucLuongThapNhat = 20,
                    MucLuongCaoNhat = 35,
                    QuyenLoi = @"<ul>
                        <li>Bảo hiểm xã hội đầy đủ</li>
                        <li>Thưởng theo hiệu suất và doanh số</li>
                        <li>Đào tạo và phát triển kỹ năng quản lý</li>
                        <li>Môi trường làm việc chuyên nghiệp</li>
                    </ul>",
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
                    MoTa = @"<h4>Mô tả công việc:</h4>
                        <ul>
                            <li>Phát triển giao diện người dùng cho website thương mại điện tử Tiki</li>
                            <li>Tối ưu hóa hiệu suất và trải nghiệm người dùng</li>
                            <li>Làm việc với team Backend để tích hợp API</li>
                            <li>Code review và chia sẻ kiến thức với team</li>
                            <li>Tham gia vào quá trình thiết kế và phát triển tính năng mới</li>
                        </ul>",
                    YeuCau = @"<h4>Yêu cầu ứng viên:</h4>
                        <ul>
                            <li>Thông thạo ReactJS, JavaScript ES6+</li>
                            <li>Có kinh nghiệm với REST API và state management</li>
                            <li>Hiểu biết về HTML/CSS, responsive design</li>
                            <li>Có kinh nghiệm với Git, code review</li>
                            <li>Kỹ năng làm việc nhóm tốt</li>
                        </ul>",
                    MucLuongThapNhat = 18,
                    MucLuongCaoNhat = 30,
                    QuyenLoi = @"<ul>
                        <li>Bảo hiểm full cho nhân viên và gia đình</li>
                        <li>Team building và các hoạt động văn hóa công ty</li>
                        <li>Đào tạo kỹ thuật và phát triển nghề nghiệp</li>
                        <li>Môi trường làm việc năng động, trẻ trung</li>
                    </ul>",
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
                    MaTinTuyenDung = 4,
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
                    MoTa = @"<h4>Mô tả công việc:</h4>
                        <ul>
                            <li>Thiết kế giao diện và trải nghiệm người dùng cho các sản phẩm game và app của VNG</li>
                            <li>Thực hiện user research và usability testing</li>
                            <li>Tạo wireframe, prototype và design system</li>
                            <li>Làm việc với team Product và Developer để hiện thực hóa thiết kế</li>
                            <li>Tham gia vào quá trình cải tiến sản phẩm liên tục</li>
                        </ul>",
                    YeuCau = @"<h4>Yêu cầu ứng viên:</h4>
                        <ul>
                            <li>Có portfolio UI/UX tốt thể hiện quá trình thiết kế</li>
                            <li>Thông thạo Figma/Adobe XD, Photoshop, Illustrator</li>
                            <li>Hiểu biết về Design System, Material Design</li>
                            <li>Có kinh nghiệm với user research và usability testing</li>
                            <li>Kỹ năng giao tiếp và trình bày tốt</li>
                        </ul>",
                    MucLuongThapNhat = 15,
                    MucLuongCaoNhat = 25,
                    QuyenLoi = @"<ul>
                        <li>Bảo hiểm đầy đủ, thưởng dự án</li>
                        <li>Môi trường làm việc trẻ trung, sáng tạo</li>
                        <li>Được làm việc với các sản phẩm có hàng triệu người dùng</li>
                        <li>Đào tạo và phát triển kỹ năng thiết kế</li>
                    </ul>",
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
                    MaTinTuyenDung = 5,
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
                }
            };
        }

        public List<TinTuyenDung> LayDanhSachTinTuyenDung()
        {
            return _danhSachTinTuyenDung;
        }

        public TinTuyenDung? LayTinTuyenDungTheoId(int id)
        {
            return _danhSachTinTuyenDung.FirstOrDefault(t => t.MaTinTuyenDung == id);
        }

        public List<TinTuyenDung> LayDanhSachThucTap()
        {
            return _danhSachTinTuyenDung.Where(t => t.LoaiCongViec.Equals("Internship", StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public List<TinTuyenDung> LayDanhSachTheoCongTy(string tenCongTy)
        {
            return _danhSachTinTuyenDung.Where(t => t.CongTy.Equals(tenCongTy, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public TinTuyenDung ThemTinTuyenDung(TinTuyenDung tinTuyenDung)
        {
            // Tạo mã tin mới (tăng dần từ số lớn nhất hiện có)
            var maxId = _danhSachTinTuyenDung.Any() ? _danhSachTinTuyenDung.Max(t => t.MaTinTuyenDung) : 0;
            tinTuyenDung.MaTinTuyenDung = maxId + 1;
            
            // Set ngày đăng là ngày hiện tại
            tinTuyenDung.NgayDang = DateTime.Now;
            
            // Khởi tạo số lượng ứng tuyển = 0
            tinTuyenDung.SoLuongUngTuyen = 0;
            
            // Thêm vào danh sách
            _danhSachTinTuyenDung.Add(tinTuyenDung);
            
            return tinTuyenDung;
        }

        public TinTuyenDung? CapNhatTinTuyenDung(int id, TinTuyenDung tinTuyenDung)
        {
            var tinHienTai = _danhSachTinTuyenDung.FirstOrDefault(t => t.MaTinTuyenDung == id);
            if (tinHienTai == null)
            {
                return null;
            }

            // Cập nhật các trường có thể chỉnh sửa
            tinHienTai.TenViecLam = tinTuyenDung.TenViecLam;
            tinHienTai.NganhNghe = tinTuyenDung.NganhNghe;
            tinHienTai.NganhNgheChiTiet = tinTuyenDung.NganhNgheChiTiet;
            tinHienTai.LoaiCongViec = tinTuyenDung.LoaiCongViec;
            tinHienTai.KinhNghiem = tinTuyenDung.KinhNghiem;
            tinHienTai.ViTri = tinTuyenDung.ViTri;
            tinHienTai.NgoaiNgu = tinTuyenDung.NgoaiNgu;
            tinHienTai.KyNang = tinTuyenDung.KyNang;
            tinHienTai.MoTa = tinTuyenDung.MoTa;
            tinHienTai.YeuCau = tinTuyenDung.YeuCau;
            tinHienTai.QuyenLoi = tinTuyenDung.QuyenLoi;
            tinHienTai.MucLuongThapNhat = tinTuyenDung.MucLuongThapNhat;
            tinHienTai.MucLuongCaoNhat = tinTuyenDung.MucLuongCaoNhat;
            tinHienTai.DiaChiLamViec = tinTuyenDung.DiaChiLamViec;
            tinHienTai.PhuongXa = tinTuyenDung.PhuongXa;
            tinHienTai.TinhThanhPho = tinTuyenDung.TinhThanhPho;
            tinHienTai.NguoiLienHe = tinTuyenDung.NguoiLienHe;
            tinHienTai.EmailLienHe = tinTuyenDung.EmailLienHe;
            tinHienTai.SDTLienHe = tinTuyenDung.SDTLienHe;
            tinHienTai.TuKhoa = tinTuyenDung.TuKhoa;
            tinHienTai.HanNop = tinTuyenDung.HanNop;
            tinHienTai.Latitude = tinTuyenDung.Latitude;
            tinHienTai.Longitude = tinTuyenDung.Longitude;

            return tinHienTai;
        }
    }
}

