# Hướng dẫn đăng ký Google Cloud và cấu hình OAuth

## Mục đích
Hướng dẫn này giúp bạn tạo Google OAuth 2.0 Client ID và Client Secret để sử dụng tính năng đăng nhập/đăng ký bằng Google trong ứng dụng Unicareer.

---

## Bước 1: Tạo tài khoản Google Cloud

1. Truy cập [Google Cloud Console](https://console.cloud.google.com/)
2. Đăng nhập bằng tài khoản Google của bạn
3. Nếu chưa có project, bạn sẽ được yêu cầu chấp nhận điều khoản dịch vụ

---

## Bước 2: Tạo Project mới

1. Ở góc trên cùng, click vào dropdown project (bên cạnh logo Google Cloud)
2. Click **"New Project"** (Dự án mới)
3. Điền thông tin:
   - **Project name**: `Unicareer` (hoặc tên bạn muốn)
   - **Organization**: Để trống (nếu không có)
   - **Location**: Chọn "No organization"
4. Click **"Create"** (Tạo)
5. Đợi vài giây để project được tạo xong

---

## Bước 3: Kích hoạt Google+ API

1. Trong Google Cloud Console, vào menu **"APIs & Services"** (APIs và Dịch vụ) > **"Library"** (Thư viện)
2. Tìm kiếm **"Google+ API"** hoặc **"People API"**
3. Chọn **"Google+ API"** hoặc **"Google People API"**
4. Click **"Enable"** (Bật) để kích hoạt API

**Lưu ý**: Google+ API đã bị deprecated, nhưng bạn vẫn có thể dùng **"Google People API"** hoặc chỉ cần **"OAuth 2.0"** là đủ.

---

## Bước 4: Tạo OAuth 2.0 Credentials

### 4.1. Cấu hình OAuth Consent Screen

1. Vào **"APIs & Services"** > **"OAuth consent screen"** (Màn hình đồng ý OAuth)
2. Chọn **"External"** (Bên ngoài) và click **"Create"**
3. Điền thông tin:
   - **App name**: `Unicareer` (hoặc tên ứng dụng của bạn)
   - **User support email**: Email hỗ trợ của bạn
   - **Developer contact information**: Email liên hệ của bạn
4. Click **"Save and Continue"** (Lưu và Tiếp tục)
5. Ở màn hình **"Scopes"**, click **"Save and Continue"** (có thể bỏ qua)
6. Ở màn hình **"Test users"**, click **"Save and Continue"** (có thể bỏ qua)
7. Xem lại và click **"Back to Dashboard"** (Quay lại Bảng điều khiển)

### 4.2. Tạo OAuth 2.0 Client ID

1. Vào **"APIs & Services"** > **"Credentials"** (Thông tin xác thực)
2. Click **"+ CREATE CREDENTIALS"** (Tạo thông tin xác thực)
3. Chọn **"OAuth client ID"** (ID khách hàng OAuth)
4. Chọn loại ứng dụng:
   - **Application type**: Chọn **"Web application"** (Ứng dụng web)
   - **Name**: `Unicareer Web Client` (hoặc tên bạn muốn)
5. Cấu hình **Authorized redirect URIs** (URI chuyển hướng được ủy quyền):
   
   **Cho môi trường Development (Localhost):**
   ```
   http://localhost:5000/Account/GoogleCallback
   http://localhost:5001/Account/GoogleCallback
   http://127.0.0.1:5000/Account/GoogleCallback
   http://127.0.0.1:5001/Account/GoogleCallback
   ```
   
   **Cho môi trường Production:**
   ```
   https://yourdomain.com/Account/GoogleCallback
   https://www.yourdomain.com/Account/GoogleCallback
   ```
   
   **Lưu ý**: Thay `yourdomain.com` bằng domain thực tế của bạn.

6. Click **"Create"** (Tạo)
7. Màn hình sẽ hiển thị:
   - **Your Client ID** (ID khách hàng của bạn)
   - **Your Client Secret** (Bí mật khách hàng của bạn)

---

## Bước 5: Lưu thông tin Credentials

**QUAN TRỌNG**: Sao chép và lưu lại:
- **Client ID**: Dãy ký tự dài bắt đầu bằng số
- **Client Secret**: Dãy ký tự dài (giữ bí mật, không chia sẻ công khai)

---

## Bước 6: Cấu hình trong ứng dụng Unicareer

### 6.1. Cập nhật appsettings.json

Mở file `appsettings.json` và thay thế:

```json
{
  "Authentication": {
    "Google": {
      "ClientId": "YOUR_GOOGLE_CLIENT_ID",
      "ClientSecret": "YOUR_GOOGLE_CLIENT_SECRET"
    }
  }
}
```

Bằng:

```json
{
  "Authentication": {
    "Google": {
      "ClientId": "YOUR_GOOGLE_CLIENT_ID",
      "ClientSecret": "YOUR_GOOGLE_CLIENT_SECRET"
    }
  }
}
```

**Thay thế bằng giá trị thực từ Google Cloud Console của bạn.**

### 6.2. Cập nhật appsettings.Development.json (nếu cần)

Nếu bạn muốn cấu hình riêng cho môi trường development:

```json
{
  "Authentication": {
    "Google": {
      "ClientId": "YOUR_DEV_CLIENT_ID",
      "ClientSecret": "YOUR_DEV_CLIENT_SECRET"
    }
  }
}
```

---

## Bước 7: Kiểm tra cấu hình

1. **Kiểm tra Program.cs**: Đảm bảo đã có cấu hình Google Authentication
2. **Kiểm tra Redirect URI**: Đảm bảo URI trong Google Cloud Console khớp với URL ứng dụng của bạn
3. **Chạy ứng dụng**: 
   ```bash
   dotnet run
   ```
4. **Test đăng nhập Google**: 
   - Vào trang Login hoặc Register
   - Click nút "Đăng nhập với Google" hoặc "Đăng ký với Google"
   - Đăng nhập bằng tài khoản Google
   - Kiểm tra xem có redirect về ứng dụng thành công không

---

## Xử lý lỗi thường gặp

### Lỗi: "redirect_uri_mismatch"

**Nguyên nhân**: URI redirect trong Google Cloud Console không khớp với URI thực tế.

**Giải pháp**:
1. Kiểm tra URL hiện tại của ứng dụng (xem trong browser address bar)
2. Vào Google Cloud Console > Credentials
3. Chỉnh sửa OAuth Client ID
4. Thêm đúng URI redirect (bao gồm cả port nếu là localhost)

### Lỗi: "access_denied"

**Nguyên nhân**: OAuth consent screen chưa được cấu hình đúng.

**Giải pháp**:
1. Vào OAuth consent screen
2. Đảm bảo đã điền đầy đủ thông tin
3. Nếu ở chế độ Testing, thêm email test user vào danh sách

### Lỗi: "invalid_client"

**Nguyên nhân**: Client ID hoặc Client Secret không đúng.

**Giải pháp**:
1. Kiểm tra lại Client ID và Client Secret trong appsettings.json
2. Đảm bảo không có khoảng trắng thừa
3. Sao chép lại từ Google Cloud Console

---

## Lưu ý bảo mật

1. **KHÔNG commit** file `appsettings.json` có chứa Client Secret thực vào Git
2. Sử dụng **User Secrets** cho development:
   ```bash
   dotnet user-secrets set "Authentication:Google:ClientId" "YOUR_CLIENT_ID"
   dotnet user-secrets set "Authentication:Google:ClientSecret" "YOUR_CLIENT_SECRET"
   ```
3. Sử dụng **Environment Variables** hoặc **Azure Key Vault** cho production
4. **Client Secret** phải được giữ bí mật, không chia sẻ công khai

---

## Sử dụng User Secrets (Khuyến nghị cho Development)

### Cài đặt User Secrets:

```bash
dotnet user-secrets init
```

### Thêm credentials:

```bash
dotnet user-secrets set "Authentication:Google:ClientId" "YOUR_CLIENT_ID"
dotnet user-secrets set "Authentication:Google:ClientSecret" "YOUR_CLIENT_SECRET"
```

### Xem lại secrets:

```bash
dotnet user-secrets list
```

---

## Tài liệu tham khảo

- [Google Cloud Console](https://console.cloud.google.com/)
- [Google OAuth 2.0 Documentation](https://developers.google.com/identity/protocols/oauth2)
- [ASP.NET Core External Authentication](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/social/)

---

## Checklist hoàn thành

- [ ] Đã tạo Google Cloud Project
- [ ] Đã kích hoạt Google+ API hoặc People API
- [ ] Đã cấu hình OAuth Consent Screen
- [ ] Đã tạo OAuth 2.0 Client ID
- [ ] Đã thêm Authorized Redirect URIs
- [ ] Đã lưu Client ID và Client Secret
- [ ] Đã cập nhật appsettings.json hoặc User Secrets
- [ ] Đã test đăng nhập/đăng ký bằng Google thành công

---

**Chúc bạn thành công!** 🎉

