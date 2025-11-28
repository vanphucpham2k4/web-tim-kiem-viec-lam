namespace Unicareer.Models
{
    public class Blog
    {
        public int MaBlog { get; set; }
        public string TieuDe { get; set; } = string.Empty;
        public string? MoTaNgan { get; set; } // Excerpt/Summary
        public string NoiDung { get; set; } = string.Empty; // Full content
        public string? HinhAnh { get; set; } // Image URL
        public string? TheLoai { get; set; } // Category (deprecated - giữ lại để tương thích)
        public int? MaTheLoai { get; set; } // Foreign key đến TheLoaiBlog
        public TheLoaiBlog? TheLoaiBlog { get; set; } // Navigation property
        public string? TacGia { get; set; } // Author
        public string? Permalink { get; set; } // Permalink/Slug cho URL
        public bool IsPermalinkAuto { get; set; } = true; // Lưu lựa chọn liên kết cố định (true = tự động, false = tùy chỉnh)
        public string? Tags { get; set; } // Tags/Nhãn (lưu dưới dạng chuỗi phân cách bởi dấu phẩy)
        public DateTime NgayDang { get; set; }
        public DateTime? NgayCapNhat { get; set; }
        public int LuotXem { get; set; } = 0;
        public bool DaDang { get; set; } = false; // Đã đăng (false = bản nháp, true = đã đăng)
        public bool HienThi { get; set; } = false; // Hiển thị trên trang chủ (chỉ áp dụng khi DaDang = true)
        public string? UserId { get; set; } // Người tạo (admin)
        public ApplicationUser? User { get; set; } // Navigation property
    }
}

