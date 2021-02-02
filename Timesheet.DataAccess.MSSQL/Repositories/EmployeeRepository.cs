using System;
using System.Linq;
using AutoMapper;
using Timesheet.Domain;
using Timesheet.Domain.Models;

namespace Timesheet.DataAccess.MSSQL.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly TimesheetContext _context;
        private readonly IMapper _mapper;

        public EmployeeRepository(TimesheetContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void Add(Employee employee)
        {
            var newEmployee = _mapper.Map<Entities.Employee>(employee);


            _context.Employees.Add(newEmployee);
            _context.SaveChanges();
        }

        public Employee Get(string lastName)
        {
            var employee = _context.Employees.FirstOrDefault(e => e.LastName.ToLower() == lastName.ToLower());

            if (employee == null)
            {
                return null;
            }

            switch (employee.Position)
            {
                case Position.Chef:
                    return _mapper.Map<ChiefEmployee>(employee);
                case Position.Staff:
                    return new StaffEmployee(employee.LastName, employee.Salary);
                case Position.Freelancer:
                    return new FreelancerEmployee(employee.LastName, employee.Salary);
                default:
                    throw new Exception("WrongPosition " + employee.Position);
            }
        }
    }
}