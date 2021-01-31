using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Timesheet.Domain;
using Timesheet.Domain.Models;

namespace Timesheet.DataAccess.csv
{
    public class TimesheetRepository : ITimesheetRepository
    {
        private readonly char _delimeter;
        private readonly string _path;

        public TimesheetRepository(CsvSettings csvSettings)
        {
            _delimeter = csvSettings.Delimeter;
            _path = csvSettings.Path + "\\timesheet.csv";
        }

        public void Add(TimeLog timeLog)
        {
            var dataRow = $"{timeLog.Comment}{_delimeter}" +
                $"{timeLog.Date}{_delimeter}" +
                $"{timeLog.Name}{_delimeter}" +
                $"{timeLog.WorkingHours}\n";

            File.AppendAllText(_path, dataRow);
        }

        public TimeLog[] GetTimeLogs(string lastName)
        {
            var data = File.ReadAllText(_path);
            var dataRows = data.Split(new char[] {'\n'}, StringSplitOptions.RemoveEmptyEntries);
            var timeLogs = new List<TimeLog>();             
            
            foreach (var dataRow in dataRows)
            {

                if (dataRow.Contains(lastName))
                {
                    var timeLog = new TimeLog();

                    var dataMembers = dataRow.Split(_delimeter);

                    timeLog.Comment = dataMembers[0];
                    timeLog.Date = DateTime.TryParse(dataMembers[1], out var date) ? date : new DateTime();
                    timeLog.Name = dataMembers[2];
                    timeLog.WorkingHours = int.TryParse(dataMembers[3], out var workingHours) ? workingHours : 0;

                    timeLogs.Add(timeLog);
                }
            }

            return timeLogs.ToArray();
        }
    }
}
