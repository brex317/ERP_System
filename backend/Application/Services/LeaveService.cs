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

    public async Task<IEnumerable<LeaveRequest>> GetAllLeaveRequestsAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<LeaveRequest> CreateLeaveRequestAsync(LeaveRequest request)
    {
        request.CreatedAt = DateTime.UtcNow;
        request.Status = "Pending";
        return await _repository.AddAsync(request);
    }

    public async Task<bool> UpdateLeaveStatusAsync(int id, string status)
    {
        var request = await _repository.GetByIdAsync(id);
        if (request == null) return false;

        request.Status = status;
        await _repository.UpdateAsync(request);
        return true;
    }
}
