using Microsoft.EntityFrameworkCore;
using Timesheet.Domain.Models;
using Employee = Timesheet.DataAccess.MSSQL.Entities.Employee;
using TimeLog = Timesheet.DataAccess.MSSQL.Entities.TimeLog;

namespace Timesheet.DataAccess.MSSQL
{
    public class TimesheetContext : DbContext
    {
        public TimesheetContext(DbContextOptions<TimesheetContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(new[]
            {
                new Employee
                {
                    Id = 1,
                    LastName = "Иванов",
                    Position = Position.Chef,
                    Salary = 200000m,
                    Bonus = 20000m,
                },
                new Employee
                {
                    Id=2,
                    LastName = "Сидоров",
                    Position = Position.Freelancer,
                    Salary = 1000m
                },
                new Employee
                {
                    Id=3,
                    LastName = "Петров",
                    Position = Position.Staff,
                    Salary = 120000m
                }
            });
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<TimeLog> TimeLogs { get; set; }
        public DbSet<Employee> Employees { get; set; }
    }
}