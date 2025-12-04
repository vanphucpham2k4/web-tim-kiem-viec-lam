# HƯỚNG DẪN SỬ DỤNG API LẤY BÀI VIẾT

## 📌 Tổng quan

Hệ thống đã tích hợp 3 nguồn API để lấy bài viết về việc làm, kỹ năng, career:

1. **Dev.to API** ✅ (FREE - Không cần key)
2. **NewsAPI** (FREE - 100 requests/day)
3. **MediaStack API** (FREE - 500 requests/month)

## 🚀 Cách sử dụng

### 1. Dev.to API (Đã sẵn sàng - Không cần cấu hình)

Dev.to API được bật mặc định và không cần API key. Nguồn này cung cấp các bài viết về:
- Programming
- Career development
- Tech skills
- Job interviews
- Software development

### 2. NewsAPI (Tùy chọn)

**Lấy API Key:**
1. Truy cập: https://newsapi.org
2. Đăng ký tài khoản miễn phí
3. Lấy API key từ dashboard
4. Mở file `appsettings.json`
5. Thay thế `YOUR_NEWSAPI_KEY_HERE` bằng key của bạn
6. Đổi `"Enabled": false` thành `"Enabled": true`

```json
"ExternalAPIs": {
  "NewsAPI": {
    "ApiKey": "your_actual_key_here",
    "Enabled": true
  }
}
```

**Giới hạn:**
- FREE: 100 requests/day
- Bài viết: Tin tức về job, career, tech

### 3. MediaStack API (Tùy chọn)

**Lấy API Key:**
1. Truy cập: https://mediastack.com
2. Đăng ký tài khoản miễn phí
3. Lấy API key từ dashboard
4. Mở file `appsettings.json`
5. Thay thế `YOUR_MEDIASTACK_KEY_HERE` bằng key của bạn
6. Đổi `"Enabled": false` thành `"Enabled": true`

```json
"ExternalAPIs": {
  "MediaStack": {
    "ApiKey": "your_actual_key_here",
    "Enabled": true
  }
}
```

**Giới hạn:**
- FREE: 500 requests/month
- Bài viết: Tin tức quốc tế về nhiều chủ đề

## 📖 Hướng dẫn sử dụng trong Admin Panel

### Bước 1: Vào trang quản lý Blog
1. Đăng nhập Admin
2. Vào menu "Quản lý Blog"

### Bước 2: Import bài viết từ API
1. Click nút **"Import từ API"** (màu xanh lá)
2. Nhập từ khóa tìm kiếm (ví dụ: "career", "programming", "interview", "job")
3. Click **"Tìm kiếm"**
4. Hệ thống sẽ hiển thị danh sách bài viết từ các nguồn API

### Bước 3: Chọn bài viết để import
1. Xem preview của các bài viết
2. Click nút **"Import bài viết"** ở bài viết muốn import
3. Bài viết sẽ được lưu dưới dạng **Bản nháp**

### Bước 4: Duyệt và đăng bài viết
1. Các bài viết đã import sẽ hiển thị trong phần **"Bài viết từ API chờ duyệt"**
2. Click **"Xem & Duyệt"** để xem chi tiết
3. Chỉnh sửa nội dung nếu cần (tiêu đề, mô tả, thể loại, tags...)
4. Click **"Đăng"** để đăng bài viết
5. Click **"Hiện"** để hiển thị trên trang chủ

## ⚙️ Cấu hình nâng cao

### Tắt/Bật từng nguồn API

Trong file `appsettings.json`:

```json
"ExternalAPIs": {
  "NewsAPI": {
    "ApiKey": "your_key",
    "Enabled": true    // true = bật, false = tắt
  },
  "MediaStack": {
    "ApiKey": "your_key",
    "Enabled": false   // Tắt nếu không muốn dùng
  },
  "DevTo": {
    "Enabled": true    // Dev.to luôn nên bật vì FREE
  }
}
```

## 🔍 Từ khóa tìm kiếm gợi ý

- **Tiếng Anh:**
  - career, job, hiring, recruitment
  - programming, coding, developer
  - interview, resume, cv
  - skills, tech, software
  - remote, work, freelance
  
- **Chủ đề cụ thể:**
  - "javascript developer"
  - "career advice"
  - "job interview tips"
  - "programming skills"
  - "tech career"

## ⚠️ Lưu ý quan trọng

1. **Bài viết import sẽ ở trạng thái Bản nháp** - Cần duyệt trước khi đăng
2. **Kiểm tra nội dung** - Một số bài có thể không liên quan đến việc làm
3. **Chỉnh sửa trước khi đăng** - Có thể sửa tiêu đề, mô tả, thêm thể loại
4. **Tránh trùng lặp** - Hệ thống tự động ngăn import bài viết đã tồn tại
5. **Giới hạn API** - Lưu ý số lượng request với các API có giới hạn

## 🆘 Khắc phục sự cố

### Không tìm thấy bài viết
- Thử từ khóa khác
- Kiểm tra kết nối internet
- Kiểm tra API key (nếu dùng NewsAPI hoặc MediaStack)

### Lỗi khi import
- Kiểm tra xem bài viết đã được import chưa
- Kiểm tra kết nối database
- Xem log trong console để biết chi tiết lỗi

### API không hoạt động
- Kiểm tra API key có đúng không
- Kiểm tra `"Enabled": true` trong config
- Kiểm tra đã hết quota chưa (FREE tier có giới hạn)

## 📞 Hỗ trợ

Nếu cần hỗ trợ thêm, vui lòng liên hệ team dev hoặc xem log trong:
- Visual Studio Output Console
- Developer Tools Console (F12)

