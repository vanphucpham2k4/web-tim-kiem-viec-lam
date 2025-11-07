using Unicareer.Models;

namespace Unicareer.Repository
{
    public interface ILoaiCongViecRepository
    {
        List<LoaiCongViec> LayDanhSachLoaiCongViec();
        LoaiCongViec? LayLoaiCongViecTheoId(int id);
    }
}

