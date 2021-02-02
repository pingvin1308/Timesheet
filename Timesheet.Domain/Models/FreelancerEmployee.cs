using System;
using System.Linq;

namespace Timesheet.Domain.Models
{
    public class FreelancerEmployee : Employee
    {
        public FreelancerEmployee(string lastName, decimal salary) : base(lastName, salary, Position.Freelancer)
        {
        }

        public override decimal CalculateBill(TimeLog[] timeLogs)
        {
            var totalHours = timeLogs.Sum(x => x.WorkingHours);
            var bill = totalHours * Salary;
            return bill;
        }

        public override string GetPersonalData(char delimeter)
        {
            return $"{LastName}{delimeter}{Salary}{delimeter}Фрилансер{delimeter}\n";
        }

        public override bool CheckInputLog(TimeLog timeLog)
        {
            var isValid = base.CheckInputLog(timeLog);
            isValid = timeLog.LastName == this.LastName && timeLog.Date > DateTime.Now.AddDays(-2) && isValid;
            return isValid;
        }
    }
}
