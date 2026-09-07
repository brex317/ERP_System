using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Raras.EMS.API.Models.Entities;

[Table("help_details")]
public class HelpDetail
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("help_header_id")]
    public int HelpHeaderId { get; set; }

    [ForeignKey(nameof(HelpHeaderId))]
    public HelpHeader? HelpHeader { get; set; }

    [Column("step_number")]
    public int StepNumber { get; set; }

    [Required]
    [Column("step_text")]
    public string StepText { get; set; } = string.Empty;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
