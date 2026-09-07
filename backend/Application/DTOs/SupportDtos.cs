using System.ComponentModel.DataAnnotations;

namespace Raras.EMS.API.Models.DTOs;

public class SupportTicketDto
{
    public int Id { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Category { get; set; } = "General";
    public string Priority { get; set; } = "Medium";
    public string Message { get; set; } = string.Empty;
    public string Status { get; set; } = "Open";
    public DateTime CreatedAt { get; set; }
}

public class CreateSupportTicketDto
{
    [Required(ErrorMessage = "Subject is required.")]
    [MaxLength(200)]
    public string Subject { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Category { get; set; } = "General";

    [Required]
    [MaxLength(20)]
    public string Priority { get; set; } = "Medium";

    [Required(ErrorMessage = "Message details are required.")]
    public string Message { get; set; } = string.Empty;
}
