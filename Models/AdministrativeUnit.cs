using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Unicareer.Models
{
    [Table("administrative_units")]
    public class AdministrativeUnit
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [MaxLength(255)]
        [Column("full_name")]
        public string? FullName { get; set; }

        [MaxLength(255)]
        [Column("full_name_en")]
        public string? FullNameEn { get; set; }

        [MaxLength(255)]
        [Column("short_name")]
        public string? ShortName { get; set; }

        [MaxLength(255)]
        [Column("short_name_en")]
        public string? ShortNameEn { get; set; }

        [MaxLength(255)]
        [Column("code_name")]
        public string? CodeName { get; set; }

        [MaxLength(255)]
        [Column("code_name_en")]
        public string? CodeNameEn { get; set; }

        // Navigation properties
        public virtual ICollection<Province> Provinces { get; set; } = new List<Province>();
        public virtual ICollection<Ward> Wards { get; set; } = new List<Ward>();
    }
}

