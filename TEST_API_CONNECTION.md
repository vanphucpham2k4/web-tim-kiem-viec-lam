# 🔍 KIỂM TRA KẾT NỐI API

## Bước 1: Test Dev.to API trực tiếp

### Mở trình duyệt và truy cập:
```
https://dev.to/api/articles?tag=career&per_page=10
```

**Kết quả mong đợi:**
- Nếu thành công: Bạn sẽ thấy JSON data với danh sách bài viết
- Nếu lỗi: Sẽ báo lỗi hoặc không load được

## Bước 2: Kiểm tra Log trong Visual Studio

1. Chạy project với Visual Studio
2. Mở **Output** window (View > Output)
3. Chọn "Debug" trong dropdown
4. Tìm kiếm bài viết trong modal
5. Xem log có lỗi gì không

**Các lỗi thường gặp:**
- `Error fetching articles from external APIs`
- `Dev.to API returned status code: XXX`
- Timeout errors

## Bước 3: Kiểm tra Browser Console

1. Mở trang Blog Admin
2. Nhấn **F12** để mở Developer Tools
3. Chuyển sang tab **Console**
4. Click "Import từ API"
5. Nhập từ khóa và tìm kiếm
6. Xem Console có báo lỗi gì không

**Lỗi có thể gặp:**
- Network error
- CORS error
- 404, 500 errors
- JSON parse error

## Bước 4: Kiểm tra Network Request

1. Mở Developer Tools (F12)
2. Chuyển sang tab **Network**
3. Click "Import từ API"
4. Nhập từ khóa `career` và tìm kiếm
5. Xem request đến `/Admin/Admin/TimKiemBaiVietAPI`
6. Click vào request đó để xem:
   - Status code (phải là 200)
   - Response data
   - Request payload

## Bước 5: Test Manual

### Copy đoạn code này vào Browser Console:
```javascript
fetch('/Admin/Admin/TimKiemBaiVietAPI', {
    method: 'POST',
    headers: {
        'Content-Type': 'application/x-www-form-urlencoded',
    },
    body: new URLSearchParams({
        keyword: 'career',
        pageSize: 10,
        __RequestVerificationToken: document.querySelector('input[name="__RequestVerificationToken"]').value
    })
})
.then(res => res.json())
.then(data => console.log('Result:', data))
.catch(err => console.error('Error:', err));
```

**Kết quả:**
- Nếu thành công: Console sẽ hiển thị object với `success: true` và `data: [...]`
- Nếu lỗi: Sẽ báo lỗi cụ thể

## Bước 6: Kiểm tra Service Configuration

### Kiểm tra file appsettings.json:
```json
"ExternalAPIs": {
  "DevTo": {
    "Enabled": true   // Phải là true
  }
}
```

## Các Lỗi Thường Gặp & Cách Fix

### 1. "Không tìm thấy bài viết nào"
**Nguyên nhân:**
- API trả về rỗng
- Từ khóa không có kết quả
- Dev.to API đang bảo trì

**Cách fix:**
- Thử từ khóa khác: `programming`, `javascript`, `react`
- Kiểm tra internet
- Test API trực tiếp (Bước 1)

### 2. "Network Error" hoặc CORS
**Nguyên nhân:**
- Không có kết nối internet
- Firewall chặn
- Dev.to API down

**Cách fix:**
- Kiểm tra internet
- Tắt firewall/antivirus tạm thời
- Đợi Dev.to API hoạt động lại

### 3. "RequestVerificationToken not found"
**Nguyên nhân:**
- Thiếu @Html.AntiForgeryToken() trong view

**Cách fix:**
- File Blog.cshtml đã có token rồi, không cần sửa

### 4. Status Code 500 (Server Error)
**Nguyên nhân:**
- Lỗi trong Controller/Service
- Exception không được handle

**Cách fix:**
- Xem Output log chi tiết
- Kiểm tra stack trace

### 5. JSON Parse Error
**Nguyên nhân:**
- Dev.to API trả về format khác
- Response không phải JSON

**Cách fix:**
- Xem Network tab > Response
- Copy response và check format

## Lệnh Debug Nhanh

### 1. Rebuild Project
```bash
dotnet clean
dotnet build
```

### 2. Chạy với Verbose Logging
Thêm vào appsettings.Development.json:
```json
"Logging": {
  "LogLevel": {
    "Default": "Debug",
    "Microsoft.AspNetCore": "Information",
    "Unicareer.Services.ExternalArticleService": "Debug"
  }
}
```

### 3. Test HttpClient trực tiếp
Tạo file test đơn giản:
```csharp
using var httpClient = new HttpClient();
var response = await httpClient.GetAsync("https://dev.to/api/articles?tag=career&per_page=5");
var content = await response.Content.ReadAsStringAsync();
Console.WriteLine(content);
```

## Checklist Kiểm Tra

- [ ] Internet đang hoạt động
- [ ] Project build thành công
- [ ] Đã đăng nhập Admin
- [ ] Ở đúng trang /Admin/Admin/Blog
- [ ] Click "Import từ API" mở được modal
- [ ] Dev.to API test trực tiếp OK (Bước 1)
- [ ] Console không có lỗi (F12)
- [ ] Network request trả về 200
- [ ] Response có data

## Nếu Vẫn Không Được

Gửi cho tôi:
1. Screenshot của Console (F12)
2. Screenshot của Network tab
3. Log trong Visual Studio Output
4. Kết quả test Dev.to API trực tiếp

---

**Lưu ý:** Dev.to API là public và FREE, không cần authentication, nên rất ít khi bị lỗi.

