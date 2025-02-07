#nullable enable
namespace Tests;

using System.Net.Http;
using Api;
using Byndyusoft.ApiClient;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Options;
using Infrastructure;

public abstract class MvcTestFixture
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;
    protected IOptions<ApiClientSettings> _clientSettings = new OptionsWrapper<ApiClientSettings>(
        new ApiClientSettings
        {
            ConnectionString = "http://localhost:5000"
        }
    );

    protected MvcTestFixture(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient(
            new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            }
        );
    }

    protected HttpClient Client => _client;
}