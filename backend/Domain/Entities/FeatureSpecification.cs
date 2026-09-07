using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Raras.EMS.API.Models.Entities;

[Table("feature_specifications")]
public class FeatureSpecification
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("feature_id")]
    public int FeatureId { get; set; }

    [ForeignKey(nameof(FeatureId))]
    public Feature? Feature { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("key")]
    public string Key { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    [Column("display_name")]
    public string DisplayName { get; set; } = string.Empty;
}
