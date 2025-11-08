using Unicareer.Models;

namespace Unicareer.Repository
{
    public class MockTruongDaiHocRepository : ITruongDaiHocRepository
    {
        private readonly List<TruongDaiHoc> _danhSachTruongDaiHoc;

        public MockTruongDaiHocRepository()
        {
            _danhSachTruongDaiHoc = new List<TruongDaiHoc>
            {
                new TruongDaiHoc
                {
                    MaTruong = 1,
                    TenTruong = "Đại học Bách khoa Hà Nội",
                    MoTa = "Trường đại học kỹ thuật hàng đầu Việt Nam, đào tạo các ngành kỹ thuật, công nghệ",
                    NgayTao = DateTime.Now.AddMonths(-24)
                },
                new TruongDaiHoc
                {
                    MaTruong = 2,
                    TenTruong = "Đại học Quốc gia Hà Nội",
                    MoTa = "Hệ thống đại học quốc gia lớn nhất miền Bắc, đào tạo đa ngành, đa lĩnh vực",
                    NgayTao = DateTime.Now.AddMonths(-30)
                },
                new TruongDaiHoc
                {
                    MaTruong = 3,
                    TenTruong = "Đại học Kinh tế Quốc dân",
                    MoTa = "Trường đại học hàng đầu về kinh tế, quản trị kinh doanh, tài chính ngân hàng",
                    NgayTao = DateTime.Now.AddMonths(-28)
                },
                new TruongDaiHoc
                {
                    MaTruong = 4,
                    TenTruong = "Đại học Ngoại thương",
                    MoTa = "Trường đại học chuyên đào tạo về thương mại quốc tế, kinh doanh quốc tế",
                    NgayTao = DateTime.Now.AddMonths(-26)
                },
                new TruongDaiHoc
                {
                    MaTruong = 5,
                    TenTruong = "Đại học Y Hà Nội",
                    MoTa = "Trường đại học y khoa hàng đầu Việt Nam, đào tạo bác sĩ, dược sĩ, điều dưỡng",
                    NgayTao = DateTime.Now.AddMonths(-32)
                },
                new TruongDaiHoc
                {
                    MaTruong = 6,
                    TenTruong = "Đại học Sư phạm Hà Nội",
                    MoTa = "Trường đại học đào tạo giáo viên, nghiên cứu khoa học giáo dục",
                    NgayTao = DateTime.Now.AddMonths(-22)
                },
                new TruongDaiHoc
                {
                    MaTruong = 7,
                    TenTruong = "Đại học Công nghệ - ĐHQG Hà Nội",
                    MoTa = "Trường đại học công nghệ thông tin, kỹ thuật điện tử, tự động hóa",
                    NgayTao = DateTime.Now.AddMonths(-20)
                },
                new TruongDaiHoc
                {
                    MaTruong = 8,
                    TenTruong = "Đại học Bách khoa TP.HCM",
                    MoTa = "Trường đại học kỹ thuật hàng đầu miền Nam, đào tạo kỹ sư các ngành",
                    NgayTao = DateTime.Now.AddMonths(-25)
                },
                new TruongDaiHoc
                {
                    MaTruong = 9,
                    TenTruong = "Đại học Kinh tế TP.HCM",
                    MoTa = "Trường đại học kinh tế lớn nhất miền Nam, đào tạo quản trị kinh doanh, tài chính",
                    NgayTao = DateTime.Now.AddMonths(-23)
                },
                new TruongDaiHoc
                {
                    MaTruong = 10,
                    TenTruong = "Đại học Quốc tế - ĐHQG TP.HCM",
                    MoTa = "Trường đại học quốc tế, đào tạo bằng tiếng Anh, liên kết với các trường nước ngoài",
                    NgayTao = DateTime.Now.AddMonths(-18)
                },
                new TruongDaiHoc
                {
                    MaTruong = 11,
                    TenTruong = "Đại học FPT",
                    MoTa = "Trường đại học công nghệ thông tin, đào tạo theo mô hình doanh nghiệp",
                    NgayTao = DateTime.Now.AddMonths(-15)
                },
                new TruongDaiHoc
                {
                    MaTruong = 12,
                    TenTruong = "Đại học RMIT Việt Nam",
                    MoTa = "Trường đại học quốc tế, đào tạo bằng tiếng Anh, chương trình quốc tế",
                    NgayTao = DateTime.Now.AddMonths(-12)
                }
            };
        }

        public List<TruongDaiHoc> LayDanhSachTruongDaiHoc()
        {
            return _danhSachTruongDaiHoc;
        }

        public TruongDaiHoc? LayTruongDaiHocTheoId(int id)
        {
            return _danhSachTruongDaiHoc.FirstOrDefault(t => t.MaTruong == id);
        }
    }
}

