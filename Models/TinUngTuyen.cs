namespace Unicareer.Models
{
    public class TinUngTuyen
    {
        public int MaTinUngTuyen { get; set; }
        public string HoTen { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string SoDienThoai { get; set; } = string.Empty;
        public string ViTriUngTuyen { get; set; } = string.Empty;
        public string CongTy { get; set; } = string.Empty;
        public string MaTinTuyenDung { get; set; } = string.Empty;
        public string TrangThaiXuLy { get; set; } = string.Empty;
        public string LinkCV { get; set; } = string.Empty;
        public string GhiChu { get; set; } = string.Empty;
        public DateTime NgayUngTuyen { get; set; }

        // Mockdata - Danh sach tin ung tuyen
        public static List<TinUngTuyen> LayDanhSachTinUngTuyen()
        {
            return new List<TinUngTuyen>
            {
                new TinUngTuyen
                {
                    MaTinUngTuyen = 1,
                    HoTen = "Nguyen Van A",
                    Email = "nguyenvana@gmail.com",
                    SoDienThoai = "0912345678",
                    ViTriUngTuyen = "Senior Full-stack Developer",
                    CongTy = "FPT Software",
                    MaTinTuyenDung = "1",
                    TrangThaiXuLy = "Cho phong van",
                    LinkCV = "/uploads/cv_nguyenvana.pdf",
                    GhiChu = "Ung vien co 5 nam kinh nghiem",
                    NgayUngTuyen = DateTime.Now.AddDays(-2)
                },
                new TinUngTuyen
                {
                    MaTinUngTuyen = 2,
                    HoTen = "Tran Thi B",
                    Email = "tranthib@gmail.com",
                    SoDienThoai = "0987654321",
                    ViTriUngTuyen = "Marketing Manager",
                    CongTy = "Vinamilk",
                    MaTinTuyenDung = "2",
                    TrangThaiXuLy = "Dang xem xet",
                    LinkCV = "/uploads/cv_tranthib.pdf",
                    GhiChu = "Co kinh nghiem quan ly doi nhom 10 nguoi",
                    NgayUngTuyen = DateTime.Now.AddDays(-1)
                },
                new TinUngTuyen
                {
                    MaTinUngTuyen = 3,
                    HoTen = "Le Van C",
                    Email = "levanc@gmail.com",
                    SoDienThoai = "0901234567",
                    ViTriUngTuyen = "Frontend Developer (ReactJS)",
                    CongTy = "Tiki",
                    MaTinTuyenDung = "4",
                    TrangThaiXuLy = "Da phong van",
                    LinkCV = "/uploads/cv_levanc.pdf",
                    GhiChu = "Ket qua phong van tot",
                    NgayUngTuyen = DateTime.Now.AddDays(-5)
                },
                new TinUngTuyen
                {
                    MaTinUngTuyen = 4,
                    HoTen = "Pham Thi D",
                    Email = "phamthid@gmail.com",
                    SoDienThoai = "0923456789",
                    ViTriUngTuyen = "UI/UX Designer",
                    CongTy = "VNG Corporation",
                    MaTinTuyenDung = "5",
                    TrangThaiXuLy = "Dang xem xet",
                    LinkCV = "/uploads/cv_phamthid.pdf",
                    GhiChu = "Portfolio rat an tuong",
                    NgayUngTuyen = DateTime.Now.AddDays(-1)
                },
                new TinUngTuyen
                {
                    MaTinUngTuyen = 5,
                    HoTen = "Hoang Van E",
                    Email = "hoangvane@gmail.com",
                    SoDienThoai = "0934567890",
                    ViTriUngTuyen = "Data Analyst",
                    CongTy = "Lazada Vietnam",
                    MaTinTuyenDung = "6",
                    TrangThaiXuLy = "Cho phong van",
                    LinkCV = "/uploads/cv_hoangvane.pdf",
                    GhiChu = "Tot nghiep loai gioi dai hoc Kinh te",
                    NgayUngTuyen = DateTime.Now.AddDays(-3)
                },
                new TinUngTuyen
                {
                    MaTinUngTuyen = 6,
                    HoTen = "Vo Thi F",
                    Email = "vothif@gmail.com",
                    SoDienThoai = "0945678901",
                    ViTriUngTuyen = "HR Manager",
                    CongTy = "Viettel Group",
                    MaTinTuyenDung = "7",
                    TrangThaiXuLy = "Tu choi",
                    LinkCV = "/uploads/cv_vothif.pdf",
                    GhiChu = "Kinh nghiem chua du",
                    NgayUngTuyen = DateTime.Now.AddDays(-6)
                },
                new TinUngTuyen
                {
                    MaTinUngTuyen = 7,
                    HoTen = "Dang Van G",
                    Email = "dangvang@gmail.com",
                    SoDienThoai = "0956789012",
                    ViTriUngTuyen = "Content Writer",
                    CongTy = "VnExpress",
                    MaTinTuyenDung = "8",
                    TrangThaiXuLy = "Dang xem xet",
                    LinkCV = "/uploads/cv_dangvang.pdf",
                    GhiChu = "Co nhieu bai viet duoc xuat ban",
                    NgayUngTuyen = DateTime.Now.AddHours(-12)
                },
                new TinUngTuyen
                {
                    MaTinUngTuyen = 8,
                    HoTen = "Bui Thi H",
                    Email = "buithih@gmail.com",
                    SoDienThoai = "0967890123",
                    ViTriUngTuyen = "Business Analyst",
                    CongTy = "Momo",
                    MaTinTuyenDung = "9",
                    TrangThaiXuLy = "Cho phong van",
                    LinkCV = "/uploads/cv_buithih.pdf",
                    GhiChu = "Co chung chi BA quoc te",
                    NgayUngTuyen = DateTime.Now.AddDays(-2)
                },
                new TinUngTuyen
                {
                    MaTinUngTuyen = 9,
                    HoTen = "Ngo Van I",
                    Email = "ngovani@gmail.com",
                    SoDienThoai = "0978901234",
                    ViTriUngTuyen = "Backend Developer (Java)",
                    CongTy = "Samsung Vietnam",
                    MaTinTuyenDung = "10",
                    TrangThaiXuLy = "Da phong van",
                    LinkCV = "/uploads/cv_ngovani.pdf",
                    GhiChu = "Ung vien tiem nang, de xuat tuyen dung",
                    NgayUngTuyen = DateTime.Now.AddDays(-7)
                },
                new TinUngTuyen
                {
                    MaTinUngTuyen = 10,
                    HoTen = "Duong Thi K",
                    Email = "duongthik@gmail.com",
                    SoDienThoai = "0989012345",
                    ViTriUngTuyen = "Intern - Software Developer",
                    CongTy = "Shopee Vietnam",
                    MaTinTuyenDung = "11",
                    TrangThaiXuLy = "Dang xem xet",
                    LinkCV = "/uploads/cv_duongthik.pdf",
                    GhiChu = "Sinh vien nam cuoi DH Bach Khoa",
                    NgayUngTuyen = DateTime.Now.AddHours(-6)
                },
                new TinUngTuyen
                {
                    MaTinUngTuyen = 11,
                    HoTen = "Ly Van L",
                    Email = "lyvanl@gmail.com",
                    SoDienThoai = "0990123456",
                    ViTriUngTuyen = "Sales Manager",
                    CongTy = "Highlands Coffee",
                    MaTinTuyenDung = "12",
                    TrangThaiXuLy = "Cho phong van",
                    LinkCV = "/uploads/cv_lyvanl.pdf",
                    GhiChu = "7 nam kinh nghiem trong linh vuc F&B",
                    NgayUngTuyen = DateTime.Now.AddDays(-3)
                },
                new TinUngTuyen
                {
                    MaTinUngTuyen = 12,
                    HoTen = "Trinh Thi M",
                    Email = "trinhthim@gmail.com",
                    SoDienThoai = "0901234560",
                    ViTriUngTuyen = "Accountant",
                    CongTy = "Deloitte Vietnam",
                    MaTinTuyenDung = "3",
                    TrangThaiXuLy = "Dang xem xet",
                    LinkCV = "/uploads/cv_trinhthim.pdf",
                    GhiChu = "Co chung chi CPA",
                    NgayUngTuyen = DateTime.Now.AddDays(-4)
                },
                new TinUngTuyen
                {
                    MaTinUngTuyen = 13,
                    HoTen = "Ha Van N",
                    Email = "havann@gmail.com",
                    SoDienThoai = "0912340987",
                    ViTriUngTuyen = "Senior Full-stack Developer",
                    CongTy = "FPT Software",
                    MaTinTuyenDung = "1",
                    TrangThaiXuLy = "Tu choi",
                    LinkCV = "/uploads/cv_havann.pdf",
                    GhiChu = "Khong phu hop voi van hoa cong ty",
                    NgayUngTuyen = DateTime.Now.AddDays(-8)
                },
                new TinUngTuyen
                {
                    MaTinUngTuyen = 14,
                    HoTen = "Quach Thi O",
                    Email = "quachthio@gmail.com",
                    SoDienThoai = "0923451234",
                    ViTriUngTuyen = "Frontend Developer (ReactJS)",
                    CongTy = "Tiki",
                    MaTinTuyenDung = "4",
                    TrangThaiXuLy = "Tuyen dung",
                    LinkCV = "/uploads/cv_quachthio.pdf",
                    GhiChu = "Da gui offer letter",
                    NgayUngTuyen = DateTime.Now.AddDays(-10)
                },
                new TinUngTuyen
                {
                    MaTinUngTuyen = 15,
                    HoTen = "Mai Van P",
                    Email = "maivanp@gmail.com",
                    SoDienThoai = "0934562345",
                    ViTriUngTuyen = "UI/UX Designer",
                    CongTy = "VNG Corporation",
                    MaTinTuyenDung = "5",
                    TrangThaiXuLy = "Cho phong van",
                    LinkCV = "/uploads/cv_maivanp.pdf",
                    GhiChu = "Lich phong van: 15/11/2024",
                    NgayUngTuyen = DateTime.Now.AddDays(-1)
                }
            };
        }
    }
}

