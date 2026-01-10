using AutoMapper;
using Unicareer.Models;

namespace Unicareer.Mappings
{
    /// <summary>
    /// AutoMapper Profile cho NhaTuyenDung entity
    /// Xử lý mapping giữa các đối tượng NhaTuyenDung khi cập nhật
    /// </summary>
    public class NhaTuyenDungMappingProfile : Profile
    {
        public NhaTuyenDungMappingProfile()
        {
            // Mapping từ NhaTuyenDung nguồn (từ form/tham số) sang NhaTuyenDung đích (từ database)
            // Sử dụng cho việc cập nhật nhà tuyển dụng hiện có
            CreateMap<NhaTuyenDung, NhaTuyenDung>()
                .ForMember(dest => dest.MaNhaTuyenDung, opt => opt.Ignore()) // Giữ nguyên MaNhaTuyenDung
                .ForMember(dest => dest.UserId, opt => opt.Ignore()) // Giữ nguyên UserId
                .ForMember(dest => dest.User, opt => opt.Ignore()) // Giữ nguyên navigation property
                .ForMember(dest => dest.NgayDangKy, opt => opt.Ignore()) // Giữ nguyên NgayDangKy
                .ForMember(dest => dest.SoTinDaDang, opt => opt.Ignore()) // Giữ nguyên SoTinDaDang
                .ForMember(dest => dest.SoUngVienNhan, opt => opt.Ignore()) // Giữ nguyên SoUngVienNhan
                .ForMember(dest => dest.Logo, opt => opt.Ignore()); // Logo được xử lý riêng (xóa hoặc cập nhật)
        }
    }
}

