using Microsoft.EntityFrameworkCore;
using Raras.EMS.API.Data;
using Raras.EMS.API.Models.Entities;

namespace Raras.EMS.API.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly EmsDbContext _db;

    public EmployeeRepository(EmsDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Employee>> GetAllAsync()
    {
        return await _db.Employees
            .AsNoTracking()
            .Include(e => e.Department)
            .OrderByDescending(e => e.Id)
            .ToListAsync();
    }

    public async Task<Employee?> GetByIdAsync(int id)
    {
        return await _db.Employees
            .Include(e => e.Department)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<Employee> AddAsync(Employee employee)
    {
        _db.Employees.Add(employee);
        await _db.SaveChangesAsync();
        return employee;
    }

    public async Task UpdateAsync(Employee employee)
    {
        _db.Employees.Update(employee);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Employee employee)
    {
        _db.Employees.Remove(employee);
        await _db.SaveChangesAsync();
    }
}
