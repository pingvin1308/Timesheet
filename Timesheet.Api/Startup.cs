using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Timesheet.Api.Models;
using Timesheet.BussinessLogic.Services;
using Timesheet.DataAccess.csv;
using Timesheet.DataAccess.MSSQL;
using Timesheet.Domain;
using Timesheet.Integrations.GitHub;

namespace Timesheet.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(ApiMappingProfile), typeof(DataAccessMappingProfile));

            services.AddTransient<IValidator<CreateTimeLogRequest>, TimeLogFluentValidator>();

            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<ITimesheetRepository, TimesheetRepository>();
            services.AddTransient<ITimeSheetService, TimesheetService>();
            services.AddTransient<IEmployeeRepository, DataAccess.MSSQL.Repositories.EmployeeRepository>();
            services.AddTransient<IEmployeeService, EmployeeService>();
            services.AddTransient<IReportService, ReportService>();
            services.AddTransient<IIssuesService, IssuesService>();

            services.AddTransient<IIssuesClient>(x => new IssuesClient("token"));

            services.AddSingleton(x => new CsvSettings(';', "..\\Timesheet.DataAccess.csv\\Data"));

            services.AddDbContext<TimesheetContext>(x => 
                x.UseSqlServer(Configuration.GetConnectionString("TimesheetContext")));

            services.AddControllers().AddFluentValidation();
            services.AddControllers().AddNewtonsoftJson();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            
            app.UseAuthorization();

            app.UseMiddleware<JwtAuthMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
