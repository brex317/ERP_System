using Raras.EMS.API.Models.DTOs;
using Raras.EMS.API.Models.Entities;
using Raras.EMS.API.Repositories;

namespace Raras.EMS.API.Services;

public class LeaveService : ILeaveService
{
    private readonly ILeaveRepository _repository;

    public LeaveService(ILeaveRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<LeaveRequestResponseDto>> GetAllLeaveRequestsAsync()
    {
        var requests = await _repository.GetAllAsync();
        return requests.Select(MapToResponseDto);
    }

    public async Task<LeaveRequestResponseDto> CreateLeaveRequestAsync(CreateLeaveRequestDto dto)
    {
        var request = new LeaveRequest
        {
            EmployeeId = dto.EmployeeId,
            LeaveType = dto.LeaveType,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            Reason = dto.Reason,
            Status = "Pending",
            CreatedAt = DateTime.UtcNow
        };

        var created = await _repository.AddAsync(request);
        return MapToResponseDto(created);
    }

    public async Task<bool> UpdateLeaveStatusAsync(int id, string status)
    {
        var request = await _repository.GetByIdAsync(id);
        if (request == null) return false;

        request.Status = status;
        await _repository.UpdateAsync(request);
        return true;
    }

    private static LeaveRequestResponseDto MapToResponseDto(LeaveRequest l)
    {
        string? empName = null;
        if (l.Employee != null)
        {
            empName = $"{l.Employee.FirstName} {l.Employee.LastName}".Trim();
        }

        return new LeaveRequestResponseDto
        {
            Id = l.Id,
            EmployeeId = l.EmployeeId,
            EmployeeName = empName,
            LeaveType = l.LeaveType,
            StartDate = l.StartDate,
            EndDate = l.EndDate,
            Status = l.Status,
            Reason = l.Reason,
            CreatedAt = l.CreatedAt
        };
    }
}
