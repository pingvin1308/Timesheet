using Microsoft.AspNetCore.Mvc;
using Timesheet.Domain;
using Timesheet.Domain.Models;

namespace Timesheet.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FreelancerEmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        public FreelancerEmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpPost]
        public ActionResult<bool> Add(FreelancerEmployee freelancerEmployee)
        {
            return Ok(_employeeService.Add(freelancerEmployee));
        }
    }
}
