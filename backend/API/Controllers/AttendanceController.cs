using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Raras.EMS.API.Models.DTOs;
using Raras.EMS.API.Services;

namespace Raras.EMS.API.Controllers;

[Authorize]
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
    public async Task<ActionResult<IEnumerable<AttendanceResponseDto>>> GetAttendance([FromQuery] DateTime? date)
    {
        var records = await _attendanceService.GetAttendanceByDateAsync(date);
        return Ok(records);
    }

    [HttpPost]
    public async Task<ActionResult<AttendanceResponseDto>> LogAttendance([FromBody] LogAttendanceDto dto)
    {
        var created = await _attendanceService.LogAttendanceAsync(dto);
        return Ok(created);
    }
}
