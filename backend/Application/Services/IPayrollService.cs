using Raras.EMS.API.Models.DTOs;

namespace Raras.EMS.API.Services;

public interface IPayrollService
{
    Task<IEnumerable<PayrollDto>> GetPayrollSummaryAsync();
    Task<IEnumerable<PayrollDto>> ProcessPayrollAsync(string payPeriod);
}
