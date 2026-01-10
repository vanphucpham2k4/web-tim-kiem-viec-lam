using AutoMapper;
using Unicareer.Models;

namespace Unicareer.Mappings
{
    /// <summary>
    /// AutoMapper Profile cho các Master Data entities (NganhNghe, LoaiCongViec, ChuyenNganh, TheLoaiBlog)
    /// Xử lý mapping giữa các đối tượng master data khi cập nhật
    /// </summary>
    public class MasterDataMappingProfile : Profile
    {
        public MasterDataMappingProfile()
        {
            // Mapping cho NganhNghe
            CreateMap<NganhNghe, NganhNghe>()
                .ForMember(dest => dest.MaNganhNghe, opt => opt.Ignore()) // Giữ nguyên MaNganhNghe
                .ForMember(dest => dest.SoLuongCongViec, opt => opt.Ignore()) // Giữ nguyên SoLuongCongViec
                .ForMember(dest => dest.NgayTao, opt => opt.Ignore()); // Giữ nguyên NgayTao

            // Mapping cho LoaiCongViec
            CreateMap<LoaiCongViec, LoaiCongViec>()
                .ForMember(dest => dest.MaLoaiCongViec, opt => opt.Ignore()) // Giữ nguyên MaLoaiCongViec
                .ForMember(dest => dest.SoLuongViTri, opt => opt.Ignore()) // Giữ nguyên SoLuongViTri
                .ForMember(dest => dest.NgayTao, opt => opt.Ignore()); // Giữ nguyên NgayTao

            // Mapping cho ChuyenNganh
            CreateMap<ChuyenNganh, ChuyenNganh>()
                .ForMember(dest => dest.MaChuyenNganh, opt => opt.Ignore()) // Giữ nguyên MaChuyenNganh
                .ForMember(dest => dest.NganhNghe, opt => opt.Ignore()) // Giữ nguyên navigation property
                .ForMember(dest => dest.NgayTao, opt => opt.Ignore()) // Giữ nguyên NgayTao
                .ForMember(dest => dest.IsActive, opt => opt.Ignore()); // Giữ nguyên IsActive

            // Mapping cho TheLoaiBlog
            CreateMap<TheLoaiBlog, TheLoaiBlog>()
                .ForMember(dest => dest.MaTheLoai, opt => opt.Ignore()) // Giữ nguyên MaTheLoai
                .ForMember(dest => dest.NgayTao, opt => opt.Ignore()) // Giữ nguyên NgayTao
                .ForMember(dest => dest.HienThi, opt => opt.Ignore()) // Giữ nguyên HienThi
                .ForMember(dest => dest.ThuTu, opt => opt.Ignore()); // Giữ nguyên ThuTu
        }
    }
}

