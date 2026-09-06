using Raras.EMS.API.Models.DTOs;
using Raras.EMS.API.Models.Entities;
using Raras.EMS.API.Repositories;

namespace Raras.EMS.API.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _repository;

    public EmployeeService(IEmployeeRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<EmployeeResponseDto>> GetAllEmployeesAsync()
    {
        var employees = await _repository.GetAllAsync();
        return employees.Select(MapToResponseDto);
    }

    public async Task<EmployeeResponseDto?> GetEmployeeByIdAsync(int id)
    {
        var employee = await _repository.GetByIdAsync(id);
        return employee == null ? null : MapToResponseDto(employee);
    }

    public async Task<EmployeeResponseDto> CreateEmployeeAsync(CreateEmployeeDto dto)
    {
        var employee = new Employee
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            DepartmentId = dto.DepartmentId,
            Position = dto.Position,
            Status = dto.Status ?? "Active",
            HireDate = dto.HireDate ?? DateTime.UtcNow.Date,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _repository.AddAsync(employee);
        return MapToResponseDto(created);
    }

    public async Task<bool> UpdateEmployeeAsync(int id, UpdateEmployeeDto dto)
    {
        var employee = await _repository.GetByIdAsync(id);
        if (employee == null) return false;

        employee.FirstName = dto.FirstName;
        employee.LastName = dto.LastName;
        employee.Email = dto.Email;
        employee.DepartmentId = dto.DepartmentId;
        employee.Position = dto.Position;
        employee.Status = dto.Status;
        if (dto.HireDate.HasValue) employee.HireDate = dto.HireDate.Value;

        await _repository.UpdateAsync(employee);
        return true;
    }

    public async Task<bool> DeleteEmployeeAsync(int id)
    {
        var employee = await _repository.GetByIdAsync(id);
        if (employee == null) return false;

        await _repository.DeleteAsync(employee);
        return true;
    }

    private static EmployeeResponseDto MapToResponseDto(Employee e)
    {
        return new EmployeeResponseDto
        {
            Id = e.Id,
            FirstName = e.FirstName,
            LastName = e.LastName,
            Email = e.Email,
            DepartmentId = e.DepartmentId,
            DepartmentName = e.Department?.Name,
            Position = e.Position,
            Status = e.Status,
            HireDate = e.HireDate
        };
    }
}
