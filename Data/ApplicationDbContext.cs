using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Unicareer.Models;

namespace Unicareer.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets cho địa chỉ Việt Nam
        public DbSet<AdministrativeRegion> AdministrativeRegions { get; set; }
        public DbSet<AdministrativeUnit> AdministrativeUnits { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<Ward> Wards { get; set; }

        // DbSets cho các entity chính
        public DbSet<UngVien> UngViens { get; set; }
        public DbSet<NhaTuyenDung> NhaTuyenDungs { get; set; }
        public DbSet<TinTuyenDung> TinTuyenDungs { get; set; }
        public DbSet<TinUngTuyen> TinUngTuyens { get; set; }
        public DbSet<ViecLamDaLuu> ViecLamDaLuus { get; set; }
        public DbSet<LoaiCongViec> LoaiCongViecs { get; set; }
        public DbSet<NganhNghe> NganhNghes { get; set; }
        public DbSet<ChuyenNganh> ChuyenNganhs { get; set; }
        public DbSet<TruongDaiHoc> TruongDaiHocs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            // Tắt IDENTITY cho các cột id (cho phép insert giá trị cụ thể)
            builder.Entity<AdministrativeRegion>()
                .Property(ar => ar.Id)
                .ValueGeneratedNever();

            builder.Entity<AdministrativeUnit>()
                .Property(au => au.Id)
                .ValueGeneratedNever();
            
            // Cấu hình quan hệ cho địa chỉ Việt Nam
            // Province -> AdministrativeUnit
            builder.Entity<Province>()
                .HasOne(p => p.AdministrativeUnit)
                .WithMany(au => au.Provinces)
                .HasForeignKey(p => p.AdministrativeUnitId)
                .OnDelete(DeleteBehavior.SetNull);

            // Ward -> Province
            builder.Entity<Ward>()
                .HasOne(w => w.Province)
                .WithMany(p => p.Wards)
                .HasForeignKey(w => w.ProvinceCode)
                .OnDelete(DeleteBehavior.SetNull);

            // Ward -> AdministrativeUnit
            builder.Entity<Ward>()
                .HasOne(w => w.AdministrativeUnit)
                .WithMany(au => au.Wards)
                .HasForeignKey(w => w.AdministrativeUnitId)
                .OnDelete(DeleteBehavior.SetNull);

            // Tạo index cho các foreign keys
            builder.Entity<Province>()
                .HasIndex(p => p.AdministrativeUnitId)
                .HasDatabaseName("idx_provinces_unit");

            builder.Entity<Ward>()
                .HasIndex(w => w.ProvinceCode)
                .HasDatabaseName("idx_wards_province");

            builder.Entity<Ward>()
                .HasIndex(w => w.AdministrativeUnitId)
                .HasDatabaseName("idx_wards_unit");

            // Cấu hình primary key và quan hệ cho UngVien
            builder.Entity<UngVien>()
                .HasKey(u => u.MaUngVien);

            builder.Entity<UngVien>()
                .Property(u => u.MaUngVien)
                .ValueGeneratedOnAdd();

            builder.Entity<UngVien>()
                .HasOne(u => u.User)
                .WithMany()
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UngVien>()
                .HasIndex(u => u.UserId)
                .IsUnique()
                .HasDatabaseName("idx_ungvien_userid");

            // Cấu hình primary key và quan hệ cho NhaTuyenDung
            builder.Entity<NhaTuyenDung>()
                .HasKey(n => n.MaNhaTuyenDung);

            builder.Entity<NhaTuyenDung>()
                .Property(n => n.MaNhaTuyenDung)
                .ValueGeneratedOnAdd();

            builder.Entity<NhaTuyenDung>()
                .HasOne(n => n.User)
                .WithMany()
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<NhaTuyenDung>()
                .HasIndex(n => n.UserId)
                .IsUnique()
                .HasDatabaseName("idx_nhatuyendung_userid");

            // Cấu hình primary key cho TinTuyenDung
            builder.Entity<TinTuyenDung>()
                .HasKey(t => t.MaTinTuyenDung);

            builder.Entity<TinTuyenDung>()
                .Property(t => t.MaTinTuyenDung)
                .ValueGeneratedOnAdd();

            // Cấu hình quan hệ TinTuyenDung -> NhaTuyenDung
            builder.Entity<TinTuyenDung>()
                .HasOne(t => t.NhaTuyenDung)
                .WithMany()
                .HasForeignKey(t => t.MaNhaTuyenDung)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<TinTuyenDung>()
                .HasIndex(t => t.MaNhaTuyenDung)
                .HasDatabaseName("idx_tintuyendung_nhatuyendung");

            // Cấu hình precision cho decimal fields
            builder.Entity<TinTuyenDung>()
                .Property(t => t.MucLuongThapNhat)
                .HasPrecision(18, 2);

            builder.Entity<TinTuyenDung>()
                .Property(t => t.MucLuongCaoNhat)
                .HasPrecision(18, 2);

            // Cấu hình precision cho UngVien decimal fields
            builder.Entity<UngVien>()
                .Property(u => u.MucLuongKyVong)
                .HasPrecision(18, 2);

            // Cấu hình primary key cho TinUngTuyen
            builder.Entity<TinUngTuyen>()
                .HasKey(t => t.MaTinUngTuyen);

            builder.Entity<TinUngTuyen>()
                .Property(t => t.MaTinUngTuyen)
                .ValueGeneratedOnAdd();

            // Cấu hình quan hệ TinUngTuyen -> ApplicationUser
            builder.Entity<TinUngTuyen>()
                .HasOne(t => t.User)
                .WithMany()
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            // Cấu hình primary key cho LoaiCongViec
            builder.Entity<LoaiCongViec>()
                .HasKey(l => l.MaLoaiCongViec);

            builder.Entity<LoaiCongViec>()
                .Property(l => l.MaLoaiCongViec)
                .ValueGeneratedOnAdd();

            // Cấu hình primary key cho NganhNghe
            builder.Entity<NganhNghe>()
                .HasKey(n => n.MaNganhNghe);

            builder.Entity<NganhNghe>()
                .Property(n => n.MaNganhNghe)
                .ValueGeneratedOnAdd();

            // Cấu hình primary key và quan hệ cho ChuyenNganh
            builder.Entity<ChuyenNganh>()
                .HasKey(c => c.MaChuyenNganh);

            builder.Entity<ChuyenNganh>()
                .Property(c => c.MaChuyenNganh)
                .ValueGeneratedOnAdd();

            // ChuyenNganh -> NganhNghe
            builder.Entity<ChuyenNganh>()
                .HasOne(c => c.NganhNghe)
                .WithMany()
                .HasForeignKey(c => c.MaNganhNghe)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<ChuyenNganh>()
                .HasIndex(c => c.MaNganhNghe)
                .HasDatabaseName("idx_chuyennganh_nganhnghe");

            // UngVien -> ChuyenNganh
            builder.Entity<UngVien>()
                .HasOne(u => u.ChuyenNganh)
                .WithMany()
                .HasForeignKey(u => u.MaChuyenNganh)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<UngVien>()
                .HasIndex(u => u.MaChuyenNganh)
                .HasDatabaseName("idx_ungvien_chuyennganh");

            // Cấu hình primary key cho TruongDaiHoc
            builder.Entity<TruongDaiHoc>()
                .HasKey(t => t.MaTruong);

            builder.Entity<TruongDaiHoc>()
                .Property(t => t.MaTruong)
                .ValueGeneratedOnAdd();

            // Cấu hình primary key và quan hệ cho ViecLamDaLuu
            builder.Entity<ViecLamDaLuu>()
                .HasKey(v => v.MaViecLamDaLuu);

            builder.Entity<ViecLamDaLuu>()
                .Property(v => v.MaViecLamDaLuu)
                .ValueGeneratedOnAdd();

            // ViecLamDaLuu -> ApplicationUser
            builder.Entity<ViecLamDaLuu>()
                .HasOne(v => v.User)
                .WithMany()
                .HasForeignKey(v => v.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // ViecLamDaLuu -> TinTuyenDung
            builder.Entity<ViecLamDaLuu>()
                .HasOne(v => v.TinTuyenDung)
                .WithMany()
                .HasForeignKey(v => v.MaTinTuyenDung)
                .OnDelete(DeleteBehavior.Cascade);

            // Tạo index và unique constraint để tránh lưu trùng
            builder.Entity<ViecLamDaLuu>()
                .HasIndex(v => new { v.UserId, v.MaTinTuyenDung })
                .IsUnique()
                .HasDatabaseName("idx_vieclamdaluu_userid_matintuyendung");

            builder.Entity<ViecLamDaLuu>()
                .HasIndex(v => v.UserId)
                .HasDatabaseName("idx_vieclamdaluu_userid");

            builder.Entity<ViecLamDaLuu>()
                .HasIndex(v => v.MaTinTuyenDung)
                .HasDatabaseName("idx_vieclamdaluu_matintuyendung");
        }
    }
}

