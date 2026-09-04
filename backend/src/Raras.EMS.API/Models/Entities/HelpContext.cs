using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Raras.EMS.API.Models.Entities;

[Table("help_contexts")]
public class HelpContext
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("module_key")]
    public string ModuleKey { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    [Column("page_key")]
    public string PageKey { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    [Column("functionality_key")]
    public string FunctionalityKey { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    [Column("title")]
    public string Title { get; set; } = "Quick steps";

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<HelpStep> Steps { get; set; } = new List<HelpStep>();
}
