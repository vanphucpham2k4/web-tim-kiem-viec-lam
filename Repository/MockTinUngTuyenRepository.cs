using Unicareer.Models;
using Unicareer.Models.Enums;
using System.Linq;

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
                    TrangThaiXuLy = TrangThaiXuLyHelper.ToString(TrangThaiXuLy.ChoPhongVan),
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
                    TrangThaiXuLy = TrangThaiXuLyHelper.ToString(TrangThaiXuLy.DangXemXet),
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
                    TrangThaiXuLy = TrangThaiXuLyHelper.ToString(TrangThaiXuLy.DaPhongVan),
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
                    TrangThaiXuLy = TrangThaiXuLyHelper.ToString(TrangThaiXuLy.DangXemXet),
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
                    TrangThaiXuLy = TrangThaiXuLyHelper.ToString(TrangThaiXuLy.ChoPhongVan),
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

        public TinUngTuyen ThemTinUngTuyen(TinUngTuyen tinUngTuyen)
        {
            // Tự động tạo ID mới
            var maxId = _danhSachTinUngTuyen.Any() 
                ? _danhSachTinUngTuyen.Max(t => t.MaTinUngTuyen) 
                : 0;
            tinUngTuyen.MaTinUngTuyen = maxId + 1;
            
            // Set ngày ứng tuyển nếu chưa có
            if (tinUngTuyen.NgayUngTuyen == default(DateTime))
            {
                tinUngTuyen.NgayUngTuyen = DateTime.Now;
            }
            
            // Set trạng thái mặc định nếu chưa có
            if (string.IsNullOrEmpty(tinUngTuyen.TrangThaiXuLy))
            {
                tinUngTuyen.TrangThaiXuLy = TrangThaiXuLyHelper.ToString(TrangThaiXuLy.DangXemXet);
            }
            
            _danhSachTinUngTuyen.Add(tinUngTuyen);
            return tinUngTuyen;
        }
    }
}

