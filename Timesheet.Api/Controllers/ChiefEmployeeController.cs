using Microsoft.AspNetCore.Mvc;
using Timesheet.Domain;
using Timesheet.Domain.Models;

namespace Timesheet.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChiefEmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        public ChiefEmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }
       
        [HttpPost]
        public ActionResult<bool> Add(ChiefEmployee ChiefEmployee)
        {
            return Ok(_employeeService.Add(ChiefEmployee));
        }
    }
}
