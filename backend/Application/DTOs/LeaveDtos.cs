using System.ComponentModel.DataAnnotations;

namespace Raras.EMS.API.Models.DTOs;

public class LeaveRequestResponseDto
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public string? EmployeeName { get; set; }
    public string LeaveType { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Status { get; set; } = "Pending";
    public string? Reason { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateLeaveRequestDto
{
    [Required(ErrorMessage = "EmployeeId is required.")]
    public int EmployeeId { get; set; }

    [Required(ErrorMessage = "LeaveType is required.")]
    [MaxLength(50, ErrorMessage = "LeaveType cannot exceed 50 characters.")]
    public string LeaveType { get; set; } = string.Empty;

    [Required(ErrorMessage = "StartDate is required.")]
    public DateTime StartDate { get; set; }

    [Required(ErrorMessage = "EndDate is required.")]
    public DateTime EndDate { get; set; }

    public string? Reason { get; set; }
}

public class UpdateLeaveStatusDto
{
    [Required(ErrorMessage = "Status is required.")]
    [MaxLength(20, ErrorMessage = "Status cannot exceed 20 characters.")]
    public string Status { get; set; } = string.Empty;
}
