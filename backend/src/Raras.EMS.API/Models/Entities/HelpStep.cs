using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Raras.EMS.API.Models.Entities;

[Table("help_steps")]
public class HelpStep
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("help_context_id")]
    public int HelpContextId { get; set; }

    [ForeignKey(nameof(HelpContextId))]
    public HelpContext? HelpContext { get; set; }

    [Column("step_number")]
    public int StepNumber { get; set; }

    [Required]
    [Column("step_text")]
    public string StepText { get; set; } = string.Empty;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
