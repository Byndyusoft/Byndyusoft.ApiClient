namespace Api.Infrastructure.Swagger
{
    using System;
    using System.IO;
    using System.Reflection;
    using Asp.Versioning.ApiExplorer;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;

    /// <summary>
    ///     ConfigureSwaggerOptions
    /// </summary>
    public class ConfigureSwaggerOptions(IApiVersionDescriptionProvider apiVersionDescriptionProvider) : IConfigureOptions<SwaggerGenOptions>
    {
        /// <inheritdoc />
        public void Configure(SwaggerGenOptions options)
        {
            foreach (var apiVersionDescription in apiVersionDescriptionProvider.ApiVersionDescriptions)
                options.SwaggerDoc(
                    apiVersionDescription.GroupName,
                    new OpenApiInfo
                    {
                        Version = apiVersionDescription.ApiVersion.ToString(),
                        Description = apiVersionDescription.IsDeprecated ? "DEPRECATED" : "",
                        Title = Assembly.GetExecutingAssembly().GetName().Name
                    }
                );
            var xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml");
            foreach (var xmlFile in xmlFiles)
                options.IncludeXmlComments(xmlFile);
        }
    }
}