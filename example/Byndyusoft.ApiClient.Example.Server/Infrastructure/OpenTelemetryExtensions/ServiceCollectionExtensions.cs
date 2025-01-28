namespace Byndyusoft.ApiClient.Example.Server.Infrastructure.OpenTelemetryExtensions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOpenTelemetry(
        this IServiceCollection services,
        string serviceName,
        IConfiguration configuration
    )
    {
        services
            .AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(serviceName))
            .WithTracing(
                builder =>
                    builder
                        .AddAspNetCoreInstrumentation(o => o.AddDefaultIgnorePatterns())
                        .AddJaegerExporter(
                            o =>
                            {
                                o.AgentHost = configuration.GetSection("Jaeger:JAEGER_AGENT_HOST").Value;
                                o.AgentPort = int.Parse(configuration.GetSection("Jaeger:JAEGER_AGENT_PORT").Value);
                            }
                        )
            )
            .WithMetrics(
                builder => builder
                    .AddAspNetCoreInstrumentation()
                    .AddPrometheusExporter()
            );
        return services;
    }
}