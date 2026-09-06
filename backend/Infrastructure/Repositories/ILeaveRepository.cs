using Raras.EMS.API.Models.Entities;

namespace Raras.EMS.API.Repositories;

public interface ILeaveRepository
{
    Task<IEnumerable<LeaveRequest>> GetAllAsync();
    Task<LeaveRequest?> GetByIdAsync(int id);
    Task<LeaveRequest> AddAsync(LeaveRequest request);
    Task UpdateAsync(LeaveRequest request);
}
