namespace Byndyusoft.ApiClient.Example.Server
{
    using System;
    using Asp.Versioning;
    using ApiClient;
    using Client;
    using Contracts;
    using Infrastructure.Swagger;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public void ConfigureServices(IServiceCollection services)
        {
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
                .Configure<ApiClientSettings>(_configuration.GetSection(nameof(ApiClientSettings)));

            services.AddHttpClient<IPersonModelListClient, PersonModelListClient>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment webHostEnvironment)
        {
            if (webHostEnvironment.IsDevelopment())
            {
                app.UseSwaggerWithApiVersionDescriptionProvider();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}