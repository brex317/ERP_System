using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Raras.EMS.API.Models.Entities;

[Table("support_tickets")]
public class SupportTicket
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("user_id")]
    public int? UserId { get; set; }

    [Required]
    [MaxLength(200)]
    [Column("subject")]
    public string Subject { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    [Column("category")]
    public string Category { get; set; } = "General";

    [Required]
    [MaxLength(20)]
    [Column("priority")]
    public string Priority { get; set; } = "Medium"; // Low, Medium, High, Urgent

    [Required]
    [Column("message")]
    public string Message { get; set; } = string.Empty;

    [MaxLength(20)]
    [Column("status")]
    public string Status { get; set; } = "Open"; // Open, In Progress, Resolved, Closed

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
