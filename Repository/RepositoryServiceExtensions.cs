using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Unicareer.Repository
{
    /// <summary>
    /// Extension methods để đăng ký repository với khả năng chuyển đổi giữa mock data và dữ liệu thật
    /// </summary>
    public static class RepositoryServiceExtensions
    {
        /// <summary>
        /// Đăng ký tất cả các repository dựa trên cấu hình UseMockData
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <param name="configuration">Configuration để đọc cấu hình UseMockData</param>
        public static void AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            var useMockData = configuration.GetValue<bool>("Repository:UseMockData", false);

            if (useMockData)
            {
                // Đăng ký Mock Repositories (không cần ApplicationDbContext)
                services.AddScoped<INhaTuyenDungRepository, MockNhaTuyenDungRepository>();
                services.AddScoped<IUngVienRepository, MockUngVienRepository>();
                services.AddScoped<ITinTuyenDungRepository, MockTinTuyenDungRepository>();
                services.AddScoped<ITinUngTuyenRepository, MockTinUngTuyenRepository>();
                services.AddScoped<IViecLamDaLuuRepository, MockViecLamDaLuuRepository>();
                services.AddScoped<ILoaiCongViecRepository, MockLoaiCongViecRepository>();
                services.AddScoped<INganhNgheRepository, MockNganhNgheRepository>();
                services.AddScoped<IChuyenNganhRepository, MockChuyenNganhRepository>();
                services.AddScoped<ITruongDaiHocRepository, MockTruongDaiHocRepository>();
                // Note: BlogRepository không có mock version
            }
            else
            {
                // Đăng ký Real Repositories (cần ApplicationDbContext)
                services.AddScoped<INhaTuyenDungRepository, NhaTuyenDungRepository>();
                services.AddScoped<IUngVienRepository, UngVienRepository>();
                services.AddScoped<ITinTuyenDungRepository, TinTuyenDungRepository>();
                services.AddScoped<ITinUngTuyenRepository, TinUngTuyenRepository>();
                services.AddScoped<IViecLamDaLuuRepository, ViecLamDaLuuRepository>();
                services.AddScoped<ILoaiCongViecRepository, LoaiCongViecRepository>();
                services.AddScoped<INganhNgheRepository, NganhNgheRepository>();
                services.AddScoped<IChuyenNganhRepository, ChuyenNganhRepository>();
                services.AddScoped<ITruongDaiHocRepository, TruongDaiHocRepository>();
                services.AddScoped<IBlogRepository, BlogRepository>();
                services.AddScoped<ITheLoaiBlogRepository, TheLoaiBlogRepository>();
            }
        }
    }
}

