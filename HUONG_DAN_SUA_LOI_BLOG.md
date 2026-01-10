# Hướng Dẫn Sửa Lỗi Blog Không Hiển Thị

## Vấn Đề

Trang `/Admin/Admin/Blog` không hiển thị danh sách blog hoặc bị lỗi khi load, trong khi `TestApiDirectly.html` vẫn hoạt động bình thường.

## Nguyên Nhân

Có thể có một hoặc nhiều blog trong database có vấn đề:

1. **UserId không hợp lệ**: Blog có `UserId` trỏ đến user không tồn tại
2. **Thiếu dữ liệu bắt buộc**: Blog không có tiêu đề hoặc nội dung
3. **Lỗi relationship**: Quan hệ giữa Blog và User bị lỗi trong database

## Giải Pháp

### Cách 1: Sử Dụng Công Cụ Tự Động (Khuyến Nghị)

1. **Mở trình duyệt và truy cập:**
   ```
   http://localhost:7188/TestBlogDatabase.html
   ```

2. **Kiểm tra database:**
   - Nhấn nút "🔍 Kiểm tra Database"
   - Xem kết quả các blog có vấn đề

3. **Sửa database:**
   - Nhấn nút "🔧 Sửa Database"
   - Hệ thống sẽ tự động:
     - Xóa các `UserId` không hợp lệ
     - Thêm tiêu đề mặc định cho blog không có tiêu đề
     - Thêm nội dung mặc định cho blog không có nội dung

4. **Kiểm tra lại:**
   - Truy cập `/Admin/Admin/Blog`
   - Danh sách blog sẽ hiển thị bình thường

### Cách 2: Kiểm Tra Bằng SQL

Nếu cách 1 không hoạt động, bạn có thể kiểm tra trực tiếp database:

```sql
-- Kiểm tra blog với UserId không hợp lệ
SELECT b.MaBlog, b.TieuDe, b.UserId, b.NguonBaiViet
FROM Blogs b
LEFT JOIN AspNetUsers u ON b.UserId = u.Id
WHERE b.UserId IS NOT NULL AND u.Id IS NULL;

-- Kiểm tra blog không có tiêu đề
SELECT MaBlog, UserId, NguonBaiViet
FROM Blogs
WHERE TieuDe IS NULL OR TieuDe = '';

-- Kiểm tra blog không có nội dung
SELECT MaBlog, TieuDe, NguonBaiViet
FROM Blogs
WHERE NoiDung IS NULL OR NoiDung = '';

-- Sửa blog với UserId không hợp lệ
UPDATE Blogs
SET UserId = NULL
WHERE UserId IS NOT NULL 
  AND UserId NOT IN (SELECT Id FROM AspNetUsers);

-- Sửa blog không có tiêu đề
UPDATE Blogs
SET TieuDe = 'Blog #' + CAST(MaBlog AS NVARCHAR)
WHERE TieuDe IS NULL OR TieuDe = '';

-- Sửa blog không có nội dung
UPDATE Blogs
SET NoiDung = COALESCE(MoTaNgan, 'Nội dung đang được cập nhật...')
WHERE NoiDung IS NULL OR NoiDung = '';
```

### Cách 3: Kiểm Tra Logs

1. **Xem logs của ứng dụng:**
   - Check console output khi chạy ứng dụng
   - Tìm log entries từ `AdminController.Blog()`
   - Log sẽ hiển thị lỗi cụ thể nếu có

2. **Log patterns để tìm:**
   ```
   Error fetching blogs: ...
   Fetched X blogs from repository
   No blogs found in database
   ```

## Cải Tiến Đã Thực Hiện

### 1. AdminController.cs

- ✅ Thêm try-catch và logging cho method `Blog()`
- ✅ Thêm null checks toàn diện
- ✅ Thêm endpoint `KiemTraBlogDatabase()` để kiểm tra
- ✅ Thêm endpoint `SuaBlogDatabase()` để tự động sửa lỗi

### 2. BlogRepository.cs

- ✅ Thêm fallback khi `.Include(b => b.User)` bị lỗi
- ✅ Các method sẽ vẫn trả về dữ liệu ngay cả khi có lỗi với User relationship

### 3. Blog.cshtml

- ✅ View đã có sẵn null checks cho User
- ✅ Hiển thị thông tin tác giả từ nhiều nguồn: User, TacGia, hoặc "Admin"

## Kiểm Tra Sau Khi Sửa

1. **Truy cập trang Blog:**
   ```
   http://localhost:7188/Admin/Admin/Blog
   ```

2. **Kiểm tra các chức năng:**
   - Danh sách blog hiển thị đầy đủ
   - Import từ API hoạt động
   - Tìm kiếm hoạt động
   - Pagination hoạt động
   - Các nút Sửa/Xóa/Đăng hoạt động

3. **Kiểm tra các blog từ API:**
   - Xem phần "Bài viết từ API chờ duyệt"
   - Nhấn "Xem & Duyệt" để xem chi tiết
   - Đăng blog sau khi duyệt

## Lưu Ý

- **UserId là optional**: Blog từ API có thể không có UserId, nhưng vẫn có `TacGia`
- **NguonBaiViet**: Blog từ API sẽ có trường này để phân biệt với blog tự viết
- **ApiArticleId**: Dùng để tránh import trùng bài viết từ API

## Liên Hệ

Nếu vẫn gặp vấn đề, vui lòng:
1. Check logs chi tiết trong console
2. Chạy công cụ TestBlogDatabase.html để xem báo cáo chi tiết
3. Kiểm tra database connection
4. Đảm bảo đã login với tài khoản Admin

## Tài Liệu Liên Quan

- [HUONG_DAN_API_BAI_VIET.md](./HUONG_DAN_API_BAI_VIET.md) - Hướng dẫn sử dụng API import bài viết
- [CHANGELOG_API_INTEGRATION.md](./CHANGELOG_API_INTEGRATION.md) - Lịch sử thay đổi
- [TEST_API_CONNECTION.md](./TEST_API_CONNECTION.md) - Test kết nối API

