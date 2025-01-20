using System.Net.Http;
using System.Net.Http.Json.Formatting;
using Asp.Versioning;
using Byndyusoft.ApiClient;
using Byndyusoft.ApiClient.Client;
using Byndyusoft.ApiClient.Contracts;
using Byndyusoft.ApiClient.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services
    .AddApiVersioning(
        options =>
        {
            options.DefaultApiVersion = ApiVersion.Default;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
        }
    )
    .AddApiExplorer(
        options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        }
    );

services.AddSwagger();

services
    .AddMvcCore()
    .AddProtoBufFormatters()
    .AddMessagePackFormatters()
    .AddFormatterMappings();
services.AddControllers();

services
    .AddOptions()
    .Configure<ApiClientSettings>(builder.Configuration.GetSection(nameof(ApiClientSettings)));

services.AddHttpClient<ISimpleModelClient, TestFormatterSimpleModelClient>(
    client =>
        new TestFormatterSimpleModelClient(
            client,
            Options.Create(
                new ApiClientSettings
                {
                    ConnectionString = client.BaseAddress?.AbsolutePath ?? "http://localhost:5000"
                }
            )
        )
    );

var app = builder.Build();

if (builder.Environment.IsProduction() == false)
    app.UseSwaggerWithApiVersionDescriptionProvider();

app.UseRouting();
app.MapControllers();

app.Run();