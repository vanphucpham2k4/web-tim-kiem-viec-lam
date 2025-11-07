using Unicareer.Models;

namespace Unicareer.Repository
{
    public class MockTinUngTuyenRepository : ITinUngTuyenRepository
    {
        private readonly List<TinUngTuyen> _danhSachTinUngTuyen;

        public MockTinUngTuyenRepository()
        {
            _danhSachTinUngTuyen = new List<TinUngTuyen>
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
                }
            };
        }

        public List<TinUngTuyen> LayDanhSachTinUngTuyen()
        {
            return _danhSachTinUngTuyen;
        }

        public TinUngTuyen? LayTinUngTuyenTheoId(int id)
        {
            return _danhSachTinUngTuyen.FirstOrDefault(t => t.MaTinUngTuyen == id);
        }
    }
}

