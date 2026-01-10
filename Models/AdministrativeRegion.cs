using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Unicareer.Models
{
    [Table("administrative_regions")]
    public class AdministrativeRegion
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        [Column("name_en")]
        public string NameEn { get; set; } = string.Empty;

        [MaxLength(255)]
        [Column("code_name")]
        public string? CodeName { get; set; }

        [MaxLength(255)]
        [Column("code_name_en")]
        public string? CodeNameEn { get; set; }
    }
}

