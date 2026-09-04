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
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int? DepartmentId { get; set; }
    public string Position { get; set; } = string.Empty;
    public string Status { get; set; } = "Active";
    public DateTime? HireDate { get; set; }
}

public class UpdateEmployeeDto : CreateEmployeeDto
{
}
