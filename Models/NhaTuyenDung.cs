namespace Unicareer.Models
{
    public class NhaTuyenDung
    {
        public int MaNhaTuyenDung { get; set; }
        public string TenCongTy { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string SoDienThoai { get; set; } = string.Empty;
        public string DiaChi { get; set; } = string.Empty;
        public string TinhThanhPho { get; set; } = string.Empty;
        public string QuanHuyen { get; set; } = string.Empty;
        public string Website { get; set; } = string.Empty;
        public string NguoiDaiDien { get; set; } = string.Empty;
        public string ChucVu { get; set; } = string.Empty;
        public string LinhVuc { get; set; } = string.Empty;
        public string MoTa { get; set; } = string.Empty;
        public string Logo { get; set; } = string.Empty;
        public int SoTinDaDang { get; set; }
        public int SoUngVienNhan { get; set; }
        public DateTime NgayDangKy { get; set; }

        // Mockdata - Danh sach nha tuyen dung
        public static List<NhaTuyenDung> LayDanhSachNhaTuyenDung()
        {
            return new List<NhaTuyenDung>
            {
                new NhaTuyenDung
                {
                    MaNhaTuyenDung = 1,
                    TenCongTy = "FPT Software",
                    Email = "recruit@fpt.com.vn",
                    SoDienThoai = "024-37912345",
                    DiaChi = "Keangnam Landmark Tower, Ha Noi",
                    TinhThanhPho = "Ha Noi",
                    QuanHuyen = "Cau Giay",
                    Website = "https://fptsoftware.com",
                    NguoiDaiDien = "Nguyen Van An",
                    ChucVu = "HR Manager",
                    LinhVuc = "Cong nghe thong tin",
                    MoTa = @"<p>FPT Software là công ty thành viên của Tập đoàn FPT, được thành lập từ năm 1999. Sau hơn 20 năm phát triển, FPT Software đã trở thành công ty cung cấp dịch vụ phần mềm hàng đầu tại Việt Nam và khu vực.</p>
                    <p>Với hơn 30,000 nhân viên tại 83 văn phòng trên toàn cầu, FPT Software cung cấp các dịch vụ chuyển đổi số, tư vấn công nghệ, phát triển phần mềm và các giải pháp công nghệ tiên tiến cho các tập đoàn lớn trên thế giới.</p>
                    <h5>Lĩnh vực hoạt động:</h5>
                    <ul>
                        <li>Phát triển phần mềm cho doanh nghiệp</li>
                        <li>Chuyển đổi số (Digital Transformation)</li>
                        <li>Cloud Computing & Big Data</li>
                        <li>AI & Machine Learning</li>
                        <li>IoT Solutions</li>
                    </ul>",
                    Logo = "https://inkythuatso.com/uploads/images/2021/11/logo-fpt-inkythuatso-01-10-09-16-42.jpg",
                    SoTinDaDang = 45,
                    SoUngVienNhan = 380,
                    NgayDangKy = DateTime.Now.AddMonths(-18)
                },
                new NhaTuyenDung
                {
                    MaNhaTuyenDung = 2,
                    TenCongTy = "Vinamilk",
                    Email = "hr@vinamilk.com.vn",
                    SoDienThoai = "028-54155555",
                    DiaChi = "10 Tan Trao, Tan Phu, TP HCM",
                    TinhThanhPho = "TP Ho Chi Minh",
                    QuanHuyen = "Tan Phu",
                    Website = "https://vinamilk.com.vn",
                    NguoiDaiDien = "Tran Thi Binh",
                    ChucVu = "Truong phong Tuyen dung",
                    LinhVuc = "San xuat thuc pham",
                    MoTa = "Cong ty sua hang dau Viet Nam va khu vuc",
                    SoTinDaDang = 28,
                    SoUngVienNhan = 245,
                    NgayDangKy = DateTime.Now.AddMonths(-24)
                },
                new NhaTuyenDung
                {
                    MaNhaTuyenDung = 3,
                    TenCongTy = "Tiki",
                    Email = "career@tiki.vn",
                    SoDienThoai = "028-62728728",
                    DiaChi = "52 Ut Tich, Tan Binh, TP HCM",
                    TinhThanhPho = "TP Ho Chi Minh",
                    QuanHuyen = "Tan Binh",
                    Website = "https://tiki.vn",
                    NguoiDaiDien = "Le Van Cuong",
                    ChucVu = "Talent Acquisition Lead",
                    LinhVuc = "Thuong mai dien tu",
                    MoTa = @"<p>Tiki là sàn thương mại điện tử hàng đầu Việt Nam, được thành lập năm 2010. Tiki cam kết mang đến trải nghiệm mua sắm tốt nhất với hàng triệu sản phẩm chính hãng, giao hàng nhanh chóng và dịch vụ khách hàng xuất sắc.</p>
                    <p>Với phương châm 'Tiki - Tích điểm đổi quà', Tiki không ngừng cải tiến và phát triển để trở thành điểm đến tin cậy của người tiêu dùng Việt Nam.</p>
                    <h5>Dịch vụ nổi bật:</h5>
                    <ul>
                        <li>TikiNOW - Giao hàng nhanh 2h</li>
                        <li>Tiki Trading - Hệ thống kho hàng hiện đại</li>
                        <li>Tiki Insurance - Bảo hiểm trực tuyến</li>
                        <li>Tiki Smart Logistics</li>
                        <li>Hàng triệu sản phẩm chính hãng</li>
                    </ul>",
                    Logo = "",
                    SoTinDaDang = 52,
                    SoUngVienNhan = 428,
                    NgayDangKy = DateTime.Now.AddMonths(-15)
                },
                new NhaTuyenDung
                {
                    MaNhaTuyenDung = 4,
                    TenCongTy = "VNG Corporation",
                    Email = "jobs@vng.com.vn",
                    SoDienThoai = "028-54587262",
                    DiaChi = "Z06 Street, Tan Thuan EPZ, Q7, TP HCM",
                    TinhThanhPho = "TP Ho Chi Minh",
                    QuanHuyen = "Quan 7",
                    Website = "https://vng.com.vn",
                    NguoiDaiDien = "Pham Thi Dung",
                    ChucVu = "Senior HR",
                    LinhVuc = "Cong nghe - Game - Truyen thong",
                    MoTa = @"<p>VNG Corporation là tập đoàn công nghệ hàng đầu Việt Nam, được thành lập năm 2004. VNG đã tạo ra nhiều sản phẩm công nghệ phổ biến như Zalo, Zing MP3, ZaloPay và các game online nổi tiếng.</p>
                    <p>Với sứ mệnh 'Make the Internet Change Vietnamese Lives', VNG không ngừng đổi mới và phát triển các sản phẩm công nghệ phục vụ hàng triệu người dùng Việt Nam.</p>
                    <h5>Sản phẩm chính:</h5>
                    <ul>
                        <li>Zalo - Ứng dụng nhắn tin hàng đầu Việt Nam</li>
                        <li>ZaloPay - Ví điện tử</li>
                        <li>Zing MP3 - Nền tảng nghe nhạc trực tuyến</li>
                        <li>Game Online - Đột Kích, Võ Lâm Truyền Kỳ</li>
                        <li>Cloud & AI Solutions</li>
                    </ul>",
                    Logo = "",
                    SoTinDaDang = 38,
                    SoUngVienNhan = 312,
                    NgayDangKy = DateTime.Now.AddMonths(-20)
                },
                new NhaTuyenDung
                {
                    MaNhaTuyenDung = 5,
                    TenCongTy = "Lazada Vietnam",
                    Email = "recruitment.vn@lazada.com",
                    SoDienThoai = "028-39107788",
                    DiaChi = "Empress Tower, 138-142 Hai Ba Trung, Q1, TP HCM",
                    TinhThanhPho = "TP Ho Chi Minh",
                    QuanHuyen = "Quan 1",
                    Website = "https://lazada.vn",
                    NguoiDaiDien = "Hoang Van Dung",
                    ChucVu = "Recruitment Manager",
                    LinhVuc = "Thuong mai dien tu",
                    MoTa = "Nen tang mua sam truc tuyen hang dau DNA",
                    SoTinDaDang = 34,
                    SoUngVienNhan = 278,
                    NgayDangKy = DateTime.Now.AddMonths(-16)
                },
                new NhaTuyenDung
                {
                    MaNhaTuyenDung = 6,
                    TenCongTy = "Viettel Group",
                    Email = "tuyendung@viettel.com.vn",
                    SoDienThoai = "024-39779898",
                    DiaChi = "Toa nha Viettel, Giang Vo, Ba Dinh, Ha Noi",
                    TinhThanhPho = "Ha Noi",
                    QuanHuyen = "Ba Dinh",
                    Website = "https://vietteltelecom.vn",
                    NguoiDaiDien = "Vo Thi Huong",
                    ChucVu = "Giam doc Nhan su",
                    LinhVuc = "Vien thong - Cong nghe",
                    MoTa = "Tap doan vien thong hang dau Viet Nam",
                    SoTinDaDang = 41,
                    SoUngVienNhan = 356,
                    NgayDangKy = DateTime.Now.AddMonths(-30)
                },
                new NhaTuyenDung
                {
                    MaNhaTuyenDung = 7,
                    TenCongTy = "VnExpress",
                    Email = "hr@vnexpress.net",
                    SoDienThoai = "024-37576666",
                    DiaChi = "123 Nguyen Thi Minh Khai, Q3, TP HCM",
                    TinhThanhPho = "TP Ho Chi Minh",
                    QuanHuyen = "Quan 3",
                    Website = "https://vnexpress.net",
                    NguoiDaiDien = "Dang Van Giau",
                    ChucVu = "HR Specialist",
                    LinhVuc = "Truyen thong - Bao chi",
                    MoTa = "Bao dien tu so 1 Viet Nam",
                    SoTinDaDang = 15,
                    SoUngVienNhan = 142,
                    NgayDangKy = DateTime.Now.AddMonths(-22)
                },
                new NhaTuyenDung
                {
                    MaNhaTuyenDung = 8,
                    TenCongTy = "Momo",
                    Email = "career@momo.vn",
                    SoDienThoai = "028-71089999",
                    DiaChi = "Lim Tower, 9-11 Ton Dan, Q4, TP HCM",
                    TinhThanhPho = "TP Ho Chi Minh",
                    QuanHuyen = "Quan 4",
                    Website = "https://momo.vn",
                    NguoiDaiDien = "Bui Thi Hoa",
                    ChucVu = "Head of Talent Acquisition",
                    LinhVuc = "Fintech - Thanh toan dien tu",
                    MoTa = @"<p>MoMo là ứng dụng ví điện tử và thanh toán di động hàng đầu Việt Nam, được thành lập năm 2007. Với sứ mệnh 'Thanh toán điện tử cho mọi người', MoMo đã kết nối hàng triệu người dùng với các dịch vụ tài chính tiện lợi.</p>
                    <p>MoMo không chỉ là ví điện tử mà còn là siêu ứng dụng tài chính, cung cấp đa dạng dịch vụ từ thanh toán, chuyển tiền đến đầu tư và bảo hiểm.</p>
                    <h5>Dịch vụ chính:</h5>
                    <ul>
                        <li>Thanh toán QR Code tại hàng triệu điểm</li>
                        <li>Chuyển tiền miễn phí</li>
                        <li>Đầu tư - Tiết kiệm - Bảo hiểm</li>
                        <li>Mua sắm trực tuyến</li>
                        <li>Thanh toán hóa đơn, nạp thẻ</li>
                    </ul>",
                    Logo = "",
                    SoTinDaDang = 47,
                    SoUngVienNhan = 391,
                    NgayDangKy = DateTime.Now.AddMonths(-14)
                },
                new NhaTuyenDung
                {
                    MaNhaTuyenDung = 9,
                    TenCongTy = "Samsung Vietnam",
                    Email = "recruit.vn@samsung.com",
                    SoDienThoai = "024-39366666",
                    DiaChi = "Keangnam Landmark Tower, Ha Noi",
                    TinhThanhPho = "Ha Noi",
                    QuanHuyen = "Cau Giay",
                    Website = "https://samsung.com/vn",
                    NguoiDaiDien = "Ngo Van Khanh",
                    ChucVu = "HR Director",
                    LinhVuc = "Dien tu - Cong nghe",
                    MoTa = "Tap doan dien tu hang dau the gioi",
                    SoTinDaDang = 36,
                    SoUngVienNhan = 298,
                    NgayDangKy = DateTime.Now.AddMonths(-28)
                },
                new NhaTuyenDung
                {
                    MaNhaTuyenDung = 10,
                    TenCongTy = "Shopee Vietnam",
                    Email = "careers.vn@shopee.com",
                    SoDienThoai = "028-73088888",
                    DiaChi = "Pearl Plaza, 561A Dien Bien Phu, Binh Thanh, TP HCM",
                    TinhThanhPho = "TP Ho Chi Minh",
                    QuanHuyen = "Binh Thanh",
                    Website = "https://careers.shopee.vn",
                    NguoiDaiDien = "Duong Thi Lan",
                    ChucVu = "Senior Recruiter",
                    LinhVuc = "Thuong mai dien tu",
                    MoTa = @"<p>Shopee là nền tảng thương mại điện tử hàng đầu khu vực Đông Nam Á và Đài Loan, trực thuộc Sea Group (NYSE: SE). Shopee được thành lập tại Singapore năm 2015 và chính thức ra mắt tại Việt Nam vào năm 2016.</p>
                    <p>Với hệ thống logistics mạnh mẽ và nền tảng thanh toán an toàn, Shopee mang đến trải nghiệm mua sắm trực tuyến thuận tiện, an toàn và thú vị cho người tiêu dùng.</p>
                    <h5>Điểm nổi bật:</h5>
                    <ul>
                        <li>Miễn phí vận chuyển toàn quốc</li>
                        <li>Shopee Mall - Sản phẩm chính hãng 100%</li>
                        <li>Shopee Food - Giao đồ ăn</li>
                        <li>ShopeePay - Ví điện tử</li>
                        <li>Chương trình khuyến mãi hấp dẫn</li>
                    </ul>",
                    Logo = "",
                    SoTinDaDang = 58,
                    SoUngVienNhan = 485,
                    NgayDangKy = DateTime.Now.AddMonths(-12)
                },
                new NhaTuyenDung
                {
                    MaNhaTuyenDung = 11,
                    TenCongTy = "Highlands Coffee",
                    Email = "recruitment@highlandscoffee.com.vn",
                    SoDienThoai = "028-39118888",
                    DiaChi = "24 Nguyen Hue, Q1, TP HCM",
                    TinhThanhPho = "TP Ho Chi Minh",
                    QuanHuyen = "Quan 1",
                    Website = "https://highlandscoffee.com.vn",
                    NguoiDaiDien = "Ly Van Long",
                    ChucVu = "HR Manager",
                    LinhVuc = "F&B - Nha hang - Ca phe",
                    MoTa = "Chuoi ca phe hang dau Viet Nam",
                    SoTinDaDang = 22,
                    SoUngVienNhan = 186,
                    NgayDangKy = DateTime.Now.AddMonths(-26)
                },
                new NhaTuyenDung
                {
                    MaNhaTuyenDung = 12,
                    TenCongTy = "Deloitte Vietnam",
                    Email = "hrvietnam@deloitte.com",
                    SoDienThoai = "028-39101100",
                    DiaChi = "Saigon Centre Tower, 65 Le Loi, Q1, TP HCM",
                    TinhThanhPho = "TP Ho Chi Minh",
                    QuanHuyen = "Quan 1",
                    Website = "https://deloitte.com/vn",
                    NguoiDaiDien = "Trinh Thi Mai",
                    ChucVu = "Talent Manager",
                    LinhVuc = "Tu van - Kiem toan",
                    MoTa = "Cong ty tu van va kiem toan quoc te hang dau",
                    SoTinDaDang = 19,
                    SoUngVienNhan = 157,
                    NgayDangKy = DateTime.Now.AddMonths(-32)
                }
            };
        }

        public static NhaTuyenDung? LayNhaTuyenDungTheoId(int id)
        {
            var danhSach = LayDanhSachNhaTuyenDung();
            return danhSach.FirstOrDefault(n => n.MaNhaTuyenDung == id);
        }

        public static NhaTuyenDung? LayNhaTuyenDungTheoTen(string tenCongTy)
        {
            var danhSach = LayDanhSachNhaTuyenDung();
            return danhSach.FirstOrDefault(n => n.TenCongTy.Equals(tenCongTy, StringComparison.OrdinalIgnoreCase));
        }
    }
}

