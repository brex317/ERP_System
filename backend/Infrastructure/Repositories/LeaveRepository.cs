using Microsoft.EntityFrameworkCore;
using Raras.EMS.API.Data;
using Raras.EMS.API.Models.Entities;

namespace Raras.EMS.API.Repositories;

public class LeaveRepository : ILeaveRepository
{
    private readonly EmsDbContext _db;

    public LeaveRepository(EmsDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<LeaveRequest>> GetAllAsync()
    {
        return await _db.LeaveRequests
            .AsNoTracking()
            .Include(l => l.Employee)
            .OrderByDescending(l => l.CreatedAt)
            .ToListAsync();
    }

    public async Task<LeaveRequest?> GetByIdAsync(int id)
    {
        return await _db.LeaveRequests.FindAsync(id);
    }

    public async Task<LeaveRequest> AddAsync(LeaveRequest request)
    {
        _db.LeaveRequests.Add(request);
        await _db.SaveChangesAsync();
        return request;
    }

    public async Task UpdateAsync(LeaveRequest request)
    {
        _db.LeaveRequests.Update(request);
        await _db.SaveChangesAsync();
    }
}
