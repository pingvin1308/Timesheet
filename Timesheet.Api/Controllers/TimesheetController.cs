using System;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog;
using Serilog.Context;
using Timesheet.Api.Models;
using Timesheet.Domain;
using Timesheet.Domain.Models;

namespace Timesheet.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimesheetController : Controller
    {
        private readonly ITimeSheetService _timeSheetService;
        private readonly IMapper _mapper;
        private readonly ILogger<TimesheetController> _logger;

        public TimesheetController(ITimeSheetService timeSheetService,
            IMapper mapper,
            ILogger<TimesheetController> logger)
        {
            _timeSheetService = timeSheetService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost]
        public ActionResult<bool> TrackTime(CreateTimeLogRequest request)
        {
            _logger.LogInformation("Пользователь фиксирует рабочее время {@Request}", request);

            var lastName = (string)HttpContext.Items["LastName"];

            if (ModelState.IsValid)
            {
                var timeLog = _mapper.Map<TimeLog>(request);

                var result = _timeSheetService.TrackTime(timeLog, lastName);
                _logger.LogInformation("Пользователь успешно зафиксировал время");
                return Ok(result);
            }

            return BadRequest();
        }
    }
}
