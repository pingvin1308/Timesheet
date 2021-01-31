using System;
using System.Collections.Generic;
using System.Text;

namespace Timesheet.Domain.Models
{
    public abstract class Employee
    {
        protected const decimal MAX_WORKING_HOURS_PER_MONTH = 160;
        protected const decimal MAX_WORKING_HOURS_PER_DAY = 8;

        public Employee(string lastname, decimal salary, Position position)
        {
            LastName = lastname;
            Salary = salary;
            Position = position;
        }

        public string LastName { get; set; }
        public decimal Salary { get; set; }

        public Position Position { get; set; }

        public abstract decimal CalculateBill(TimeLog[] timeLogs);
        public abstract string GetPersonalData(char delimeter);

        public virtual bool CheckInputLog(TimeLog timeLog)
        {
            return true;
        }
    }
}
