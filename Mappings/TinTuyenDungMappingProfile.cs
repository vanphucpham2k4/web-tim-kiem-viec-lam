using AutoMapper;
using Unicareer.Models;

namespace Unicareer.Mappings
{
    /// <summary>
    /// AutoMapper Profile cho TinTuyenDung entity
    /// Xử lý mapping giữa các đối tượng TinTuyenDung khi cập nhật
    /// </summary>
    public class TinTuyenDungMappingProfile : Profile
    {
        public TinTuyenDungMappingProfile()
        {
            // Mapping từ TinTuyenDung nguồn (từ form) sang TinTuyenDung đích (từ database)
            // Sử dụng cho việc cập nhật tin tuyển dụng hiện có
            CreateMap<TinTuyenDung, TinTuyenDung>()
                .ForMember(dest => dest.MaTinTuyenDung, opt => opt.Ignore()) // Giữ nguyên MaTinTuyenDung
                .ForMember(dest => dest.MaNhaTuyenDung, opt => opt.Ignore()) // Giữ nguyên MaNhaTuyenDung
                .ForMember(dest => dest.NhaTuyenDung, opt => opt.Ignore()) // Giữ nguyên navigation property
                .ForMember(dest => dest.NgayDang, opt => opt.Ignore()) // Giữ nguyên NgayDang
                .ForMember(dest => dest.SoLuongUngTuyen, opt => opt.Ignore()) // Giữ nguyên SoLuongUngTuyen
                .ForMember(dest => dest.TrangThai, opt => opt.Ignore()) // Giữ nguyên TrangThai
                .ForMember(dest => dest.TrangThaiDuyet, opt => opt.Ignore()) // Giữ nguyên TrangThaiDuyet
                .ForMember(dest => dest.LyDoTuChoi, opt => opt.Ignore()) // Giữ nguyên LyDoTuChoi
                .ForMember(dest => dest.NgayDuyet, opt => opt.Ignore()) // Giữ nguyên NgayDuyet
                .ForMember(dest => dest.CongTy, opt => opt.Ignore()) // Giữ nguyên CongTy (được quản lý riêng)
                .ForMember(dest => dest.AnhVanPhong, opt => opt.Ignore()); // Giữ nguyên AnhVanPhong (được xử lý riêng)
        }
    }
}

