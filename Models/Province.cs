using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Unicareer.Models
{
    [Table("provinces")]
    public class Province
    {
        [Key]
        [MaxLength(20)]
        [Column("code")]
        public string Code { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(255)]
        [Column("name_en")]
        public string? NameEn { get; set; }

        [Required]
        [MaxLength(255)]
        [Column("full_name")]
        public string FullName { get; set; } = string.Empty;

        [MaxLength(255)]
        [Column("full_name_en")]
        public string? FullNameEn { get; set; }

        [MaxLength(255)]
        [Column("code_name")]
        public string? CodeName { get; set; }

        [Column("administrative_unit_id")]
        public int? AdministrativeUnitId { get; set; }

        [MaxLength(500)]
        [Column("img")]
        public string? Image { get; set; }

        // Navigation properties
        [ForeignKey("AdministrativeUnitId")]
        public virtual AdministrativeUnit? AdministrativeUnit { get; set; }

        public virtual ICollection<Ward> Wards { get; set; } = new List<Ward>();
    }
}

