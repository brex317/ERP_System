using Microsoft.AspNetCore.Mvc;
using Raras.EMS.API.Models.Entities;
using Raras.EMS.API.Services;

namespace Raras.EMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepartmentsController : ControllerBase
{
    private readonly IDepartmentService _departmentService;

    public DepartmentsController(IDepartmentService departmentService)
    {
        _departmentService = departmentService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Department>>> GetDepartments()
    {
        var departments = await _departmentService.GetAllDepartmentsAsync();
        return Ok(departments);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Department>> GetDepartment(int id)
    {
        var dept = await _departmentService.GetDepartmentByIdAsync(id);
        if (dept == null) return NotFound();
        return Ok(dept);
    }

    [HttpPost]
    public async Task<ActionResult<Department>> CreateDepartment([FromBody] Department department)
    {
        var created = await _departmentService.CreateDepartmentAsync(department);
        return CreatedAtAction(nameof(GetDepartment), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDepartment(int id, [FromBody] Department updated)
    {
        var result = await _departmentService.UpdateDepartmentAsync(id, updated);
        if (!result) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDepartment(int id)
    {
        var result = await _departmentService.DeleteDepartmentAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }
}
