using Raras.EMS.API.Models.Entities;

namespace Raras.EMS.API.Services;

public interface IDepartmentService
{
    Task<IEnumerable<Department>> GetAllDepartmentsAsync();
    Task<Department?> GetDepartmentByIdAsync(int id);
    Task<Department> CreateDepartmentAsync(Department department);
    Task<bool> UpdateDepartmentAsync(int id, Department updated);
    Task<bool> DeleteDepartmentAsync(int id);
}
