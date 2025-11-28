using AutoMapper;
using Unicareer.Models;

namespace Unicareer.Mappings
{
    /// <summary>
    /// AutoMapper Profile cho UngVien entity
    /// Xử lý mapping giữa các đối tượng UngVien khi cập nhật
    /// </summary>
    public class UngVienMappingProfile : Profile
    {
        public UngVienMappingProfile()
        {
            // Mapping từ UngVien nguồn (từ form/tham số) sang UngVien đích (từ database)
            // Sử dụng cho việc cập nhật ứng viên hiện có
            CreateMap<UngVien, UngVien>()
                .ForMember(dest => dest.MaUngVien, opt => opt.Ignore()) // Giữ nguyên MaUngVien
                .ForMember(dest => dest.UserId, opt => opt.Ignore()) // Giữ nguyên UserId
                .ForMember(dest => dest.User, opt => opt.Ignore()) // Giữ nguyên navigation property
                .ForMember(dest => dest.ChuyenNganh, opt => opt.Ignore()) // Giữ nguyên navigation property
                .ForMember(dest => dest.NgayDangKy, opt => opt.Ignore()) // Giữ nguyên NgayDangKy
                .ForMember(dest => dest.SoLanUngTuyen, opt => opt.Ignore()) // Giữ nguyên SoLanUngTuyen
                .ForMember(dest => dest.LinkCV, opt => opt.Ignore()) // Giữ nguyên LinkCV (được xử lý riêng)
                .ForMember(dest => dest.CVFile, opt => opt.Condition(src => !string.IsNullOrEmpty(src.CVFile))) // Chỉ cập nhật CVFile nếu có giá trị mới
                .ForMember(dest => dest.HoTen, opt => opt.Condition(src => !string.IsNullOrEmpty(src.HoTen))) // Chỉ cập nhật HoTen nếu có giá trị mới
                .ForMember(dest => dest.Email, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Email))) // Chỉ cập nhật Email nếu có giá trị mới
                .ForMember(dest => dest.SoDienThoai, opt => opt.Condition(src => !string.IsNullOrEmpty(src.SoDienThoai))) // Chỉ cập nhật SoDienThoai nếu có giá trị mới
                .ForMember(dest => dest.NgaySinh, opt => opt.Condition(src => src.NgaySinh > new DateTime(1900, 1, 1))); // Chỉ cập nhật NgaySinh nếu có giá trị hợp lệ (sau năm 1900)
        }
    }
}

