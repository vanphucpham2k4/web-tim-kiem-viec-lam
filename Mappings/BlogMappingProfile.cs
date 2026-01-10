using AutoMapper;
using Unicareer.Models;

namespace Unicareer.Mappings
{
    /// <summary>
    /// AutoMapper Profile cho Blog entity
    /// Xử lý mapping giữa các đối tượng Blog khi cập nhật
    /// </summary>
    public class BlogMappingProfile : Profile
    {
        public BlogMappingProfile()
        {
            // Mapping từ Blog nguồn (từ form) sang Blog đích (từ database)
            // Sử dụng cho việc cập nhật blog hiện có
            CreateMap<Blog, Blog>()
                .ForMember(dest => dest.MaBlog, opt => opt.Ignore()) // Giữ nguyên MaBlog
                .ForMember(dest => dest.NgayDang, opt => opt.Ignore()) // Giữ nguyên NgayDang
                .ForMember(dest => dest.LuotXem, opt => opt.Ignore()) // Giữ nguyên LuotXem
                .ForMember(dest => dest.UserId, opt => opt.Ignore()) // Giữ nguyên UserId
                .ForMember(dest => dest.User, opt => opt.Ignore()) // Giữ nguyên navigation property
                .ForMember(dest => dest.TheLoaiBlog, opt => opt.Ignore()) // Giữ nguyên navigation property
                .ForMember(dest => dest.NgayCapNhat, opt => opt.MapFrom(src => DateTime.Now)) // Tự động cập nhật NgayCapNhat
                .ForMember(dest => dest.HinhAnh, opt => opt.Condition(src => !string.IsNullOrEmpty(src.HinhAnh))); // Chỉ cập nhật HinhAnh nếu có giá trị mới
        }
    }
}

