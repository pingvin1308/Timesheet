using System;

namespace Timesheet.BussinessLogic.Exceptions
{
    public class NotFoundException: Exception
    {
        public NotFoundException(string message):base(message)
        { }
    }
}