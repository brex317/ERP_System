using Raras.EMS.API.Models.Entities;

namespace Raras.EMS.API.Services;

public interface IAttendanceService
{
    Task<IEnumerable<Attendance>> GetAttendanceByDateAsync(DateTime? date);
    Task<Attendance> LogAttendanceAsync(Attendance record);
}
