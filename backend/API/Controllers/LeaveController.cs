using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Raras.EMS.API.Models.DTOs;
using Raras.EMS.API.Services;

namespace Raras.EMS.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class LeaveController : ControllerBase
{
    private static readonly HashSet<string> ValidStatuses = new(StringComparer.OrdinalIgnoreCase)
    {
        "Pending", "Approved", "Rejected"
    };

    private readonly ILeaveService _leaveService;

    public LeaveController(ILeaveService leaveService)
    {
        _leaveService = leaveService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LeaveRequestResponseDto>>> GetLeaveRequests()
    {
        var requests = await _leaveService.GetAllLeaveRequestsAsync();
        return Ok(requests);
    }

    [HttpPost]
    public async Task<ActionResult<LeaveRequestResponseDto>> CreateLeaveRequest([FromBody] CreateLeaveRequestDto dto)
    {
        var created = await _leaveService.CreateLeaveRequestAsync(dto);
        return Ok(created);
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateLeaveStatus(int id, [FromBody] UpdateLeaveStatusDto dto)
    {
        if (dto == null || string.IsNullOrWhiteSpace(dto.Status) || !ValidStatuses.Contains(dto.Status.Trim()))
        {
            return BadRequest(new { message = "Invalid status. Allowed values: Pending, Approved, Rejected." });
        }

        string normalizedStatus = char.ToUpper(dto.Status.Trim()[0]) + dto.Status.Trim().Substring(1).ToLower();

        var result = await _leaveService.UpdateLeaveStatusAsync(id, normalizedStatus);
        if (!result) return NotFound();

        return NoContent();
    }
}
