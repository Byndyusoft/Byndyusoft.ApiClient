namespace Byndyusoft.ApiClient
{
    using System;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;

    public class BaseClient
    {
        protected readonly ApiClientSettings ApiSettings;
        protected readonly HttpClient Client;

        protected BaseClient(HttpClient client, IOptions<ApiClientSettings> apiSettings)
        {
            Client = client ?? throw new ArgumentNullException(nameof(client));
            ApiSettings = apiSettings.Value ?? throw new ArgumentNullException(nameof(apiSettings));
        }

        protected async Task<TResult> GetAsync<TResult>(string url, CancellationToken cancellationToken)
        {
            var response = await Client.GetAsync(GetAbsoluteUrl(url), cancellationToken).ConfigureAwait(false);
            
            await EnsureSuccessStatusCode(response);

            return await response.Content.ReadAsJsonAsync<TResult>();
        }

        protected async Task<TResult> GetAsync<TParams, TResult>(string url, CancellationToken cancellationToken, TParams? dto = null)
            where TParams : class
        {
            var endpoint = GetAbsoluteUrl(url);
            var httpQuery = dto != null
                ? $"{endpoint}?{HttpGetParamsBuilder.Build(dto)}"
                : endpoint;

            var response = await Client.GetAsync(httpQuery, cancellationToken).ConfigureAwait(false);

            await EnsureSuccessStatusCode(response);

            return await response.Content.ReadAsJsonAsync<TResult>();
        }

        protected Task PostAsync(string url, object content, CancellationToken cancellationToken) =>
            CallAsync(HttpMethod.Post, url, content, cancellationToken);

        protected Task<TResult> PostAsync<TResult>(string url, object content, CancellationToken cancellationToken) =>
            CallAsync<TResult>(HttpMethod.Post, url, content, cancellationToken);

        protected Task<TResult> PutAsync<TResult>(string url, object content, CancellationToken cancellationToken) =>
            CallAsync<TResult>(HttpMethod.Put, url, content, cancellationToken);

        protected Task PatchAsync(string url, object content, CancellationToken cancellationToken) =>
            CallAsync(new HttpMethod("PATCH"), url, content, cancellationToken);

        protected Task DeleteAsync(string url, CancellationToken cancellationToken) =>
            CallAsync(HttpMethod.Delete, url, null, cancellationToken);

        protected Task DeleteAsync<TParams>(string url, TParams parameters, CancellationToken cancellationToken) =>
            CallAsync(HttpMethod.Delete, url, parameters, cancellationToken);

        protected string GetAbsoluteUrl(string url)
        {
            return $"{ApiSettings.ConnectionString}{url}";
        }

        protected async Task<TResult> CallAsync<TResult>(HttpMethod method, string url, object? content, CancellationToken cancellationToken)
        {
            var requestMessage
                = new HttpRequestMessage
                  {
                      Method = method,
                      RequestUri = new Uri(GetAbsoluteUrl(url), UriKind.RelativeOrAbsolute),
                      Content = HttpContentExtensions.PrepareHttpContent(content)
                  };

            var response = await Client.SendAsync(requestMessage, cancellationToken);

            await EnsureSuccessStatusCode(response);

            return await response.Content.ReadAsJsonAsync<TResult>();
        }

        protected async Task CallAsync(HttpMethod method, string url, object? content, CancellationToken cancellationToken)
        {
            var requestMessage
                = new HttpRequestMessage
                  {
                      Method = method,
                      RequestUri = new Uri(GetAbsoluteUrl(url), UriKind.RelativeOrAbsolute),
                      Content = HttpContentExtensions.PrepareHttpContent(content)
                  };

            var response = await Client.SendAsync(requestMessage, cancellationToken).ConfigureAwait(false);

            await EnsureSuccessStatusCode(response);
        }

        protected async Task EnsureSuccessStatusCode(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode == false)
            {
                var stringContent = await response.Content.ReadAsStringAsync();
                response.Content.Dispose();

                throw new HttpRequestWithContentException(
                    message: $"Error occurred on sending a request. Status code: {(int)response.StatusCode} - {response.StatusCode.ToString()}. Message: {response.ReasonPhrase}",
                    statusCode: response.StatusCode,
                    content: stringContent);
            }
        }
    }
}