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
        public DbSet<LoaiCongViec> LoaiCongViecs { get; set; }
        public DbSet<NganhNghe> NganhNghes { get; set; }
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

            // Cấu hình precision cho decimal fields
            builder.Entity<TinTuyenDung>()
                .Property(t => t.MucLuongThapNhat)
                .HasPrecision(18, 2);

            builder.Entity<TinTuyenDung>()
                .Property(t => t.MucLuongCaoNhat)
                .HasPrecision(18, 2);

            // Cấu hình primary key cho TinUngTuyen
            builder.Entity<TinUngTuyen>()
                .HasKey(t => t.MaTinUngTuyen);

            builder.Entity<TinUngTuyen>()
                .Property(t => t.MaTinUngTuyen)
                .ValueGeneratedOnAdd();

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

            // Cấu hình primary key cho TruongDaiHoc
            builder.Entity<TruongDaiHoc>()
                .HasKey(t => t.MaTruong);

            builder.Entity<TruongDaiHoc>()
                .Property(t => t.MaTruong)
                .ValueGeneratedOnAdd();
        }
    }
}

