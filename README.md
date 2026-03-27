# Unicareer

Nền tảng web kết nối **ứng viên** và **nhà tuyển dụng**, hỗ trợ đăng tin tuyển dụng, ứng tuyển, quản lý hồ sơ và phân quyền theo vai trò trên cùng một hệ thống.

## Tổng quan

Unicareer được phát triển bằng ASP.NET Core MVC theo hướng tách lớp rõ ràng (Controller - Service - Repository - Data), tập trung vào:

- Trải nghiệm người dùng cho 3 nhóm vai trò: `Admin`, `NhaTuyenDung`, `UngVien`
- Quản trị dữ liệu tuyển dụng và luồng ứng tuyển end-to-end
- Bảo mật xác thực, phân quyền, và các tác vụ nền tự động

## Tính năng chính

### 1) Xác thực và phân quyền

- Đăng ký/đăng nhập bằng tài khoản nội bộ (Identity)
- Hỗ trợ đăng nhập Google OAuth
- Role-based authorization với 3 vai trò:
  - `Admin`: quản trị hệ thống và người dùng
  - `NhaTuyenDung`: quản lý hồ sơ công ty, đăng tin, xử lý ứng viên
  - `UngVien`: quản lý hồ sơ cá nhân, ứng tuyển, theo dõi trạng thái
- Seed role và tài khoản admin mặc định khi khởi động ứng dụng

### 2) Dành cho nhà tuyển dụng

- Tạo, cập nhật, quản lý tin tuyển dụng
- Theo dõi số lượng ứng tuyển và thống kê dashboard
- Xử lý hồ sơ ứng viên theo trạng thái tuyển dụng

### 3) Dành cho ứng viên

- Quản lý hồ sơ ứng viên và trạng thái tìm việc
- Ứng tuyển, theo dõi tiến trình xử lý hồ sơ
- Lưu việc làm quan tâm và xem thông tin công khai ứng viên (có kiểm soát)

### 4) Tự động hóa và tích hợp

- Background service cập nhật tin tuyển dụng hết hạn theo lịch
- Gửi email thông báo qua SMTP
- Tích hợp nguồn bài viết bên ngoài qua HTTP client

## Công nghệ sử dụng

- **Backend Framework:** ASP.NET Core MVC (.NET 8)
- **Database:** SQL Server + Entity Framework Core
- **Authentication/Authorization:** ASP.NET Core Identity + Google OAuth
- **Mapping:** AutoMapper
- **Architecture Style:** Areas + Repository pattern + Services + DI
- **Hosting-ready:** cấu hình HTTPS, cookie policy, session

## Kiến trúc hệ thống (rút gọn)

```text
Client (Browser)
    -> Controllers (Areas: Admin / Recruiter / Candidate)
        -> Services (Email, ExternalArticle, Background Jobs)
            -> Repositories
                -> EF Core DbContext
                    -> SQL Server
```

## Hướng dẫn cài đặt và chạy local

### 1) Yêu cầu môi trường

- .NET SDK 8.0+
- SQL Server (SQL Server Express/Developer đều được)
- IDE khuyến nghị: Visual Studio 2022 hoặc VS Code/Cursor

### 2) Cấu hình ứng dụng

1. Mở file `appsettings.json`
2. Cập nhật các giá trị:
   - `ConnectionStrings:DefaultConnection`
   - `Authentication:Google` (nếu dùng đăng nhập Google)
   - `Email` (nếu dùng gửi email SMTP)

Lưu ý:
- `appsettings.Development.json` đã được `.gitignore`, nên chỉ dùng cho máy local.
- Không commit secret (client secret, SMTP password) lên git.

### 3) Khởi tạo database

Chạy migration EF Core (nếu đã có migration trong project):

```bash
dotnet ef database update
```

Nếu bạn đang thêm migration mới:

```bash
dotnet ef migrations add <MigrationName>
dotnet ef database update
```

Ngoài ra, project có các script SQL bổ sung trong thư mục `Migrations/` để cập nhật dữ liệu lịch sử khi cần.

### 4) Chạy ứng dụng

```bash
dotnet restore
dotnet run
```

Sau khi chạy, truy cập theo URL được in ra trong console (thường là localhost với HTTPS).

## Phân quyền và tài khoản mẫu

- Hệ thống tự seed các role: `Admin`, `NhaTuyenDung`, `UngVien`
- Có cơ chế seed tài khoản admin mặc định khi khởi động lần đầu
- Luồng điều hướng sau đăng nhập phụ thuộc role người dùng

Gợi ý: với môi trường CV/portfolio, bạn nên đổi mật khẩu admin mặc định ngay sau khi khởi tạo.

## Điểm nhấn kỹ thuật cho CV

- Thiết kế nhiều `Area` tách biệt theo vai trò để quản lý module rõ ràng
- Áp dụng `Repository pattern` và `Dependency Injection` cho tính mở rộng
- Xử lý nghiệp vụ bất đồng bộ bằng `Hosted Background Service`
- Kết hợp bảo mật thực tế: cookie policy, session, logging cho luồng đăng nhập nhạy cảm
- Có chiến lược cập nhật dữ liệu chuyển đổi qua migration + SQL scripts

## Hướng phát triển

- Bổ sung test tự động (unit/integration) cho các service/repository chính
- Nâng cấp observability (structured logging, dashboard theo dõi)
- Tối ưu CI/CD và đóng gói container
- Mở rộng tìm kiếm việc làm theo gợi ý cá nhân hóa

## Liên hệ

Nếu bạn là nhà tuyển dụng hoặc reviewer kỹ thuật, có thể liên hệ với tác giả dự án qua:

- Email: `your-email@example.com`
- LinkedIn: `https://www.linkedin.com/in/your-profile`

---

Unicareer được xây dựng với mục tiêu giải bài toán tuyển dụng thực tế, đồng thời thể hiện năng lực phát triển ứng dụng web full-stack trên nền tảng .NET.
