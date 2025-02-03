namespace Byndyusoft.ApiClient.Example.Server;

using Infrastructure.OpenTelemetryExtensions;
using Logging.Builders;
using Logging.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Npgsql;
using Serilog;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args)
            .Build()
            .Run();
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        var serviceName = typeof(Program).Assembly.GetName().Name!;
        return Host.CreateDefaultBuilder(args)
            .UseSerilog(
                (context, configuration) => configuration
                    .UseDefaultSettings(context.Configuration)
                    .UseOpenTelemetryTraces()
                    .WriteToOpenTelemetry(activityEventBuilder: StructuredActivityEventBuilder.Instance)
            )
            .ConfigureServices(
                (context, services) =>
                    services.AddOpenTelemetry(
                        serviceName,
                        context.Configuration.GetSection("OtlpExporterOptions").Bind,
                        builder => builder.AddNpgsql()
                    )
            )
            .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
    }
}