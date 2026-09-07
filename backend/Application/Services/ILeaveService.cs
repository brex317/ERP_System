using Raras.EMS.API.Models.DTOs;

namespace Raras.EMS.API.Services;

public interface ILeaveService
{
    Task<IEnumerable<LeaveRequestResponseDto>> GetAllLeaveRequestsAsync();
    Task<LeaveRequestResponseDto> CreateLeaveRequestAsync(CreateLeaveRequestDto dto);
    Task<bool> UpdateLeaveStatusAsync(int id, string status);
}
