using Microsoft.AspNetCore.Mvc;
using Timesheet.Api.ResourceModels;
using Timesheet.BussinessLogic.Exceptions;
using Timesheet.Domain;

namespace Timesheet.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public ActionResult<string> Login(LoginRequest request)
        {
            try
            {
                var token = _authService.Login(request.LastName);

                return Ok(token);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
    }
}