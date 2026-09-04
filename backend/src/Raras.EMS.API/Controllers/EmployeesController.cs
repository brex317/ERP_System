using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Raras.EMS.API.Data;
using Raras.EMS.API.Models.DTOs;
using Raras.EMS.API.Models.Entities;

namespace Raras.EMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly EmsDbContext _db;

    public EmployeesController(EmsDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EmployeeResponseDto>>> GetEmployees()
    {
        var employees = await _db.Employees
            .Include(e => e.Department)
            .OrderByDescending(e => e.Id)
            .Select(e => new EmployeeResponseDto
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Email = e.Email,
                DepartmentId = e.DepartmentId,
                DepartmentName = e.Department != null ? e.Department.Name : null,
                Position = e.Position,
                Status = e.Status,
                HireDate = e.HireDate
            })
            .ToListAsync();

        return Ok(employees);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EmployeeResponseDto>> GetEmployee(int id)
    {
        var e = await _db.Employees
            .Include(x => x.Department)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (e == null) return NotFound();

        return Ok(new EmployeeResponseDto
        {
            Id = e.Id,
            FirstName = e.FirstName,
            LastName = e.LastName,
            Email = e.Email,
            DepartmentId = e.DepartmentId,
            DepartmentName = e.Department?.Name,
            Position = e.Position,
            Status = e.Status,
            HireDate = e.HireDate
        });
    }

    [HttpPost]
    public async Task<ActionResult<EmployeeResponseDto>> CreateEmployee([FromBody] CreateEmployeeDto dto)
    {
        var employee = new Employee
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            DepartmentId = dto.DepartmentId,
            Position = dto.Position,
            Status = dto.Status ?? "Active",
            HireDate = dto.HireDate ?? DateTime.UtcNow.Date,
            CreatedAt = DateTime.UtcNow
        };

        _db.Employees.Add(employee);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, new EmployeeResponseDto
        {
            Id = employee.Id,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Email = employee.Email,
            DepartmentId = employee.DepartmentId,
            Position = employee.Position,
            Status = employee.Status,
            HireDate = employee.HireDate
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEmployee(int id, [FromBody] UpdateEmployeeDto dto)
    {
        var employee = await _db.Employees.FindAsync(id);
        if (employee == null) return NotFound();

        employee.FirstName = dto.FirstName;
        employee.LastName = dto.LastName;
        employee.Email = dto.Email;
        employee.DepartmentId = dto.DepartmentId;
        employee.Position = dto.Position;
        employee.Status = dto.Status;
        if (dto.HireDate.HasValue) employee.HireDate = dto.HireDate.Value;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        var employee = await _db.Employees.FindAsync(id);
        if (employee == null) return NotFound();

        _db.Employees.Remove(employee);
        await _db.SaveChangesAsync();

        return NoContent();
    }
}
