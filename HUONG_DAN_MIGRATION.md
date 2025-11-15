# Hướng dẫn áp dụng Migration: Thêm Foreign Key MaNhaTuyenDung

## Vấn đề đã được giải quyết

Trước đây, bảng `TinTuyenDung` chỉ lưu tên công ty (`CongTy`) dưới dạng string để liên kết với `NhaTuyenDung`. Điều này gây ra vấn đề khi export/import database vì:
- Tên công ty có thể bị thay đổi (encoding, case sensitivity, khoảng trắng)
- Không có quan hệ thực sự giữa các bảng
- Khó xác định chính xác tin nào thuộc về công ty nào

## Giải pháp

Đã thêm foreign key `MaNhaTuyenDung` vào bảng `TinTuyenDung` để tạo quan hệ thực sự với bảng `NhaTuyenDung`.

## Các bước thực hiện

### 1. Dừng ứng dụng (nếu đang chạy)

Đảm bảo ứng dụng không đang chạy để có thể build và chạy migration.

### 2. Tạo lại file Designer.cs (nếu cần)

Nếu file `Migrations/20251111230433_AddMaNhaTuyenDungToTinTuyenDung.Designer.cs` chưa có, bạn có thể:

**Cách 1: Xóa migration và tạo lại (khuyến nghị)**
```bash
dotnet ef migrations remove
dotnet ef migrations add AddMaNhaTuyenDungToTinTuyenDung
```

**Cách 2: Chạy migration trực tiếp (nếu file Designer.cs đã có)**
```bash
dotnet ef database update
```

### 3. Chạy migration

```bash
dotnet ef database update
```

### 4. Cập nhật dữ liệu hiện có

Sau khi migration chạy xong, chạy script SQL để cập nhật `MaNhaTuyenDung` cho các tin tuyển dụng đã có:

```sql
-- File: Migrations/UpdateExistingTinTuyenDungData.sql
UPDATE t
SET t.MaNhaTuyenDung = n.MaNhaTuyenDung
FROM TinTuyenDungs t
INNER JOIN NhaTuyenDungs n ON LOWER(LTRIM(RTRIM(t.CongTy))) = LOWER(LTRIM(RTRIM(n.TenCongTy)))
WHERE t.MaNhaTuyenDung IS NULL;
```

Hoặc mở file `Migrations/UpdateExistingTinTuyenDungData.sql` và chạy trong SQL Server Management Studio.

### 5. Kiểm tra kết quả

Sau khi cập nhật, kiểm tra xem các tin tuyển dụng đã có `MaNhaTuyenDung` chưa:

```sql
SELECT 
    t.MaTinTuyenDung,
    t.CongTy,
    t.MaNhaTuyenDung,
    n.TenCongTy
FROM TinTuyenDungs t
LEFT JOIN NhaTuyenDungs n ON t.MaNhaTuyenDung = n.MaNhaTuyenDung
ORDER BY t.MaTinTuyenDung;
```

## Thay đổi trong code

1. **Model `TinTuyenDung`**: Đã thêm `MaNhaTuyenDung` và navigation property `NhaTuyenDung`
2. **ApplicationDbContext**: Đã cấu hình quan hệ foreign key
3. **Repository**: Đã thêm method `LayDanhSachTheoMaNhaTuyenDung()`
4. **RecruiterController**: Đã cập nhật để sử dụng `MaNhaTuyenDung` thay vì chỉ dựa vào tên công ty

## Lưu ý

- Các tin tuyển dụng mới sẽ tự động có `MaNhaTuyenDung` khi đăng tin
- Các tin cũ cần được cập nhật bằng script SQL ở bước 4
- Trường `CongTy` vẫn được giữ lại để tương thích ngược và hiển thị

