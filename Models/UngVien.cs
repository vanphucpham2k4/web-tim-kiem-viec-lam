namespace Unicareer.Models
{
    public class UngVien
    {
        public int MaUngVien { get; set; }
        public string UserId { get; set; } = string.Empty; // Foreign key đến ApplicationUser
        public ApplicationUser? User { get; set; } // Navigation property
        public string HoTen { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string SoDienThoai { get; set; } = string.Empty;
        public DateTime NgaySinh { get; set; } = new DateTime(2000, 1, 1);
        public string? GioiTinh { get; set; }
        public string? DiaChi { get; set; }
        public string? HocVan { get; set; }
        public string? KinhNghiem { get; set; }
        public string? KyNang { get; set; }
        public string? NganhNghe { get; set; } // Giữ lại để tương thích ngược
        public int? MaChuyenNganh { get; set; } // Foreign key đến ChuyenNganh (nullable)
        public ChuyenNganh? ChuyenNganh { get; set; } // Navigation property
        public string? ChuyenNganhKhac { get; set; } // Tự nhập khi chọn "Khác..."
        public string? LinkCV { get; set; }
        public int SoLanUngTuyen { get; set; }
        public DateTime NgayDangKy { get; set; }
        
        // Các trường mới cho hồ sơ ứng viên
        public string? ViTriMongMuon { get; set; } // Vị trí mong muốn
        public decimal? MucLuongKyVong { get; set; } // Mức lương kỳ vọng
        public string? NoiLamViecMongMuon { get; set; } // Nơi làm việc mong muốn
        public string? CVFile { get; set; } // Đường dẫn file CV đã upload
        public string? MucTieuNgheNghiep { get; set; } // Mục tiêu nghề nghiệp
        public string? HocVanChiTiet { get; set; } // Học vấn chi tiết (JSON hoặc text)
        public string? KinhNghiemChiTiet { get; set; } // Kinh nghiệm chi tiết (JSON hoặc text)
        public string? KyNangChiTiet { get; set; } // Kỹ năng chi tiết (JSON hoặc text)
        public string? ChungChi { get; set; } // Chứng chỉ
        public string? LinkGitHub { get; set; } // Link GitHub
        public string? LinkBehance { get; set; } // Link Behance
        public string? LinkPortfolio { get; set; } // Link Portfolio khác
        public string? MoTaBanThan { get; set; } // Mô tả bản thân ngắn gọn
        public string? TrangThaiTimViec { get; set; } // "Đang tìm việc", "Đang thực tập", "Đã có việc"
        public bool HienThiCongKhai { get; set; } = false; // Hiển thị hồ sơ công khai (chỉ nhà tuyển dụng đã đăng nhập mới xem được)
    }
}

