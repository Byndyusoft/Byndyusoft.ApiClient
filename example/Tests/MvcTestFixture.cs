#nullable enable
namespace Byndyusoft.ApiClient.Example.Tests;

using System;
using System.Net.Http;
using Server;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

public abstract class MvcTestFixture : IDisposable
{
    private HttpClient? _client;
    protected IHost? _host;
    protected IOptions<ApiClientSettings> _clientSettings = new OptionsWrapper<ApiClientSettings>(
        new ApiClientSettings
        {
            ConnectionString = "http://localhost:5000"
        }
    );

    protected MvcTestFixture()
    {
        _host = Host
            .CreateDefaultBuilder()
            .ConfigureWebHostDefaults(
                webBuilder =>
                {
                    webBuilder.UseTestServer();
                    webBuilder.UseStartup<Startup>();
                }
            )
            .Build();
        _host.Start();
    }

    protected HttpClient Client
    {
        get
        {
            if (_client == null)
                _client = _host!.GetTestClient();
            
            return _client;
        }
    }

    public virtual void Dispose()
    {
        _host?.Dispose();
        _host = null;

        _client?.Dispose();
        _client = null;

        GC.SuppressFinalize(this);
    }
}