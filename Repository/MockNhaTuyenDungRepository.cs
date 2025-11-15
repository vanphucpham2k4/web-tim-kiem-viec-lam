using Unicareer.Models;

namespace Unicareer.Repository
{
    public class MockNhaTuyenDungRepository : INhaTuyenDungRepository
    {
        private readonly List<NhaTuyenDung> _danhSachNhaTuyenDung;

        public MockNhaTuyenDungRepository()
        {
            _danhSachNhaTuyenDung = new List<NhaTuyenDung>
            {
                new NhaTuyenDung
                {
                    MaNhaTuyenDung = 1,
                    UserId = "mock-user-1", // Mock UserId
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
                    UserId = "mock-user-2", // Mock UserId
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
                    MoTa = @"<p>Vinamilk là công ty sữa hàng đầu Việt Nam và khu vực, được thành lập năm 1976. Với hơn 40 năm kinh nghiệm, Vinamilk đã trở thành thương hiệu sữa quen thuộc với hàng triệu gia đình Việt Nam.</p>
                    <p>Vinamilk không ngừng đầu tư vào công nghệ và nghiên cứu phát triển để mang đến những sản phẩm sữa chất lượng cao, đảm bảo dinh dưỡng và an toàn cho người tiêu dùng.</p>
                    <h5>Sản phẩm chính:</h5>
                    <ul>
                        <li>Sữa tươi tiệt trùng</li>
                        <li>Sữa bột dinh dưỡng</li>
                        <li>Sữa chua và các sản phẩm từ sữa</li>
                        <li>Nước giải khát</li>
                        <li>Thực phẩm bổ sung</li>
                    </ul>",
                    Logo = "",
                    SoTinDaDang = 28,
                    SoUngVienNhan = 245,
                    NgayDangKy = DateTime.Now.AddMonths(-24)
                },
                new NhaTuyenDung
                {
                    MaNhaTuyenDung = 3,
                    UserId = "mock-user-3", // Mock UserId
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
                    UserId = "mock-user-4", // Mock UserId
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
                    UserId = "mock-user-5", // Mock UserId
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
                    MoTa = @"<p>Lazada là nền tảng thương mại điện tử hàng đầu khu vực Đông Nam Á, trực thuộc Alibaba Group. Lazada Vietnam được thành lập năm 2012 và đã trở thành một trong những sàn thương mại điện tử lớn nhất tại Việt Nam.</p>
                    <p>Với hệ thống logistics mạnh mẽ và nền tảng thanh toán an toàn, Lazada mang đến trải nghiệm mua sắm trực tuyến thuận tiện, an toàn và thú vị cho người tiêu dùng.</p>
                    <h5>Điểm nổi bật:</h5>
                    <ul>
                        <li>Giao hàng nhanh chóng toàn quốc</li>
                        <li>LazMall - Sản phẩm chính hãng 100%</li>
                        <li>Lazada Express - Giao hàng trong ngày</li>
                        <li>Lazada Wallet - Ví điện tử tích hợp</li>
                        <li>Chương trình khuyến mãi hấp dẫn hàng ngày</li>
                    </ul>",
                    Logo = "",
                    SoTinDaDang = 34,
                    SoUngVienNhan = 278,
                    NgayDangKy = DateTime.Now.AddMonths(-16)
                }
            };
        }

        public List<NhaTuyenDung> LayDanhSachNhaTuyenDung()
        {
            return _danhSachNhaTuyenDung;
        }

        public NhaTuyenDung? LayNhaTuyenDungTheoId(int id)
        {
            return _danhSachNhaTuyenDung.FirstOrDefault(n => n.MaNhaTuyenDung == id);
        }

        public NhaTuyenDung? LayNhaTuyenDungTheoTen(string tenCongTy)
        {
            return _danhSachNhaTuyenDung.FirstOrDefault(n => n.TenCongTy.Equals(tenCongTy, StringComparison.OrdinalIgnoreCase));
        }

        public NhaTuyenDung? LayNhaTuyenDungTheoUserId(string userId)
        {
            return _danhSachNhaTuyenDung.FirstOrDefault(n => n.UserId == userId);
        }
    }
}

