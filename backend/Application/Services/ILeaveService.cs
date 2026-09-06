using Raras.EMS.API.Models.Entities;

namespace Raras.EMS.API.Services;

public interface ILeaveService
{
    Task<IEnumerable<LeaveRequest>> GetAllLeaveRequestsAsync();
    Task<LeaveRequest> CreateLeaveRequestAsync(LeaveRequest request);
    Task<bool> UpdateLeaveStatusAsync(int id, string status);
}
