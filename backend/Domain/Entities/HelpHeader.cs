using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Raras.EMS.API.Models.Entities;

[Table("help_headers")]
public class HelpHeader
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("feature_specification_id")]
    public int? FeatureSpecificationId { get; set; }

    [ForeignKey(nameof(FeatureSpecificationId))]
    public FeatureSpecification? FeatureSpecification { get; set; }

    [Column("feature_id")]
    public int? FeatureId { get; set; }

    [ForeignKey(nameof(FeatureId))]
    public Feature? Feature { get; set; }

    [Column("module_id")]
    public int? ModuleId { get; set; }

    [ForeignKey(nameof(ModuleId))]
    public Module? Module { get; set; }

    [Required]
    [MaxLength(255)]
    [Column("title")]
    public string Title { get; set; } = "Quick steps";

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<HelpDetail> Details { get; set; } = new List<HelpDetail>();
}
