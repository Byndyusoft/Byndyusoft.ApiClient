#nullable enable
namespace Byndyusoft.ApiClient.Functional;

using System;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Reflection;
using Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public abstract class MvcTestFixture : IDisposable
{
    protected readonly string URL;
    private HttpClient? _client;
    protected IHost? _host;

    protected MvcTestFixture()
    {
        URL = $"http://localhost:{FreeTcpPort()}";
        _host = Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(
                    webBuilder =>
                    {
                        webBuilder.UseUrls(URL);
                        webBuilder.UseTestServer();
                        webBuilder.ConfigureServices(ConfigureServices);
                        webBuilder.Configure(Configure);
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
            {
                _client = _host.GetTestClient();
                _client.BaseAddress = new Uri(URL);
                ConfigureHttpClient(_client);
            }

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

    public virtual void Configure(IApplicationBuilder app)
    {
        app.UseRouting();
        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }

    public virtual void ConfigureServices(IServiceCollection services)
    {
        services.AddLogging(c => c.ClearProviders());
        services.AddControllers();
        var assembly = Assembly.GetAssembly(typeof(SimpleModelController));
        ConfigureMvc(
            services
                .AddMvcCore()
                .AddApplicationPart(assembly!)
                .AddControllersAsServices()
            );
    }

    protected abstract void ConfigureMvc(IMvcCoreBuilder builder);

    protected virtual void ConfigureHttpClient(HttpClient client)
    {
    }

    private static int FreeTcpPort()
    {
        var listener = new TcpListener(IPAddress.Loopback, 0);
        listener.Start();
        var port = ((IPEndPoint) listener.LocalEndpoint).Port;
        listener.Stop();
        return port;
    }
}
