using Timesheet.Domain;
using Timesheet.Domain.Models;

namespace Timesheet.BussinessLogic.Services
{
  
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public bool Add(Employee employee)
        {
            bool isValid = !string.IsNullOrEmpty(employee.LastName) && employee.Salary > 0;

            if (isValid)
            {
                _employeeRepository.Add(employee);
            }

            return isValid;
        }
    }
}
