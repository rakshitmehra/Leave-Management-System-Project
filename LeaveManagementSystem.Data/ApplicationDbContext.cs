using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace LeaveManagementSystem.Data
{
    /// <summary>
    /// The application's main database context, extending IdentityDbContext to includes ASP.NET Core Identity tables along with custom domain entities.
    /// It seeds default roles, a default administrator user, and assigns that user to the Administrator role.
    /// This context also exposes DbSets for Leave Types, Leave Allocations, and Periods, allowing EF Core to manage these tables.
    /// </summary>

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet<LeaveType> LeaveTypes { get; set; }
        public DbSet<LeaveAllocation> LeaveAllocations { get; set; }
        public DbSet<Period> Periods { get; set; }
        public DbSet<LeaveRequestStatus> LeaveRequestStatuses { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }

    }
}
