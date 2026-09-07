using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Raras.EMS.API.Models.DTOs;
using Raras.EMS.API.Services;

namespace Raras.EMS.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PayrollController : ControllerBase
{
    private readonly IPayrollService _payrollService;

    public PayrollController(IPayrollService payrollService)
    {
        _payrollService = payrollService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PayrollDto>>> GetPayroll()
    {
        var records = await _payrollService.GetPayrollSummaryAsync();
        return Ok(records);
    }

    [HttpPost("process")]
    public async Task<ActionResult<IEnumerable<PayrollDto>>> ProcessPayroll([FromBody] ProcessPayrollDto dto)
    {
        var period = dto?.PayPeriod ?? "2026-09";
        var records = await _payrollService.ProcessPayrollAsync(period);
        return Ok(records);
    }
}
