using Raras.EMS.API.Models.DTOs;

namespace Raras.EMS.API.Services;

public interface IAttendanceService
{
    Task<IEnumerable<AttendanceResponseDto>> GetAttendanceByDateAsync(DateTime? date);
    Task<AttendanceResponseDto> LogAttendanceAsync(LogAttendanceDto dto);
}
