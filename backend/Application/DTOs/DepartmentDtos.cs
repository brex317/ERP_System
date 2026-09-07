using System.ComponentModel.DataAnnotations;

namespace Raras.EMS.API.Models.DTOs;

public class DepartmentResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateDepartmentDto
{
    [Required(ErrorMessage = "Department Name is required.")]
    [MaxLength(100, ErrorMessage = "Department Name cannot exceed 100 characters.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Department Code is required.")]
    [MaxLength(20, ErrorMessage = "Department Code cannot exceed 20 characters.")]
    public string Code { get; set; } = string.Empty;

    public string? Description { get; set; }
}

public class UpdateDepartmentDto : CreateDepartmentDto
{
}
