using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Raras.EMS.API.Data;
using Raras.EMS.API.Models.Entities;

namespace Raras.EMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AttendanceController : ControllerBase
{
    private readonly EmsDbContext _db;

    public AttendanceController(EmsDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Attendance>>> GetAttendance([FromQuery] DateTime? date)
    {
        var targetDate = (date ?? DateTime.Today).Date;
        var records = await _db.AttendanceRecords
            .Include(a => a.Employee)
            .Where(a => a.Date.Date == targetDate)
            .ToListAsync();

        return Ok(records);
    }

    [HttpPost]
    public async Task<ActionResult<Attendance>> LogAttendance([FromBody] Attendance record)
    {
        record.CreatedAt = DateTime.UtcNow;
        _db.AttendanceRecords.Add(record);
        await _db.SaveChangesAsync();

        return Ok(record);
    }
}
