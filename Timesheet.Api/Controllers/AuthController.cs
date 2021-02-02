using System;
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
                return Ok(_authService.Login(request.LastName));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}