using AutoMapper;
using System;
using System.Linq;
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
            var employee = _context.Employees
                .FirstOrDefault(x => x.LastName.ToLower() == lastName.ToLower());

            switch (employee.Position)
            {
                case Position.Chef:
                    return _mapper.Map<ChiefEmployee>(employee);

                case Position.Staff:
                    return _mapper.Map<StaffEmployee>(employee);

                case Position.Freelancer:
                    return _mapper.Map<FreelancerEmployee>(employee);

                default:
                    throw new Exception("Wrong position: " + employee.Position);
            }
        }
    }
}
