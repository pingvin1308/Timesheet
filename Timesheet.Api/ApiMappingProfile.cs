using AutoMapper;
using Timesheet.Api.Models;
using Timesheet.Domain.Models;

namespace Timesheet.Api
{
    public class ApiMappingProfile: Profile
    {
        public ApiMappingProfile()
        {
            CreateMap<CreateTimeLogRequest, TimeLog>();
        }
    }
}