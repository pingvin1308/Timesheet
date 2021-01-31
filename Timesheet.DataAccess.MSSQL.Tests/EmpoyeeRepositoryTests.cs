using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using Timesheet.DataAccess.MSSQL.Repositories;
using AutoMapper;

namespace Timesheet.DataAccess.MSSQL.Tests
{
    public class EmpoyeeRepositoryTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Get()
        {
            var contextOptions = new DbContextOptionsBuilder<TimesheetContext>()
                .UseSqlServer("Server=.\\SQLEXPRESS;Database=TimesheetDB;Trusted_Connection=True;")
                .Options;

            var context = new TimesheetContext(contextOptions);

            var configuration = new MapperConfiguration(x => x.AddProfile<DataAccessMappingProfile>());
            var mapper = new Mapper(configuration);

            var repository = new EmployeeRepository(context, mapper);
            var employee = repository.Get("Иванов");
        }
    }
}