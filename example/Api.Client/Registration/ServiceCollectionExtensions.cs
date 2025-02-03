namespace Byndyusoft.ApiClient.Example.Client.Registration
{
    using System.Net.Http.Formatting;
    using System.Net.Http.Json.Formatting;
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
            services
                .AddOptions()
                .Configure<ApiClientSettings>(configuration.GetSection(nameof(ApiClientSettings)));

            services.AddHttpClient<IPersonApi, PersonApi>(
                client=>
                    new PersonApi(
                        client,
                        configuration.Get<IOptions<ApiClientSettings>>()!,
                        new JsonMediaTypeFormatter()
                    )
            );
        }
    }
}