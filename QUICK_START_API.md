# 🚀 QUICK START - Sử dụng Import Bài Viết từ API

## ✅ ĐÃ HOÀN THÀNH

Hệ thống đã sẵn sàng sử dụng ngay! Không cần cấu hình thêm.

## 📋 CÁC BƯỚC SỬ DỤNG

### 1️⃣ Đăng nhập Admin
```
URL: https://localhost:7xxx/admin/login
```

### 2️⃣ Vào trang Quản lý Blog
- Click menu "Quản lý Blog"
- Hoặc truy cập: `/Admin/Admin/Blog`

### 3️⃣ Import bài viết từ API
1. Click nút **"Import từ API"** (màu xanh lá)
2. Nhập từ khóa tìm kiếm:
   - `career` - Nghề nghiệp
   - `programming` - Lập trình
   - `interview` - Phỏng vấn
   - `job` - Công việc
   - `developer` - Lập trình viên
   - `software engineer` - Kỹ sư phần mềm

3. Click **"Tìm kiếm"**
4. Hệ thống sẽ hiển thị bài viết từ Dev.to
5. Click **"Import bài viết"** ở bài bạn muốn

### 4️⃣ Duyệt bài viết
1. Bài viết được import sẽ hiện ở phần **"Bài viết từ API chờ duyệt"**
2. Click **"Xem & Duyệt"**
3. Chỉnh sửa nếu cần:
   - Tiêu đề
   - Mô tả ngắn
   - Thể loại
   - Tags
4. Click **"Đăng"** để đăng bài
5. Click **"Hiện"** để hiển thị trên trang chủ

## 🎯 NGUỒN API ĐANG SỬ DỤNG

### ✅ Dev.to API (FREE - Sẵn sàng)
- 🟢 **Trạng thái**: ACTIVE
- 🆓 **Chi phí**: Miễn phí hoàn toàn
- 📊 **Giới hạn**: Không giới hạn
- 📝 **Nội dung**: Bài viết về programming, career, tech

### ⚪ NewsAPI (Optional)
- ⚙️ **Cần cấu hình**: API Key
- 🔗 **Đăng ký**: https://newsapi.org
- 🆓 **FREE Tier**: 100 requests/ngày
- 📝 **Nội dung**: Tin tức về job, tech

### ⚪ MediaStack (Optional)
- ⚙️ **Cần cấu hình**: API Key  
- 🔗 **Đăng ký**: https://mediastack.com
- 🆓 **FREE Tier**: 500 requests/tháng
- 📝 **Nội dung**: Tin tức quốc tế

## 🔧 THÊM API MỚI (Tùy chọn)

### NewsAPI
1. Đăng ký tại: https://newsapi.org
2. Lấy API Key
3. Mở `appsettings.json`
4. Thay đổi:
```json
"NewsAPI": {
  "ApiKey": "paste_key_here",
  "Enabled": true
}
```

### MediaStack
1. Đăng ký tại: https://mediastack.com
2. Lấy API Key
3. Mở `appsettings.json`
4. Thay đổi:
```json
"MediaStack": {
  "ApiKey": "paste_key_here",
  "Enabled": true
}
```

## 💡 MẸO SỬ DỤNG

1. **Tìm kiếm hiệu quả**: Dùng từ khóa cụ thể
   - ✅ "react developer career"
   - ✅ "python programming tips"
   - ❌ "work" (quá chung chung)

2. **Kiểm tra trước khi đăng**: 
   - Đọc nội dung bài viết
   - Kiểm tra hình ảnh
   - Sửa tiêu đề cho phù hợp

3. **Phân loại đúng**: 
   - Chọn thể loại blog phù hợp
   - Thêm tags để dễ tìm kiếm

4. **Tránh trùng lặp**:
   - Hệ thống tự động ngăn import bài trùng
   - Kiểm tra "Nguồn bài viết" để biết đã import chưa

## 📊 THỐNG KÊ & GIÁM SÁT

- **Bài viết chờ duyệt**: Hiển thị ngay trên trang Blog
- **Nguồn bài viết**: Lưu trong database (cột `NguonBaiViet`)
- **Tránh trùng**: Dựa vào `ApiArticleId`

## 🐛 XỬ LÝ LỖI THƯỜNG GẶP

### "Không tìm thấy bài viết"
- Thử từ khóa khác
- Kiểm tra kết nối internet

### "Bài viết này đã được import"
- Bài viết đã tồn tại trong hệ thống
- Kiểm tra danh sách blog

### Modal không mở
- Refresh lại trang
- Xóa cache trình duyệt (Ctrl + F5)

## 📱 HỖ TRỢ

Nếu gặp vấn đề, kiểm tra:
1. Console log (F12 > Console)
2. Network tab (F12 > Network)
3. Server logs (Visual Studio Output)

---

**Lưu ý**: Dev.to API đã sẵn sàng sử dụng ngay không cần cấu hình gì thêm! 🎉

