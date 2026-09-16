using AppleWorldShoppingMallEMS.Models;
using Microsoft.EntityFrameworkCore;

namespace AppleWorldShoppingMallEMS.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<WorkSchedule> WorkSchedules => Set<WorkSchedule>();
    public DbSet<Assessment> Assessments => Set<Assessment>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<AppUser> Users => Set<AppUser>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>()
            .HasIndex(x => x.NationalId).IsUnique();

        modelBuilder.Entity<Employee>()
            .HasIndex(x => x.Email).IsUnique();

        modelBuilder.Entity<AppUser>()
            .HasIndex(x => x.Username).IsUnique();

        modelBuilder.Entity<Employee>()
            .HasOne(x => x.Department)
            .WithMany(x => x.Employees)
            .HasForeignKey(x => x.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<WorkSchedule>()
            .HasOne(x => x.Employee)
            .WithMany(x => x.Schedules)
            .HasForeignKey(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Assessment>()
            .HasOne(x => x.Employee)
            .WithMany(x => x.Assessments)
            .HasForeignKey(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Payment>()
            .HasOne(x => x.Employee)
            .WithMany(x => x.Payments)
            .HasForeignKey(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}