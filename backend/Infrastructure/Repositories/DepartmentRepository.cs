using Microsoft.EntityFrameworkCore;
using Raras.EMS.API.Data;
using Raras.EMS.API.Models.Entities;

namespace Raras.EMS.API.Repositories;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly EmsDbContext _db;

    public DepartmentRepository(EmsDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Department>> GetAllAsync()
    {
        return await _db.Departments
            .AsNoTracking()
            .OrderBy(d => d.Name)
            .ToListAsync();
    }

    public async Task<Department?> GetByIdAsync(int id)
    {
        return await _db.Departments.FindAsync(id);
    }

    public async Task<Department> AddAsync(Department department)
    {
        _db.Departments.Add(department);
        await _db.SaveChangesAsync();
        return department;
    }

    public async Task UpdateAsync(Department department)
    {
        _db.Departments.Update(department);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Department department)
    {
        _db.Departments.Remove(department);
        await _db.SaveChangesAsync();
    }
}
