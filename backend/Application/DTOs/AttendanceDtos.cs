using System.ComponentModel.DataAnnotations;

namespace Raras.EMS.API.Models.DTOs;

public class AttendanceResponseDto
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public string? EmployeeName { get; set; }
    public DateTime Date { get; set; }
    public string Status { get; set; } = "Present";
    public TimeSpan? CheckIn { get; set; }
    public TimeSpan? CheckOut { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class LogAttendanceDto
{
    [Required(ErrorMessage = "EmployeeId is required.")]
    public int EmployeeId { get; set; }

    public DateTime? Date { get; set; }

    [MaxLength(20, ErrorMessage = "Status cannot exceed 20 characters.")]
    public string? Status { get; set; } = "Present";

    public TimeSpan? CheckIn { get; set; }
    public TimeSpan? CheckOut { get; set; }
}
