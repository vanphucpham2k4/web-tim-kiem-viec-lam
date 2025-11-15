using Microsoft.EntityFrameworkCore;
using Unicareer.Data;

namespace Unicareer.Services
{
    /// <summary>
    /// Background service để tự động cập nhật trạng thái "Het han" cho các tin tuyển dụng đã quá hạn
    /// </summary>
    public class UpdateTrangThaiBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<UpdateTrangThaiBackgroundService> _logger;
        private readonly TimeSpan _checkInterval = TimeSpan.FromHours(1); // Kiểm tra mỗi giờ

        public UpdateTrangThaiBackgroundService(
            IServiceProvider serviceProvider,
            ILogger<UpdateTrangThaiBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("UpdateTrangThaiBackgroundService đã khởi động. Sẽ kiểm tra trạng thái tin tuyển dụng mỗi {Interval} giờ.", _checkInterval.TotalHours);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await UpdateExpiredJobPosts(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Có lỗi xảy ra khi cập nhật trạng thái tin tuyển dụng");
                }

                // Đợi một khoảng thời gian trước khi kiểm tra lại
                await Task.Delay(_checkInterval, stoppingToken);
            }
        }

        private async Task UpdateExpiredJobPosts(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            try
            {
                var ngayHienTai = DateTime.Now.Date;
                
                // Tìm tất cả tin tuyển dụng đã quá hạn nhưng chưa được đánh dấu là "Het han" hoặc "Da dong"
                var tinDaHetHan = await context.TinTuyenDungs
                    .Where(t => t.HanNop < ngayHienTai 
                        && t.TrangThai != "Het han" 
                        && t.TrangThai != "Da dong"
                        && (t.TrangThai == null || t.TrangThai == "Dang tuyen"))
                    .ToListAsync(cancellationToken);

                if (tinDaHetHan.Any())
                {
                    foreach (var tin in tinDaHetHan)
                    {
                        tin.TrangThai = "Het han";
                    }

                    await context.SaveChangesAsync(cancellationToken);
                    _logger.LogInformation("Đã tự động cập nhật {Count} tin tuyển dụng thành trạng thái 'Het han'", tinDaHetHan.Count);
                }
                else
                {
                    _logger.LogDebug("Không có tin tuyển dụng nào cần cập nhật trạng thái");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật trạng thái tin tuyển dụng hết hạn");
                throw;
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("UpdateTrangThaiBackgroundService đang dừng...");
            await base.StopAsync(cancellationToken);
        }
    }
}

