using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Raras.EMS.API.Models.Entities;

[Table("help_contexts")]
public class HelpContext
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("functionality_id")]
    public int? FunctionalityId { get; set; }

    [ForeignKey(nameof(FunctionalityId))]
    public Functionality? Functionality { get; set; }

    [Column("page_id")]
    public int? PageId { get; set; }

    [ForeignKey(nameof(PageId))]
    public Page? Page { get; set; }

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

    public ICollection<HelpStep> Steps { get; set; } = new List<HelpStep>();
}
