using Raras.EMS.API.Models.DTOs;

namespace Raras.EMS.API.Services;

public interface IDepartmentService
{
    Task<IEnumerable<DepartmentResponseDto>> GetAllDepartmentsAsync();
    Task<DepartmentResponseDto?> GetDepartmentByIdAsync(int id);
    Task<DepartmentResponseDto> CreateDepartmentAsync(CreateDepartmentDto dto);
    Task<bool> UpdateDepartmentAsync(int id, UpdateDepartmentDto dto);
    Task<bool> DeleteDepartmentAsync(int id);
}
