using Raras.EMS.API.Models.Entities;
using Raras.EMS.API.Repositories;

namespace Raras.EMS.API.Services;

public class AttendanceService : IAttendanceService
{
    private readonly IAttendanceRepository _repository;

    public AttendanceService(IAttendanceRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Attendance>> GetAttendanceByDateAsync(DateTime? date)
    {
        var targetDate = (date ?? DateTime.Today).Date;
        return await _repository.GetByDateAsync(targetDate);
    }

    public async Task<Attendance> LogAttendanceAsync(Attendance record)
    {
        record.CreatedAt = DateTime.UtcNow;
        return await _repository.AddAsync(record);
    }
}
