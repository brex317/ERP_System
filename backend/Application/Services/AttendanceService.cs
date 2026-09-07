using Raras.EMS.API.Models.DTOs;
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

    public async Task<IEnumerable<AttendanceResponseDto>> GetAttendanceByDateAsync(DateTime? date)
    {
        var targetDate = (date ?? DateTime.Today).Date;
        var records = await _repository.GetByDateAsync(targetDate);
        return records.Select(MapToResponseDto);
    }

    public async Task<AttendanceResponseDto> LogAttendanceAsync(LogAttendanceDto dto)
    {
        var record = new Attendance
        {
            EmployeeId = dto.EmployeeId,
            Date = dto.Date ?? DateTime.UtcNow.Date,
            Status = dto.Status ?? "Present",
            CheckIn = dto.CheckIn,
            CheckOut = dto.CheckOut,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _repository.AddAsync(record);
        return MapToResponseDto(created);
    }

    private static AttendanceResponseDto MapToResponseDto(Attendance a)
    {
        string? empName = null;
        if (a.Employee != null)
        {
            empName = $"{a.Employee.FirstName} {a.Employee.LastName}".Trim();
        }

        return new AttendanceResponseDto
        {
            Id = a.Id,
            EmployeeId = a.EmployeeId,
            EmployeeName = empName,
            Date = a.Date,
            Status = a.Status,
            CheckIn = a.CheckIn,
            CheckOut = a.CheckOut,
            CreatedAt = a.CreatedAt
        };
    }
}
