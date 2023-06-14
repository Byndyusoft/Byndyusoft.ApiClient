namespace Byndyusoft.ApiClient
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;

    public class BaseClient
    {
        private readonly ApiClientSettings _apiSettings;
        private readonly HttpClient _client;

        protected BaseClient(HttpClient client, IOptions<ApiClientSettings> apiSettings)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _apiSettings = apiSettings.Value ?? throw new ArgumentNullException(nameof(apiSettings));
        }

        protected async Task<TResult> GetAsync<TResult>(string url)
        {
            var response = await _client.GetAsync(GetAbsoluteUrl(url)).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsJsonAsync<TResult>();
        }

        protected async Task<TResult> GetAsync<TParams, TResult>(string url, TParams? dto = null)
            where TParams : class
        {
            var endpoint = GetAbsoluteUrl(url);
            var httpQuery = dto != null
                ? $"{endpoint}?{HttpGetParamsBuilder.Build(dto)}"
                : endpoint;

            var response = await _client.GetAsync(httpQuery).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsJsonAsync<TResult>();
        }

        protected Task PostAsync(string url, object content) =>
            CallAsync(HttpMethod.Post, url, content);

        protected Task<TResult> PostAsync<TResult>(string url, object content) =>
            CallAsync<TResult>(HttpMethod.Post, url, content);

        protected Task<TResult> PutAsync<TResult>(string url, object content) =>
            CallAsync<TResult>(HttpMethod.Put, url, content);

        protected  Task<HttpResponseMessage> PatchAsync<TResult>(string url, object content)
        {
            var requestMessage
                = new HttpRequestMessage
                  {
                      Method = new HttpMethod("PATCH"),
                      RequestUri = new Uri(GetAbsoluteUrl(url), UriKind.RelativeOrAbsolute),
                      Content = HttpContentExtensions.PrepareHttpContent(content)
                  };
            
            return _client.SendAsync(requestMessage);
        }
        
        protected Task DeleteAsync(string url) =>
            CallAsync(HttpMethod.Delete, url, null);

        protected Task DeleteAsync<TParams>(string url, TParams parameters) =>
            CallAsync(HttpMethod.Delete, url, parameters);

        private string GetAbsoluteUrl(string url)
        {
            return $"{_apiSettings.ConnectionString}{url}";
        }

        private async Task<TResult> CallAsync<TResult>(HttpMethod method, string url, object? content)
        {
            var requestMessage
                = new HttpRequestMessage
                  {
                      Method = method,
                      RequestUri = new Uri(GetAbsoluteUrl(url), UriKind.RelativeOrAbsolute),
                      Content = HttpContentExtensions.PrepareHttpContent(content)
                  };

            var response = await _client.SendAsync(requestMessage);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsJsonAsync<TResult>();
        }

        private async Task CallAsync(HttpMethod method, string url, object? content)
        {
            var requestMessage
                = new HttpRequestMessage
                  {
                      Method = method,
                      RequestUri = new Uri(GetAbsoluteUrl(url), UriKind.RelativeOrAbsolute),
                      Content = HttpContentExtensions.PrepareHttpContent(content)
                  };

            var response = await _client.SendAsync(requestMessage).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
        }
    }
}