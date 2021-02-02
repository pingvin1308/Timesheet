using System.Linq;
using Timesheet.Domain;
using Timesheet.Domain.Models;

namespace Timesheet.BussinessLogic.Services
{
    public class ReportService : IReportService
    {
        private const decimal MAX_WORKING_HOURS_PER_MONTH = 160;
        private const decimal MAX_WORKING_HOURS_PER_DAY = 8;
        private const decimal BONUS_FOR_MANAGER = 20000;

        private readonly ITimesheetRepository _timesheetRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public ReportService(ITimesheetRepository timesheetRepository, IEmployeeRepository employeeRepository)
        {
            _timesheetRepository = timesheetRepository;
            _employeeRepository = employeeRepository;
        }

        public EmployeeReport GetEmployeeReport(string lastName)
        {
            var employee = _employeeRepository.Get(lastName);
            var timeLogs = _timesheetRepository.GetTimeLogs(employee.LastName);

            if (timeLogs == null || timeLogs.Length == 0)
            {
                return new EmployeeReport
                {
                    LastName = employee.LastName
                };
            }

            var totalHours = timeLogs.Sum(x => x.WorkingHours);
            var bill = employee.CalculateBill(timeLogs);

            return new EmployeeReport
            {
                LastName = employee.LastName,
                TimeLogs = timeLogs.ToList(),
                Bill = bill,
                TotalHours = totalHours,
                StartDate = timeLogs.Select(t => t.Date).Min(),
                EndDate = timeLogs.Select(t => t.Date).Max()
            };
        }
    }
}
