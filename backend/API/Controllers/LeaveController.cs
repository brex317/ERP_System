using Microsoft.AspNetCore.Mvc;
using Raras.EMS.API.Models.Entities;
using Raras.EMS.API.Services;

namespace Raras.EMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LeaveController : ControllerBase
{
    private readonly ILeaveService _leaveService;

    public LeaveController(ILeaveService leaveService)
    {
        _leaveService = leaveService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LeaveRequest>>> GetLeaveRequests()
    {
        var requests = await _leaveService.GetAllLeaveRequestsAsync();
        return Ok(requests);
    }

    [HttpPost]
    public async Task<ActionResult<LeaveRequest>> CreateLeaveRequest([FromBody] LeaveRequest request)
    {
        var created = await _leaveService.CreateLeaveRequestAsync(request);
        return Ok(created);
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateLeaveStatus(int id, [FromBody] string status)
    {
        var result = await _leaveService.UpdateLeaveStatusAsync(id, status);
        if (!result) return NotFound();

        return NoContent();
    }
}
