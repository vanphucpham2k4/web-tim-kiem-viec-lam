-- Script SQL để cập nhật trạng thái cho các bản ghi hiện có
-- Chạy script này nếu bạn muốn set giá trị mặc định "Dang tuyen" cho các tin tuyển dụng đã có

UPDATE TinTuyenDungs 
SET TrangThai = 'Dang tuyen' 
WHERE TrangThai IS NULL;

