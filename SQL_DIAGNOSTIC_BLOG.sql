-- =============================================
-- SQL DIAGNOSTIC SCRIPT FOR BLOG ISSUES
-- Kiểm tra và sửa các vấn đề với bảng Blogs
-- =============================================

USE [YourDatabaseName]; -- Thay đổi tên database của bạn
GO

PRINT '========================================';
PRINT 'BẮT ĐẦU KIỂM TRA DATABASE BLOG';
PRINT '========================================';
PRINT '';

-- =============================================
-- 1. THỐNG KÊ TỔNG QUAN
-- =============================================
PRINT '1. THỐNG KÊ TỔNG QUAN:';
PRINT '----------------------------------------';

SELECT 
    COUNT(*) AS 'Tổng số Blog',
    COUNT(CASE WHEN UserId IS NOT NULL THEN 1 END) AS 'Blog có UserId',
    COUNT(CASE WHEN UserId IS NULL THEN 1 END) AS 'Blog không có UserId',
    COUNT(CASE WHEN DaDang = 1 THEN 1 END) AS 'Blog đã đăng',
    COUNT(CASE WHEN HienThi = 1 THEN 1 END) AS 'Blog hiển thị',
    COUNT(CASE WHEN NguonBaiViet IS NOT NULL THEN 1 END) AS 'Blog từ API'
FROM Blogs;

PRINT '';

-- =============================================
-- 2. KIỂM TRA USERID KHÔNG HỢP LỆ
-- =============================================
PRINT '2. KIỂM TRA USERID KHÔNG HỢP LỆ:';
PRINT '----------------------------------------';

IF EXISTS (
    SELECT 1 
    FROM Blogs b
    LEFT JOIN AspNetUsers u ON b.UserId = u.Id
    WHERE b.UserId IS NOT NULL AND u.Id IS NULL
)
BEGIN
    PRINT '⚠️ CÓ BLOG VỚI USERID KHÔNG HỢP LỆ!';
    SELECT 
        b.MaBlog,
        b.TieuDe,
        b.UserId,
        b.NguonBaiViet,
        b.NgayDang
    FROM Blogs b
    LEFT JOIN AspNetUsers u ON b.UserId = u.Id
    WHERE b.UserId IS NOT NULL AND u.Id IS NULL;
END
ELSE
BEGIN
    PRINT '✅ Không có blog với UserId không hợp lệ';
END

PRINT '';

-- =============================================
-- 3. KIỂM TRA BLOG KHÔNG CÓ TIÊU ĐỀ
-- =============================================
PRINT '3. KIỂM TRA BLOG KHÔNG CÓ TIÊU ĐỀ:';
PRINT '----------------------------------------';

IF EXISTS (SELECT 1 FROM Blogs WHERE TieuDe IS NULL OR TieuDe = '')
BEGIN
    PRINT '⚠️ CÓ BLOG KHÔNG CÓ TIÊU ĐỀ!';
    SELECT 
        MaBlog,
        UserId,
        NguonBaiViet,
        NgayDang
    FROM Blogs
    WHERE TieuDe IS NULL OR TieuDe = '';
END
ELSE
BEGIN
    PRINT '✅ Tất cả blog đều có tiêu đề';
END

PRINT '';

-- =============================================
-- 4. KIỂM TRA BLOG KHÔNG CÓ NỘI DUNG
-- =============================================
PRINT '4. KIỂM TRA BLOG KHÔNG CÓ NỘI DUNG:';
PRINT '----------------------------------------';

IF EXISTS (SELECT 1 FROM Blogs WHERE NoiDung IS NULL OR NoiDung = '')
BEGIN
    PRINT '⚠️ CÓ BLOG KHÔNG CÓ NỘI DUNG!';
    SELECT 
        MaBlog,
        TieuDe,
        NguonBaiViet,
        NgayDang
    FROM Blogs
    WHERE NoiDung IS NULL OR NoiDung = '';
END
ELSE
BEGIN
    PRINT '✅ Tất cả blog đều có nội dung';
END

PRINT '';

-- =============================================
-- 5. KIỂM TRA BLOG TỪ API
-- =============================================
PRINT '5. THỐNG KÊ BLOG TỪ API:';
PRINT '----------------------------------------';

SELECT 
    COUNT(*) AS 'Tổng blog từ API',
    COUNT(CASE WHEN DaDang = 0 THEN 1 END) AS 'Chờ duyệt',
    COUNT(CASE WHEN DaDang = 1 AND HienThi = 0 THEN 1 END) AS 'Đã đăng - Ẩn',
    COUNT(CASE WHEN DaDang = 1 AND HienThi = 1 THEN 1 END) AS 'Đã đăng - Hiển thị'
FROM Blogs
WHERE NguonBaiViet IS NOT NULL;

PRINT '';
PRINT 'Danh sách blog từ API chờ duyệt:';
SELECT TOP 10
    MaBlog,
    TieuDe,
    NguonBaiViet,
    TacGia,
    NgayDang
FROM Blogs
WHERE NguonBaiViet IS NOT NULL AND DaDang = 0
ORDER BY NgayDang DESC;

PRINT '';

-- =============================================
-- 6. TỰ ĐỘNG SỬA CÁC VẤN ĐỀ (OPTIONAL)
-- =============================================
PRINT '========================================';
PRINT 'BẠN CÓ MUỐN SỬA CÁC VẤN ĐỀ KHÔNG?';
PRINT 'Nếu CÓ, hãy uncomment các lệnh UPDATE bên dưới';
PRINT '========================================';
PRINT '';

/*
-- UNCOMMENT CÁC DÒNG BÊN DƯỚI ĐỂ SỬA TỰ ĐỘNG

-- Sửa blog với UserId không hợp lệ
UPDATE b
SET UserId = NULL
FROM Blogs b
LEFT JOIN AspNetUsers u ON b.UserId = u.Id
WHERE b.UserId IS NOT NULL AND u.Id IS NULL;

PRINT '✅ Đã xóa UserId không hợp lệ';

-- Sửa blog không có tiêu đề
UPDATE Blogs
SET TieuDe = 'Blog #' + CAST(MaBlog AS NVARCHAR)
WHERE TieuDe IS NULL OR TieuDe = '';

PRINT '✅ Đã thêm tiêu đề mặc định';

-- Sửa blog không có nội dung
UPDATE Blogs
SET NoiDung = COALESCE(MoTaNgan, 'Nội dung đang được cập nhật...')
WHERE NoiDung IS NULL OR NoiDung = '';

PRINT '✅ Đã thêm nội dung mặc định';

PRINT '';
PRINT '========================================';
PRINT 'HOÀN TẤT SỬA LỖI!';
PRINT '========================================';
*/

-- =============================================
-- 7. KIỂM TRA LẠI SAU KHI SỬA
-- =============================================
/*
PRINT '';
PRINT '7. KIỂM TRA LẠI SAU KHI SỬA:';
PRINT '----------------------------------------';

-- Kiểm tra lại UserId
IF EXISTS (
    SELECT 1 
    FROM Blogs b
    LEFT JOIN AspNetUsers u ON b.UserId = u.Id
    WHERE b.UserId IS NOT NULL AND u.Id IS NULL
)
    PRINT '❌ Vẫn còn blog với UserId không hợp lệ!'
ELSE
    PRINT '✅ Đã sửa hết UserId không hợp lệ';

-- Kiểm tra lại tiêu đề
IF EXISTS (SELECT 1 FROM Blogs WHERE TieuDe IS NULL OR TieuDe = '')
    PRINT '❌ Vẫn còn blog không có tiêu đề!'
ELSE
    PRINT '✅ Đã sửa hết blog không có tiêu đề';

-- Kiểm tra lại nội dung
IF EXISTS (SELECT 1 FROM Blogs WHERE NoiDung IS NULL OR NoiDung = '')
    PRINT '❌ Vẫn còn blog không có nội dung!'
ELSE
    PRINT '✅ Đã sửa hết blog không có nội dung';
*/

PRINT '';
PRINT '========================================';
PRINT 'HOÀN TẤT KIỂM TRA!';
PRINT '========================================';
PRINT '';
PRINT 'Hướng dẫn:';
PRINT '1. Xem kết quả kiểm tra ở trên';
PRINT '2. Nếu có vấn đề, uncomment phần "TỰ ĐỘNG SỬA" và chạy lại';
PRINT '3. Sau khi sửa, uncomment phần "KIỂM TRA LẠI" để xác nhận';
PRINT '4. Refresh trang /Admin/Admin/Blog để xem kết quả';
GO

