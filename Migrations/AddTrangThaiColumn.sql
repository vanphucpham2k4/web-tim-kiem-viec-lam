-- Script SQL để thêm cột TrangThai vào bảng TinTuyenDungs
-- Chạy script này trong SQL Server Management Studio hoặc Azure Data Studio

-- Thêm cột TrangThai
ALTER TABLE TinTuyenDungs
ADD TrangThai NVARCHAR(MAX) NULL;

-- Cập nhật các bản ghi hiện có thành "Dang tuyen" nếu null
UPDATE TinTuyenDungs 
SET TrangThai = 'Dang tuyen' 
WHERE TrangThai IS NULL;

-- Đặt giá trị mặc định cho các bản ghi mới (tùy chọn)
-- ALTER TABLE TinTuyenDungs
-- ADD CONSTRAINT DF_TinTuyenDungs_TrangThai DEFAULT 'Dang tuyen' FOR TrangThai;

