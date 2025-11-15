using Unicareer.Models;

namespace Unicareer.Repository
{
    public interface ILoaiCongViecRepository
    {
        List<LoaiCongViec> LayDanhSachLoaiCongViec();
        LoaiCongViec? LayLoaiCongViecTheoId(int id);
        LoaiCongViec? ThemLoaiCongViec(LoaiCongViec loaiCongViec);
        LoaiCongViec? CapNhatLoaiCongViec(LoaiCongViec loaiCongViec);
        bool XoaLoaiCongViec(int id);
    }
}

