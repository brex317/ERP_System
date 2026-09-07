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
    public DbSet<Feature> Features => Set<Feature>();
    public DbSet<FeatureSpecification> FeatureSpecifications => Set<FeatureSpecification>();
    public DbSet<HelpHeader> HelpHeaders => Set<HelpHeader>();
    public DbSet<HelpDetail> HelpDetails => Set<HelpDetail>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<SupportTicket> SupportTickets => Set<SupportTicket>();
    public DbSet<PayrollRecord> PayrollRecords => Set<PayrollRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Module>(entity =>
        {
            entity.HasIndex(e => e.Key).IsUnique();
        });

        modelBuilder.Entity<Feature>(entity =>
        {
            entity.HasIndex(e => new { e.ModuleId, e.Key }).IsUnique();
            entity.HasOne(p => p.Module)
                  .WithMany(m => m.Features)
                  .HasForeignKey(p => p.ModuleId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<FeatureSpecification>(entity =>
        {
            entity.HasIndex(e => new { e.FeatureId, e.Key }).IsUnique();
            entity.HasOne(f => f.Feature)
                  .WithMany(p => p.FeatureSpecifications)
                  .HasForeignKey(f => f.FeatureId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<HelpHeader>(entity =>
        {
            entity.ToTable(t => t.HasCheckConstraint("chk_help_header_single_target",
                "((CASE WHEN feature_specification_id IS NOT NULL THEN 1 ELSE 0 END + " +
                 "CASE WHEN feature_id IS NOT NULL THEN 1 ELSE 0 END + " +
                 "CASE WHEN module_id IS NOT NULL THEN 1 ELSE 0 END) = 1)"));

            entity.HasOne(h => h.Module)
                  .WithMany()
                  .HasForeignKey(h => h.ModuleId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(h => h.Feature)
                  .WithMany()
                  .HasForeignKey(h => h.FeatureId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(h => h.FeatureSpecification)
                  .WithMany()
                  .HasForeignKey(h => h.FeatureSpecificationId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<HelpDetail>(entity =>
        {
            entity.HasIndex(e => new { e.HelpHeaderId, e.StepNumber }).IsUnique();
            entity.HasOne(s => s.HelpHeader)
                  .WithMany(c => c.Details)
                  .HasForeignKey(s => s.HelpHeaderId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
