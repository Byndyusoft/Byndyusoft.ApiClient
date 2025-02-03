namespace Byndyusoft.ApiClient.Example.Server
{
    using System;
    using System.Net.Http.ProtoBuf;
    using Infrastructure.Swagger;
    using Infrastructure.Versioning;
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
                .AddEndpointsApiExplorer();
            
            services
                .AddSwaggerGen()
                .AddVersioning()
                .AddSwagger();

            services
                .AddMvcCore()
                .AddProtoBufNet(options => { options.Model = ProtoBufDefaults.TypeModel; })
                .AddMessagePackFormatters()
                .AddFormatterMappings()
                .AddTracing();
            
            services.AddControllers();
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