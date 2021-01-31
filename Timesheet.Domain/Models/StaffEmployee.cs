using System.Linq;

namespace Timesheet.Domain.Models
{
    public class StaffEmployee : Employee
    {
        public StaffEmployee(string lastName, decimal salary) : base(lastName, salary, Position.Staff)
        {
        }

        public override decimal CalculateBill(TimeLog[] timeLogs)
        {
            var totalHours = timeLogs.Sum(x => x.WorkingHours);
            decimal bill = 0;
            var workingHoursGroupsByDay = timeLogs
                                            .GroupBy(x => x.Date.ToShortDateString());

            foreach (var workingLogsPerDay in workingHoursGroupsByDay)
            {
                int dayHours = workingLogsPerDay.Sum(x => x.WorkingHours);

                if (dayHours > MAX_WORKING_HOURS_PER_DAY)
                {
                    var overtime = dayHours - MAX_WORKING_HOURS_PER_DAY;

                    bill += MAX_WORKING_HOURS_PER_DAY / MAX_WORKING_HOURS_PER_MONTH * Salary;
                    bill += overtime / MAX_WORKING_HOURS_PER_MONTH * Salary * 2;
                }
                else
                {
                    bill += dayHours / MAX_WORKING_HOURS_PER_MONTH * Salary;
                }
            }

            return bill;
        }

        public override string GetPersonalData(char delimeter)
        {
            return $"{LastName}{delimeter}{Salary}{delimeter}Штатный сотрудник{delimeter}\n";
        }

        public override bool CheckInputLog(TimeLog timeLog)
        {
            bool isValid = base.CheckInputLog(timeLog);
            isValid = timeLog.Name == this.LastName && isValid;
            return isValid;
        }
    }
}
