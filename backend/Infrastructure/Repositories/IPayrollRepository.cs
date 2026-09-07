using Raras.EMS.API.Models.Entities;

namespace Raras.EMS.API.Repositories;

public interface IPayrollRepository
{
    Task<IEnumerable<PayrollRecord>> GetAllAsync();
    Task<PayrollRecord?> GetByIdAsync(int id);
    Task<IEnumerable<PayrollRecord>> AddRangeAsync(IEnumerable<PayrollRecord> records);
}
