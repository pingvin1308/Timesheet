using Microsoft.EntityFrameworkCore;
using Timesheet.DataAccess.MSSQL.Entities;

namespace Timesheet.DataAccess.MSSQL
{
    public class TimesheetContext : DbContext
    {
        public TimesheetContext(DbContextOptions<TimesheetContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .HasData(new[] 
                {
                    new Employee
                    {
                        Id = 1,
                        LastName = "Иванов",
                        Position = Domain.Models.Position.Chef,
                        Salary = 200000m,
                        Bonus = 20000m
                    },
                    new Employee
                    {
                        Id = 2,
                        LastName = "Сидоров",
                        Position = Domain.Models.Position.Freelancer,
                        Salary = 120000m
                    },
                    new Employee
                    {
                        Id = 3,
                        LastName = "Петров",
                        Position = Domain.Models.Position.Staff,
                        Salary = 1000m
                    }
                });

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<TimeLog> TimeLogs { get; set; }
        public DbSet<Employee> Employees { get; set; }
    }
}
