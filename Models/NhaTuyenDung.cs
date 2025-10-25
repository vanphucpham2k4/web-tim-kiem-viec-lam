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
        public string QuyMo { get; set; } = string.Empty;
        public string MoTa { get; set; } = string.Empty;
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
                    QuyMo = "10000+ nhan vien",
                    MoTa = "Cong ty hang dau ve giai phap cong nghe va dich vu IT tai Viet Nam",
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
                    QuyMo = "5000-10000 nhan vien",
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
                    QuyMo = "1000-5000 nhan vien",
                    MoTa = "San thuong mai dien tu hang dau Viet Nam",
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
                    QuyMo = "1000-5000 nhan vien",
                    MoTa = "Tap doan cong nghe hang dau Viet Nam voi cac san pham Zalo, Zing",
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
                    QuyMo = "1000-5000 nhan vien",
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
                    QuyMo = "10000+ nhan vien",
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
                    QuyMo = "500-1000 nhan vien",
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
                    QuyMo = "1000-5000 nhan vien",
                    MoTa = "Ung dung vi dien tu hang dau Viet Nam",
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
                    QuyMo = "10000+ nhan vien",
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
                    QuyMo = "1000-5000 nhan vien",
                    MoTa = "Nen tang mua sam truc tuyen hang dau DNA",
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
                    QuyMo = "1000-5000 nhan vien",
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
                    QuyMo = "500-1000 nhan vien",
                    MoTa = "Cong ty tu van va kiem toan quoc te hang dau",
                    SoTinDaDang = 19,
                    SoUngVienNhan = 157,
                    NgayDangKy = DateTime.Now.AddMonths(-32)
                }
            };
        }
    }
}

