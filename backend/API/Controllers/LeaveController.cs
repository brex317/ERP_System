using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Raras.EMS.API.Data;
using Raras.EMS.API.Models.Entities;

namespace Raras.EMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LeaveController : ControllerBase
{
    private readonly EmsDbContext _db;

    public LeaveController(EmsDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LeaveRequest>>> GetLeaveRequests()
    {
        var requests = await _db.LeaveRequests
            .Include(l => l.Employee)
            .OrderByDescending(l => l.CreatedAt)
            .ToListAsync();

        return Ok(requests);
    }

    [HttpPost]
    public async Task<ActionResult<LeaveRequest>> CreateLeaveRequest([FromBody] LeaveRequest request)
    {
        request.CreatedAt = DateTime.UtcNow;
        request.Status = "Pending";
        _db.LeaveRequests.Add(request);
        await _db.SaveChangesAsync();

        return Ok(request);
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateLeaveStatus(int id, [FromBody] string status)
    {
        var request = await _db.LeaveRequests.FindAsync(id);
        if (request == null) return NotFound();

        request.Status = status;
        await _db.SaveChangesAsync();

        return NoContent();
    }
}
