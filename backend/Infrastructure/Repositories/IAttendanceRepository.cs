using Raras.EMS.API.Models.Entities;

namespace Raras.EMS.API.Repositories;

public interface IAttendanceRepository
{
    Task<IEnumerable<Attendance>> GetByDateAsync(DateTime date);
    Task<Attendance> AddAsync(Attendance record);
}
