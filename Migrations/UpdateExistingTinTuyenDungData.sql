-- Script SQL để cập nhật MaNhaTuyenDung cho các tin tuyển dụng đã có
-- Chạy script này SAU KHI đã chạy migration AddMaNhaTuyenDungToTinTuyenDung

-- Cập nhật MaNhaTuyenDung dựa trên tên công ty (CongTy) khớp với TenCongTy trong bảng NhaTuyenDungs
UPDATE t
SET t.MaNhaTuyenDung = n.MaNhaTuyenDung
FROM TinTuyenDungs t
INNER JOIN NhaTuyenDungs n ON LOWER(LTRIM(RTRIM(t.CongTy))) = LOWER(LTRIM(RTRIM(n.TenCongTy)))
WHERE t.MaNhaTuyenDung IS NULL;

-- Kiểm tra kết quả
SELECT 
    t.MaTinTuyenDung,
    t.CongTy,
    t.MaNhaTuyenDung,
    n.TenCongTy,
    n.MaNhaTuyenDung AS NhaTuyenDung_MaNhaTuyenDung
FROM TinTuyenDungs t
LEFT JOIN NhaTuyenDungs n ON t.MaNhaTuyenDung = n.MaNhaTuyenDung
ORDER BY t.MaTinTuyenDung;

