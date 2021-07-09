using AnnualLeaveSystem.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AnnualLeaveSystem.Data
{
    public class LeaveSystemDbContext : IdentityDbContext
    {
        public LeaveSystemDbContext(DbContextOptions<LeaveSystemDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Leave> Leaves { get; set; }

        public DbSet<LeaveType> LeaveTypes { get; set; }

        public DbSet<Department> Departments { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<Project> Projects { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
             .Entity<Employee>()
             .HasOne(e => e.Department)
             .WithMany(d => d.Employees)
             .HasForeignKey(e => e.DepartmentId)
             .OnDelete(DeleteBehavior.Restrict);

            builder
             .Entity<Employee>()
             .HasOne(e => e.Team)
             .WithMany(t => t.Employees)
             .HasForeignKey(e => e.TeamId)
             .OnDelete(DeleteBehavior.Restrict);

            builder
             .Entity<Employee>()
             .HasOne(e => e.TeamLead)
             .WithMany()
             .HasForeignKey(e => e.TeamLeadId)
             .OnDelete(DeleteBehavior.Restrict);

            builder
              .Entity<Leave>()
              .HasOne(l => l.RequestEmployee)
              .WithMany(re => re.RequestedLeaves)
              .HasForeignKey(l => l.RequestEmployeeId)
              .OnDelete(DeleteBehavior.Restrict);

            builder
              .Entity<Leave>()
              .HasOne(l => l.ApproveEmployee)
              .WithMany(ae => ae.ApprovedLeaves)
              .HasForeignKey(l => l.ApproveEmployeeId)
              .OnDelete(DeleteBehavior.Restrict);

            builder
              .Entity<Leave>()
              .HasOne(l => l.LeaveType)
              .WithMany(lt => lt.Leaves)
              .HasForeignKey(l => l.LeaveTypeId)
              .OnDelete(DeleteBehavior.Restrict);

            builder
              .Entity<Team>()
              .HasOne(t=> t.Project)
              .WithMany(p => p.Teams)
              .HasForeignKey(t => t.ProjectId)
              .OnDelete(DeleteBehavior.Restrict);


            base.OnModelCreating(builder);
        }
    }
}
