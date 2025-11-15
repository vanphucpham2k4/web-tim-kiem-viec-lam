namespace Unicareer.Models.Enums
{
    /// <summary>
    /// Helper class để chuyển đổi giữa enum TrangThaiXuLy và string
    /// </summary>
    public static class TrangThaiXuLyHelper
    {
        /// <summary>
        /// Chuyển enum sang string tiếng Việt có dấu
        /// </summary>
        public static string ToString(TrangThaiXuLy trangThai)
        {
            return trangThai switch
            {
                TrangThaiXuLy.DangXemXet => "Đang xem xét",
                TrangThaiXuLy.ChoPhongVan => "Chờ phỏng vấn",
                TrangThaiXuLy.DaPhongVan => "Đã phỏng vấn",
                TrangThaiXuLy.TuyenDung => "Tuyển dụng",
                TrangThaiXuLy.TuChoi => "Từ chối",
                TrangThaiXuLy.KhongPhuHop => "Không phù hợp",
                _ => "Không xác định"
            };
        }

        /// <summary>
        /// Chuyển string sang enum (hỗ trợ cả format cũ và mới)
        /// </summary>
        public static TrangThaiXuLy? FromString(string? trangThai)
        {
            if (string.IsNullOrWhiteSpace(trangThai))
                return null;

            // Format mới (tiếng Việt có dấu)
            return trangThai.Trim() switch
            {
                "Đang xem xét" => TrangThaiXuLy.DangXemXet,
                "Chờ phỏng vấn" => TrangThaiXuLy.ChoPhongVan,
                "Đã phỏng vấn" => TrangThaiXuLy.DaPhongVan,
                "Tuyển dụng" => TrangThaiXuLy.TuyenDung,
                "Từ chối" => TrangThaiXuLy.TuChoi,
                "Không phù hợp" => TrangThaiXuLy.KhongPhuHop,
                // Format cũ (không dấu) - để tương thích với dữ liệu cũ
                "Dang xem xet" => TrangThaiXuLy.DangXemXet,
                "Cho phong van" => TrangThaiXuLy.ChoPhongVan,
                "Da phong van" => TrangThaiXuLy.DaPhongVan,
                "Tuyen dung" => TrangThaiXuLy.TuyenDung,
                "Tu choi" => TrangThaiXuLy.TuChoi,
                "Khong phu hop" => TrangThaiXuLy.KhongPhuHop,
                _ => null
            };
        }

        /// <summary>
        /// Lấy class CSS cho badge dựa trên trạng thái
        /// </summary>
        public static string GetBadgeClass(TrangThaiXuLy trangThai)
        {
            return trangThai switch
            {
                TrangThaiXuLy.DangXemXet => "bg-warning",
                TrangThaiXuLy.ChoPhongVan => "bg-info",
                TrangThaiXuLy.DaPhongVan => "bg-primary",
                TrangThaiXuLy.TuyenDung => "bg-success",
                TrangThaiXuLy.TuChoi => "bg-danger",
                TrangThaiXuLy.KhongPhuHop => "bg-danger",
                _ => "bg-secondary"
            };
        }

        /// <summary>
        /// Lấy class CSS cho badge từ string (hỗ trợ cả format cũ và mới)
        /// </summary>
        public static string GetBadgeClassFromString(string? trangThai)
        {
            var enumValue = FromString(trangThai);
            if (enumValue.HasValue)
            {
                return GetBadgeClass(enumValue.Value);
            }
            return "bg-secondary";
        }
    }
}

