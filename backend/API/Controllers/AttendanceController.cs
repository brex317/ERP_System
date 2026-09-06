using Microsoft.AspNetCore.Mvc;
using Raras.EMS.API.Models.Entities;
using Raras.EMS.API.Services;

namespace Raras.EMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AttendanceController : ControllerBase
{
    private readonly IAttendanceService _attendanceService;

    public AttendanceController(IAttendanceService attendanceService)
    {
        _attendanceService = attendanceService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Attendance>>> GetAttendance([FromQuery] DateTime? date)
    {
        var records = await _attendanceService.GetAttendanceByDateAsync(date);
        return Ok(records);
    }

    [HttpPost]
    public async Task<ActionResult<Attendance>> LogAttendance([FromBody] Attendance record)
    {
        var created = await _attendanceService.LogAttendanceAsync(record);
        return Ok(created);
    }
}
