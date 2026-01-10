# Hướng Dẫn Sử Dụng Hệ Thống Mock Data

## 📋 Mục Đích

Hệ thống Mock Data cho phép Frontend và Backend làm việc độc lập với nhau:
- **Frontend** có thể phát triển và test mà không cần đợi Backend hoàn thiện
- **Backend** có thể phát triển mà không ảnh hưởng đến Frontend
- Dễ dàng chuyển đổi giữa Mock Data và Real Data chỉ bằng một cấu hình

## 🎯 Cách Hoạt Động

Hệ thống sử dụng **Repository Pattern** với khả năng chuyển đổi tự động:
- Khi `UseMockData = true`: Sử dụng các Mock Repository (dữ liệu giả, không cần database)
- Khi `UseMockData = false`: Sử dụng các Real Repository (kết nối database thật)

## ⚙️ Cấu Hình

### 1. File Cấu Hình

Hệ thống sử dụng 2 file cấu hình chính:

#### `appsettings.json` (Production)
```json
{
  "Repository": {
    "UseMockData": false
  }
}
```

#### `appsettings.Development.json` (Development)
```json
{
  "Repository": {
    "UseMockData": true
  }
}
```

### 2. Các Repository Được Hỗ Trợ

Hệ thống tự động chuyển đổi các repository sau:

| Repository | Mock Class | Real Class |
|------------|------------|------------|
| `INhaTuyenDungRepository` | `MockNhaTuyenDungRepository` | `NhaTuyenDungRepository` |
| `IUngVienRepository` | `MockUngVienRepository` | `UngVienRepository` |
| `ITinTuyenDungRepository` | `MockTinTuyenDungRepository` | `TinTuyenDungRepository` |
| `ITinUngTuyenRepository` | `MockTinUngTuyenRepository` | `TinUngTuyenRepository` |
| `ILoaiCongViecRepository` | `MockLoaiCongViecRepository` | `LoaiCongViecRepository` |
| `INganhNgheRepository` | `MockNganhNgheRepository` | `NganhNgheRepository` |
| `ITruongDaiHocRepository` | `MockTruongDaiHocRepository` | `TruongDaiHocRepository` |

## 🚀 Cách Sử Dụng

### Kịch Bản 1: Phát Triển Frontend (Sử Dụng Mock Data)

**Khi nào sử dụng:**
- Frontend đang phát triển UI/UX
- Backend chưa sẵn sàng hoặc đang bảo trì
- Cần test các tính năng mà không cần database
- Demo nhanh cho khách hàng

**Cách làm:**
1. Mở file `appsettings.Development.json`
2. Đặt `UseMockData = true`:
```json
{
  "Repository": {
    "UseMockData": true
  }
}
```
3. Chạy ứng dụng - hệ thống sẽ tự động sử dụng Mock Data
4. Không cần kết nối database, không cần migrate

**Lợi ích:**
- ✅ Làm việc nhanh, không cần đợi Backend
- ✅ Không cần setup database
- ✅ Dữ liệu luôn có sẵn và nhất quán
- ✅ Dễ dàng test các edge cases

### Kịch Bản 2: Phát Triển Backend (Sử Dụng Real Data)

**Khi nào sử dụng:**
- Backend đang phát triển tính năng mới
- Cần test với dữ liệu thật từ database
- Cần kiểm tra performance với database
- Chuẩn bị deploy lên production

**Cách làm:**
1. Mở file `appsettings.Development.json` hoặc `appsettings.json`
2. Đặt `UseMockData = false`:
```json
{
  "Repository": {
    "UseMockData": false
  }
}
```
3. Đảm bảo database đã được setup và migrate
4. Chạy ứng dụng - hệ thống sẽ kết nối database thật

**Lợi ích:**
- ✅ Test với dữ liệu thật
- ✅ Kiểm tra tính năng với database
- ✅ Phát hiện lỗi sớm trước khi deploy

### Kịch Bản 3: Production (Luôn Dùng Real Data)

**Lưu ý quan trọng:**
- Trong môi trường Production, **LUÔN** sử dụng `UseMockData = false`
- File `appsettings.json` mặc định đã được cấu hình đúng

```json
{
  "Repository": {
    "UseMockData": false
  }
}
```

## 📊 Kiểm Tra Trạng Thái

Khi khởi động ứng dụng, hệ thống sẽ tự động log trạng thái:

```
=== REPOSITORY CONFIGURATION ===
UseMockData: True
Repository Mode: MOCK DATA
================================
```

hoặc

```
=== REPOSITORY CONFIGURATION ===
UseMockData: False
Repository Mode: REAL DATABASE
================================
```

Kiểm tra console/log để xác nhận đang sử dụng loại repository nào.

## 🔧 Cấu Trúc Code

### Extension Method

Hệ thống sử dụng Extension Method trong `Repository/RepositoryServiceExtensions.cs`:

```csharp
public static void AddRepositories(this IServiceCollection services, IConfiguration configuration)
{
    var useMockData = configuration.GetValue<bool>("Repository:UseMockData", false);
    
    if (useMockData)
    {
        // Đăng ký Mock Repositories
    }
    else
    {
        // Đăng ký Real Repositories
    }
}
```

### Đăng Ký Trong Program.cs

```csharp
// Tự động đăng ký repository dựa trên cấu hình
builder.Services.AddRepositories(builder.Configuration);
```

## ⚠️ Lưu Ý Quan Trọng

### 1. Mock Data vs Real Data

| Đặc điểm | Mock Data | Real Data |
|----------|-----------|-----------|
| Database | Không cần | Cần kết nối |
| Dữ liệu | Cố định, giả lập | Từ database thật |
| Performance | Nhanh | Phụ thuộc database |
| Thay đổi dữ liệu | Không lưu lại | Lưu vào database |
| Phù hợp | Development, Testing | Production, Integration Testing |

### 2. Khi Nào Nên Chuyển Đổi

**Chuyển sang Mock Data khi:**
- Frontend đang phát triển UI
- Database chưa sẵn sàng
- Cần demo nhanh
- Test các tính năng không phụ thuộc database

**Chuyển sang Real Data khi:**
- Cần test với dữ liệu thật
- Backend đang phát triển
- Chuẩn bị deploy
- Integration testing

### 3. Best Practices

1. **Development Environment:**
   - Mặc định sử dụng Mock Data (`UseMockData = true`)
   - Chuyển sang Real Data khi cần test integration

2. **Production Environment:**
   - **LUÔN** sử dụng Real Data (`UseMockData = false`)
   - Không bao giờ deploy với Mock Data

3. **Testing:**
   - Unit tests: Sử dụng Mock Data
   - Integration tests: Sử dụng Real Data (hoặc test database)

4. **Team Collaboration:**
   - Frontend team: Sử dụng Mock Data
   - Backend team: Sử dụng Real Data
   - Full-stack developer: Chuyển đổi linh hoạt

## 🐛 Troubleshooting

### Vấn đề: Không thấy log về Repository Configuration

**Giải pháp:**
- Kiểm tra xem logging đã được bật chưa
- Xem console output khi khởi động ứng dụng

### Vấn đề: Vẫn kết nối database khi dùng Mock Data

**Giải pháp:**
- Kiểm tra lại file `appsettings.Development.json`
- Đảm bảo `UseMockData = true`
- Restart ứng dụng sau khi thay đổi config

### Vấn đề: Lỗi khi chuyển sang Real Data

**Giải pháp:**
- Kiểm tra connection string trong `appsettings.json`
- Đảm bảo database đã được tạo và migrate
- Kiểm tra SQL Server đang chạy

### Vấn đề: Dữ liệu không thay đổi khi dùng Mock Data

**Giải pháp:**
- Đây là hành vi bình thường của Mock Data
- Mock Data là dữ liệu cố định, không lưu thay đổi
- Chuyển sang Real Data nếu cần lưu thay đổi

## 📝 Ví Dụ Thực Tế

### Ví Dụ 1: Frontend Developer

```json
// appsettings.Development.json
{
  "Repository": {
    "UseMockData": true  // ← Frontend dev không cần database
  }
}
```

**Kết quả:**
- Có thể phát triển UI ngay lập tức
- Không cần đợi Backend API
- Dữ liệu luôn có sẵn để test

### Ví Dụ 2: Backend Developer

```json
// appsettings.Development.json
{
  "Repository": {
    "UseMockData": false  // ← Backend dev cần test với database
  }
}
```

**Kết quả:**
- Test với dữ liệu thật
- Kiểm tra performance
- Phát hiện lỗi sớm

### Ví Dụ 3: Full-Stack Developer

Có thể chuyển đổi linh hoạt:
- Sáng: `UseMockData = true` để phát triển UI
- Chiều: `UseMockData = false` để test Backend

## 🔄 Quy Trình Phát Triển Đề Xuất

1. **Giai đoạn đầu (Frontend focus):**
   - `UseMockData = true`
   - Frontend phát triển UI/UX
   - Backend thiết kế API

2. **Giai đoạn giữa (Integration):**
   - `UseMockData = false`
   - Kết nối Frontend với Backend
   - Test integration

3. **Giai đoạn cuối (Production):**
   - `UseMockData = false`
   - Test toàn bộ hệ thống
   - Deploy

## 📚 Tài Liệu Tham Khảo

- File cấu hình: `appsettings.json`, `appsettings.Development.json`
- Extension method: `Repository/RepositoryServiceExtensions.cs`
- Đăng ký services: `Program.cs`
- Mock repositories: `Repository/Mock*.cs`
- Real repositories: `Repository/*Repository.cs`

## ❓ Câu Hỏi Thường Gặp (FAQ)

**Q: Có thể sử dụng cả Mock và Real Data cùng lúc không?**
A: Không, hệ thống chỉ sử dụng một loại tại một thời điểm. Bạn phải chọn một trong hai.

**Q: Mock Data có giống với dữ liệu thật không?**
A: Mock Data là dữ liệu giả lập, có cấu trúc giống nhưng giá trị cố định. Không phản ánh dữ liệu thật trong database.

**Q: Làm sao để thêm dữ liệu mới vào Mock Data?**
A: Chỉnh sửa trực tiếp trong các file `Mock*Repository.cs` trong thư mục `Repository/`.

**Q: Có cần restart ứng dụng sau khi thay đổi config không?**
A: Có, bạn cần restart ứng dụng để thay đổi có hiệu lực.

**Q: Production có thể dùng Mock Data không?**
A: **KHÔNG BAO GIỜ!** Production phải luôn sử dụng Real Data.

---

**Tác giả:** Hệ thống Unicareer  
**Cập nhật:** 2024  
**Phiên bản:** 1.0

