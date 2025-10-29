# 🔐 Hệ thống Phân quyền Role-based - Unicareer

## ✨ Đã tích hợp Class SD

Hệ thống đã được cập nhật để sử dụng **Role-based Authorization** với class `SD.cs`:

```csharp
public static class SD
{
    public const string Role_Admin = "Admin";
    public const string Role_NhaTuyenDung = "NhaTuyenDung";
    public const string Role_UngVien = "UngVien";
}
```

---

## 📋 Những gì đã thay đổi

### 1. **AccountController** ✅
- Thêm `RoleManager<IdentityRole>` để quản lý roles
- **Khi đăng ký**: Tự động gán role cho user dựa trên loại tài khoản
  - Chọn "Nhà tuyển dụng" → Gán role `SD.Role_NhaTuyenDung`
  - Chọn "Ứng viên" → Gán role `SD.Role_UngVien`
- **Khi đăng nhập**: Kiểm tra role và chuyển hướng phù hợp
  - Admin → `/Admin/Admin/Index`
  - Nhà tuyển dụng → `/Recruiter/Recruiter/Index`
  - Ứng viên → `/Home/Index`

### 2. **DbInitializer.cs** (Mới) ✅
Tự động tạo roles và tài khoản admin khi ứng dụng khởi động:

**Roles được tạo:**
- `Admin`
- `NhaTuyenDung`
- `UngVien`

**Tài khoản Admin mặc định:**
- Email: `admin@unicareer.vn`
- Password: `Admin@123`
- Role: `Admin`

### 3. **Program.cs** ✅
Thêm logic seed roles và admin user khi ứng dụng khởi động

### 4. **AdminController** ✅
```csharp
[Area("Admin")]
[Authorize(Roles = $"{SD.Role_Admin}")]
public class AdminController : Controller
```
**Chỉ Admin mới truy cập được**

### 5. **RecruiterController** ✅
```csharp
[Area("Recruiter")]
[Authorize(Roles = $"{SD.Role_NhaTuyenDung}")]
public class RecruiterController : Controller
```
**Chỉ Nhà tuyển dụng mới truy cập được**

### 6. **Layout (_Layout.cshtml)** ✅
Sử dụng `User.IsInRole()` thay vì kiểm tra `LoaiTaiKhoan`:
```csharp
@if (User.IsInRole(Unicareer.Models.SD.Role_Admin))
{
    // Hiển thị menu Admin
}
else if (User.IsInRole(Unicareer.Models.SD.Role_NhaTuyenDung))
{
    // Hiển thị menu Nhà tuyển dụng
}
```

### 7. **Register.cshtml** ✅
Sử dụng `SD.Role_*` cho dropdown:
```html
<option value="@Unicareer.Models.SD.Role_UngVien">Ứng viên tìm việc</option>
<option value="@Unicareer.Models.SD.Role_NhaTuyenDung">Nhà tuyển dụng</option>
```

---

## 🚀 Cách sử dụng

### Bước 1: Chạy Migration
```bash
dotnet ef migrations add AddIdentityRoles
dotnet ef database update
```

### Bước 2: Chạy ứng dụng
```bash
dotnet run
```

### Bước 3: Đăng nhập Admin
Khi ứng dụng chạy lần đầu, hệ thống tự động tạo:
- ✅ 3 roles (Admin, NhaTuyenDung, UngVien)
- ✅ Tài khoản admin với:
  - Email: `admin@unicareer.vn`
  - Password: `Admin@123`

### Bước 4: Đăng ký tài khoản mới
1. Truy cập `/Account/Register`
2. Chọn loại tài khoản
3. Điền thông tin và đăng ký
4. Hệ thống tự động gán role phù hợp

---

## 🔒 Phân quyền

| Role | Quyền truy cập |
|------|----------------|
| **Admin** | Toàn bộ trang quản trị (`/Admin/*`) |
| **NhaTuyenDung** | Trang quản lý tuyển dụng (`/Recruiter/*`) |
| **UngVien** | Trang chủ, tìm việc, ứng tuyển |

---

## 🎯 Luồng hoạt động

### Đăng ký Nhà tuyển dụng:
1. User chọn "Nhà tuyển dụng" khi đăng ký
2. Hệ thống tạo user và gán role `NhaTuyenDung`
3. Tự động đăng nhập và chuyển đến `/Recruiter/Recruiter/Index`

### Đăng ký Ứng viên:
1. User chọn "Ứng viên" khi đăng ký
2. Hệ thống tạo user và gán role `UngVien`
3. Tự động đăng nhập và chuyển đến trang chủ

### Đăng nhập:
1. User nhập email/password
2. Hệ thống kiểm tra role:
   - **Admin** → Dashboard Admin
   - **NhaTuyenDung** → Dashboard Recruiter
   - **UngVien** → Trang chủ

---

## 📊 Database Tables

Sau khi chạy migration, các bảng Identity sẽ được tạo:
- `AspNetUsers` - Thông tin người dùng
- `AspNetRoles` - Danh sách roles
- `AspNetUserRoles` - Quan hệ User-Role
- `AspNetUserClaims`, `AspNetUserLogins`, etc.

---

## 🛡️ Bảo mật

### Tại Controller:
```csharp
[Authorize(Roles = SD.Role_Admin)]  // Chỉ Admin
[Authorize(Roles = SD.Role_NhaTuyenDung)]  // Chỉ Nhà tuyển dụng
[Authorize(Roles = $"{SD.Role_Admin},{SD.Role_NhaTuyenDung}")]  // Admin hoặc NTD
```

### Tại View:
```csharp
@if (User.IsInRole(SD.Role_Admin))
{
    // Nội dung chỉ Admin thấy
}
```

---

## 💡 Mở rộng

### Thêm role mới:
1. Thêm constant vào `SD.cs`:
```csharp
public const string Role_Moderator = "Moderator";
```

2. Thêm vào `DbInitializer.cs`:
```csharp
if (!await roleManager.RoleExistsAsync(SD.Role_Moderator))
{
    await roleManager.CreateAsync(new IdentityRole(SD.Role_Moderator));
}
```

### Gán nhiều role cho user:
```csharp
await _userManager.AddToRoleAsync(user, SD.Role_Admin);
await _userManager.AddToRoleAsync(user, SD.Role_NhaTuyenDung);
```

---

## ✅ Checklist

- [x] Tạo class SD với các role constants
- [x] Cập nhật AccountController để gán role khi đăng ký
- [x] Tạo DbInitializer để seed roles và admin
- [x] Thêm authorization cho AdminController
- [x] Thêm authorization cho RecruiterController
- [x] Cập nhật Layout để sử dụng `User.IsInRole()`
- [x] Cập nhật Register view để sử dụng SD constants

---

## 🎉 Hoàn thành!

Hệ thống bây giờ đã sử dụng **Role-based Authorization** đầy đủ với class `SD` của bạn!

