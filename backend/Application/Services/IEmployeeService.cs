using Raras.EMS.API.Models.DTOs;

namespace Raras.EMS.API.Services;

public interface IEmployeeService
{
    Task<IEnumerable<EmployeeResponseDto>> GetAllEmployeesAsync();
    Task<EmployeeResponseDto?> GetEmployeeByIdAsync(int id);
    Task<EmployeeResponseDto> CreateEmployeeAsync(CreateEmployeeDto dto);
    Task<bool> UpdateEmployeeAsync(int id, UpdateEmployeeDto dto);
    Task<bool> DeleteEmployeeAsync(int id);
}
