#nullable enable
namespace Benchmark;

using System;
using System.Net.Http;
using Api.Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Logging;

public abstract class MvcTestFixture : IDisposable
{
    private HttpClient? _client;

    protected MvcTestFixture()
    {
        var factory = new WebApplicationFactory<Api.Program>();
        _client = factory
            .WithWebHostBuilder(
                builder =>
                {
                    builder.UseTestServer();
                    builder.ConfigureLogging(loggingConfig => loggingConfig.ClearProviders());
                    builder.UseEnvironment(BenchmarkEnvironment.Name);
                }
            )
            .CreateClient(
                new WebApplicationFactoryClientOptions
                          {
                              AllowAutoRedirect = false
                          }
        );
    }

    protected HttpClient Client => _client;

    public virtual void Dispose()
    {
        _client?.Dispose();
        _client = null;

        GC.SuppressFinalize(this);
    }
}
