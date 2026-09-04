using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Raras.EMS.API.Data;
using Raras.EMS.API.Models.DTOs;

namespace Raras.EMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly EmsDbContext _db;

    public DashboardController(EmsDbContext db)
    {
        _db = db;
    }

    [HttpGet("stats")]
    public async Task<ActionResult<DashboardStatsDto>> GetStats()
    {
        var today = DateTime.SpecifyKind(DateTime.Today.Date, DateTimeKind.Utc);

        var totalEmployees = await _db.Employees.CountAsync(e => e.Status == "Active");
        var totalDepartments = await _db.Departments.CountAsync();
        
        var presentToday = await _db.AttendanceRecords
            .CountAsync(a => a.Date.Date == today && a.Status == "Present");

        var onLeave = await _db.AttendanceRecords
            .CountAsync(a => a.Date.Date == today && a.Status == "On Leave");

        return Ok(new DashboardStatsDto
        {
            TotalEmployees = totalEmployees,
            TotalDepartments = totalDepartments,
            PresentToday = presentToday,
            OnLeave = onLeave
        });
    }
}
