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
    public DbSet<Module> Modules => Set<Module>();
    public DbSet<Page> Pages => Set<Page>();
    public DbSet<Functionality> Functionalities => Set<Functionality>();
    public DbSet<HelpContext> HelpContexts => Set<HelpContext>();
    public DbSet<HelpStep> HelpSteps => Set<HelpStep>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Module>(entity =>
        {
            entity.HasIndex(e => e.Key).IsUnique();
        });

        modelBuilder.Entity<Page>(entity =>
        {
            entity.HasIndex(e => new { e.ModuleId, e.Key }).IsUnique();
            entity.HasOne(p => p.Module)
                  .WithMany(m => m.Pages)
                  .HasForeignKey(p => p.ModuleId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Functionality>(entity =>
        {
            entity.HasIndex(e => new { e.PageId, e.Key }).IsUnique();
            entity.HasOne(f => f.Page)
                  .WithMany(p => p.Functionalities)
                  .HasForeignKey(f => f.PageId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<HelpContext>(entity =>
        {
            entity.ToTable(t => t.HasCheckConstraint("chk_help_context_single_target",
                "((CASE WHEN functionality_id IS NOT NULL THEN 1 ELSE 0 END + " +
                 "CASE WHEN page_id IS NOT NULL THEN 1 ELSE 0 END + " +
                 "CASE WHEN module_id IS NOT NULL THEN 1 ELSE 0 END) = 1)"));

            entity.HasOne(h => h.Module)
                  .WithMany()
                  .HasForeignKey(h => h.ModuleId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(h => h.Page)
                  .WithMany()
                  .HasForeignKey(h => h.PageId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(h => h.Functionality)
                  .WithMany()
                  .HasForeignKey(h => h.FunctionalityId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<HelpStep>(entity =>
        {
            entity.HasIndex(e => new { e.HelpContextId, e.StepNumber }).IsUnique();
            entity.HasOne(s => s.HelpContext)
                  .WithMany(c => c.Steps)
                  .HasForeignKey(s => s.HelpContextId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
