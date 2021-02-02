using Timesheet.Domain.Models;

namespace Timesheet.Domain
{
    public interface IEmployeeRepository
    {
        Employee Get(string lastName);
        void Add(Employee staffEmployee);
    }
}
