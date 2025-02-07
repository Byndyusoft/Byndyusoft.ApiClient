namespace Api.Infrastructure.OpenTelemetryExtensions;

using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOpenTelemetry(
        this IServiceCollection services,
        string? serviceName,
        Action<OtlpExporterOptions> configureOtlp,
        Action<TracerProviderBuilder>? configureBuilder = null,
        Action<MeterProviderBuilder>? configureMeter = null)
    {
        services
            .AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(serviceName))
            .WithTracing(
                builder =>
                {
                    builder
                        .AddAspNetCoreInstrumentation(o => o.AddDefaultIgnorePatterns())
                        .AddHttpClientInstrumentation()
                        .AddOtlpExporter(configureOtlp);
                    configureBuilder?.Invoke(builder);
                }
            )
            .WithMetrics(
                builder =>
                {
                    builder
                        .AddPrometheusExporter()
                        .AddRuntimeInstrumentation()
                        .AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation();
                    configureMeter?.Invoke(builder);
                }
            );
        return services;
    }
}