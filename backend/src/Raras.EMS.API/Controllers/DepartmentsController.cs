using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Raras.EMS.API.Data;
using Raras.EMS.API.Models.Entities;

namespace Raras.EMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepartmentsController : ControllerBase
{
    private readonly EmsDbContext _db;

    public DepartmentsController(EmsDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Department>>> GetDepartments()
    {
        return Ok(await _db.Departments.OrderBy(d => d.Name).ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Department>> GetDepartment(int id)
    {
        var dept = await _db.Departments.FindAsync(id);
        if (dept == null) return NotFound();
        return Ok(dept);
    }

    [HttpPost]
    public async Task<ActionResult<Department>> CreateDepartment([FromBody] Department department)
    {
        department.CreatedAt = DateTime.UtcNow;
        _db.Departments.Add(department);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetDepartment), new { id = department.Id }, department);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDepartment(int id, [FromBody] Department updated)
    {
        var dept = await _db.Departments.FindAsync(id);
        if (dept == null) return NotFound();

        dept.Name = updated.Name;
        dept.Code = updated.Code;
        dept.Description = updated.Description;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDepartment(int id)
    {
        var dept = await _db.Departments.FindAsync(id);
        if (dept == null) return NotFound();

        _db.Departments.Remove(dept);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
