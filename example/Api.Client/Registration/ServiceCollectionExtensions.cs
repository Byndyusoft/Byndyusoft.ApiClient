namespace Api.Client.Registration
{
    using System.Net.Http.Json.Formatting;
    using Byndyusoft.ApiClient;
    using Contracts;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;

    public static class ServiceCollectionExtension
    {
        /// <summary>
        ///     Для работы клиенты необходимо указать настройки подключения к апи в appsetting
        /// </summary>
        public static void AddPersonClient(this IServiceCollection services, IConfiguration configuration)
        {
            var settingsSection = configuration.GetSection(nameof(ApiClientSettings));
            var settings = settingsSection.Get<ApiClientSettings>();
            
            services
                .AddOptions()
                .Configure<ApiClientSettings>(settingsSection);

            services.AddHttpClient<IPersonApi, PersonApi>(
                client =>
                    new PersonApi(
                        client,
                        Options.Create<ApiClientSettings>(settings),
                        new JsonMediaTypeFormatter()
                    )
            );
        }
    }
}