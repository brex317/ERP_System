using System.ComponentModel.DataAnnotations.Schema;

namespace Raras.EMS.API.Models;

[Table("departments")]
public class Department
{
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Column("code")]
    public string Code { get; set; } = string.Empty;

    [Column("description")]
    public string? Description { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}

[Table("employees")]
public class Employee
{
    [Column("id")]
    public int Id { get; set; }

    [Column("first_name")]
    public string FirstName { get; set; } = string.Empty;

    [Column("last_name")]
    public string LastName { get; set; } = string.Empty;

    [Column("email")]
    public string Email { get; set; } = string.Empty;

    [Column("department_id")]
    public int? DepartmentId { get; set; }

    [Column("position")]
    public string Position { get; set; } = string.Empty;

    [Column("status")]
    public string Status { get; set; } = "Active";

    [Column("hire_date")]
    public DateTime HireDate { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}

[Table("attendance")]
public class Attendance
{
    [Column("id")]
    public int Id { get; set; }

    [Column("employee_id")]
    public int EmployeeId { get; set; }

    [Column("date")]
    public DateTime Date { get; set; }

    [Column("status")]
    public string Status { get; set; } = "Present";

    [Column("check_in")]
    public TimeSpan? CheckIn { get; set; }

    [Column("check_out")]
    public TimeSpan? CheckOut { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}

[Table("leave_requests")]
public class LeaveRequest
{
    [Column("id")]
    public int Id { get; set; }

    [Column("employee_id")]
    public int EmployeeId { get; set; }

    [Column("leave_type")]
    public string LeaveType { get; set; } = string.Empty;

    [Column("start_date")]
    public DateTime StartDate { get; set; }

    [Column("end_date")]
    public DateTime EndDate { get; set; }

    [Column("status")]
    public string Status { get; set; } = "Pending";

    [Column("reason")]
    public string? Reason { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}
