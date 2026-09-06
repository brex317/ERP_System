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

    public async Task<IEnumerable<Department>> GetAllDepartmentsAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Department?> GetDepartmentByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<Department> CreateDepartmentAsync(Department department)
    {
        department.CreatedAt = DateTime.UtcNow;
        return await _repository.AddAsync(department);
    }

    public async Task<bool> UpdateDepartmentAsync(int id, Department updated)
    {
        var dept = await _repository.GetByIdAsync(id);
        if (dept == null) return false;

        dept.Name = updated.Name;
        dept.Code = updated.Code;
        dept.Description = updated.Description;

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
}
