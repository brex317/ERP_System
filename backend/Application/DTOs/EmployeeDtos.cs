using System.ComponentModel.DataAnnotations;

namespace Raras.EMS.API.Models.DTOs;

public class EmployeeResponseDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}".Trim();
    public string Email { get; set; } = string.Empty;
    public int? DepartmentId { get; set; }
    public string? DepartmentName { get; set; }
    public string Position { get; set; } = string.Empty;
    public string Status { get; set; } = "Active";
    public DateTime HireDate { get; set; }
}

public class CreateEmployeeDto
{
    [Required(ErrorMessage = "First Name is required.")]
    [MaxLength(50, ErrorMessage = "First Name cannot exceed 50 characters.")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last Name is required.")]
    [MaxLength(50, ErrorMessage = "Last Name cannot exceed 50 characters.")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address format.")]
    [MaxLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
    public string Email { get; set; } = string.Empty;

    public int? DepartmentId { get; set; }

    [MaxLength(100, ErrorMessage = "Position cannot exceed 100 characters.")]
    public string Position { get; set; } = string.Empty;

    [MaxLength(20, ErrorMessage = "Status cannot exceed 20 characters.")]
    public string Status { get; set; } = "Active";

    public DateTime? HireDate { get; set; }
}

public class UpdateEmployeeDto : CreateEmployeeDto
{
}
