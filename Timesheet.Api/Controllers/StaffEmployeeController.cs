using Microsoft.AspNetCore.Mvc;
using Timesheet.Domain;
using Timesheet.Domain.Models;

namespace Timesheet.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffEmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        public StaffEmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpPost]
        public ActionResult<bool> Add(StaffEmployee staffEmployee)
        {
            return Ok(_employeeService.Add(staffEmployee));
        }
    }
}
