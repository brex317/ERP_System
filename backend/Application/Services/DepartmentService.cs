using Raras.EMS.API.Models.DTOs;
using Raras.EMS.API.Models.Entities;
using Raras.EMS.API.Repositories;

namespace Raras.EMS.API.Services;

public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository _repository;

    public DepartmentService(IDepartmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<DepartmentResponseDto>> GetAllDepartmentsAsync()
    {
        var departments = await _repository.GetAllAsync();
        return departments.Select(MapToResponseDto);
    }

    public async Task<DepartmentResponseDto?> GetDepartmentByIdAsync(int id)
    {
        var dept = await _repository.GetByIdAsync(id);
        return dept == null ? null : MapToResponseDto(dept);
    }

    public async Task<DepartmentResponseDto> CreateDepartmentAsync(CreateDepartmentDto dto)
    {
        var department = new Department
        {
            Name = dto.Name,
            Code = dto.Code,
            Description = dto.Description,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _repository.AddAsync(department);
        return MapToResponseDto(created);
    }

    public async Task<bool> UpdateDepartmentAsync(int id, UpdateDepartmentDto dto)
    {
        var dept = await _repository.GetByIdAsync(id);
        if (dept == null) return false;

        dept.Name = dto.Name;
        dept.Code = dto.Code;
        dept.Description = dto.Description;

        await _repository.UpdateAsync(dept);
        return true;
    }

    public async Task<bool> DeleteDepartmentAsync(int id)
    {
        var dept = await _repository.GetByIdAsync(id);
        if (dept == null) return false;

        await _repository.DeleteAsync(dept);
        return true;
    }

    private static DepartmentResponseDto MapToResponseDto(Department d)
    {
        return new DepartmentResponseDto
        {
            Id = d.Id,
            Name = d.Name,
            Code = d.Code,
            Description = d.Description,
            CreatedAt = d.CreatedAt
        };
    }
}
