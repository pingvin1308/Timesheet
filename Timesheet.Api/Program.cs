using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Configuration;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;

namespace Timesheet.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile(
                    $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
                    optional: true)
                .Build();

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithExceptionDetails()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            try
            {
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal($"Failed to start {Assembly.GetExecutingAssembly().GetName().Name}"+" {Exception}", ex);
                Log.CloseAndFlush();
                throw;
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
                .UseSerilog((context, configuration) =>
                {
                    configuration
                        .Enrich.FromLogContext()
                        .Enrich.WithMachineName()
                        .ReadFrom.Configuration(context.Configuration);
                });
    }
}