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
        }
    }
}

