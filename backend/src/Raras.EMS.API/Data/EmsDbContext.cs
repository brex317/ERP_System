using Microsoft.EntityFrameworkCore;
using Raras.EMS.API.Models.Entities;

namespace Raras.EMS.API.Data;

public class EmsDbContext : DbContext
{
    public EmsDbContext(DbContextOptions<EmsDbContext> options) : base(options) { }

    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Attendance> AttendanceRecords => Set<Attendance>();
    public DbSet<LeaveRequest> LeaveRequests => Set<LeaveRequest>();
    public DbSet<HelpContext> HelpContexts => Set<HelpContext>();
    public DbSet<HelpStep> HelpSteps => Set<HelpStep>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<User> Users => Set<User>();
}
