using Microsoft.EntityFrameworkCore;
using Raras.EMS.API.Data;
using Raras.EMS.API.Models.Entities;

namespace Raras.EMS.API.Repositories;

public class PayrollRepository : IPayrollRepository
{
    private readonly EmsDbContext _db;

    public PayrollRepository(EmsDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<PayrollRecord>> GetAllAsync()
    {
        return await _db.PayrollRecords
            .AsNoTracking()
            .Include(p => p.Employee)
                .ThenInclude(e => e!.Department)
            .OrderByDescending(p => p.Id)
            .ToListAsync();
    }

    public async Task<PayrollRecord?> GetByIdAsync(int id)
    {
        return await _db.PayrollRecords
            .Include(p => p.Employee)
                .ThenInclude(e => e!.Department)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<PayrollRecord>> AddRangeAsync(IEnumerable<PayrollRecord> records)
    {
        _db.PayrollRecords.AddRange(records);
        await _db.SaveChangesAsync();
        return records;
    }
}
