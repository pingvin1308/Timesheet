using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Trace;
using System.Diagnostics;

namespace OpenTelemetryConsoleTests
{
    class Program
    {
        private const string ACTIVITY_SOURCE_NAME = "TestActivity";
        private static readonly ActivitySource MyActivitySource = new ActivitySource(ACTIVITY_SOURCE_NAME);

        static void Main(string[] args)
        {
            using var traceProvider = Sdk
                .CreateTracerProviderBuilder()
                .SetSampler(new AlwaysOnSampler())
                .AddSource(ACTIVITY_SOURCE_NAME)
                .AddConsoleExporter()
                .Build();

            using (var activity = MyActivitySource.StartActivity("TestActivity"))
            {
                // start
                activity?.AddTag("number", 42);

                activity?.AddEvent(new ActivityEvent("test event"));

                // stage 1

                activity?.AddTag("string", "test message");
                activity?.AddBaggage("string", "test message");
                // stage 2

                // stage 3

                activity?.AddTag("array", new[] {1,2,34,4,56 });

                // end
            }
        }

        private static void TestLogging()
        {
            var serviceCollection = new ServiceCollection()
                .AddLogging(builder =>
                {
                    builder.AddOpenTelemetry(options => options.AddConsoleExporter());
                });

            var serviceProvider = serviceCollection.BuildServiceProvider();
            var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

            logger.LogInformation("Test information");
            logger.LogError("Test error");
            logger.LogDebug("Test debug");
            logger.LogWarning("Test warning");
            logger.LogTrace("Test trace");
        }
    }
}
