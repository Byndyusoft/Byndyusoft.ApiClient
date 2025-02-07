using System.Net.Http.ProtoBuf;
using Api.Contracts;
using Api.Infrastructure.OpenTelemetryExtensions;
using Api.Infrastructure.Swagger;
using Api.Infrastructure.Versioning;
using Byndyusoft.Logging.Builders;
using Byndyusoft.Logging.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;
using Serilog;

var serviceName = typeof(Program).Assembly.GetName().Name;
var builder = WebApplication.CreateBuilder(args);
if (builder.Environment.EnvironmentName != BenchmarkEnvironment.Name)
    builder.Host.UseSerilog(
        (context, configuration) =>
            configuration
                .UseDefaultSettings(context.Configuration)
                .UseOpenTelemetryTraces()
                .WriteToOpenTelemetry(activityEventBuilder: StructuredActivityEventBuilder.Instance)
    );

var services = builder.Services;
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddOpenTelemetry(
    serviceName,
    builder.Configuration.GetSection("OtlpExporterOptions").Bind,
    builder => builder.AddNpgsql()
);
services
    .AddMvcCore()
    .AddProtoBufNet(options => { options.Model = ProtoBufDefaults.TypeModel; })
    .AddMessagePackFormatters()
    .AddFormatterMappings()
    .AddTracing();
services
    .AddRouting(options => options.LowercaseUrls = true);
services.AddHealthChecks();
services
    .AddVersioning()
    .AddSwagger();

var app = builder.Build();
app
    .UseHealthChecks("/healthz")
    .UseOpenTelemetryPrometheusScrapingEndpoint()
    .UseRouting()
    .UseEndpoints(endpoints => endpoints.MapControllers());

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();

// For tests accessibility
namespace Api
{
    public partial class Program { } 
}