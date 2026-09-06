using Microsoft.EntityFrameworkCore;
using Raras.EMS.API.Data;
using Raras.EMS.API.Models.Entities;

namespace Raras.EMS.API.Repositories;

public class AttendanceRepository : IAttendanceRepository
{
    private readonly EmsDbContext _db;

    public AttendanceRepository(EmsDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Attendance>> GetByDateAsync(DateTime date)
    {
        var targetDate = date.Date;
        return await _db.AttendanceRecords
            .AsNoTracking()
            .Include(a => a.Employee)
            .Where(a => a.Date.Date == targetDate)
            .ToListAsync();
    }

    public async Task<Attendance> AddAsync(Attendance record)
    {
        _db.AttendanceRecords.Add(record);
        await _db.SaveChangesAsync();
        return record;
    }
}
