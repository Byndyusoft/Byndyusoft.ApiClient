namespace Byndyusoft.ApiClient
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading;
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

        protected async Task<TResult> GetAsync<TResult>(string url, CancellationToken cancellationToken)
        {
            var response = await _client.GetAsync(GetAbsoluteUrl(url), cancellationToken).ConfigureAwait(false);
            
            await EnsureSuccessStatusCode(response);

            return await response.Content.ReadAsJsonAsync<TResult>();
        }

        protected async Task<TResult> GetAsync<TParams, TResult>(string url, Dictionary<string, string>? headers = null, CancellationToken cancellationToken = default, TParams? dto = null)
            where TParams : class
        {
            var endpoint = GetAbsoluteUrl(url);
            var httpQuery = dto != null
                ? $"{endpoint}?{HttpGetParamsBuilder.Build(dto)}"
                : endpoint;

            var response = await _client.GetAsync(httpQuery, cancellationToken).ConfigureAwait(false);

            await EnsureSuccessStatusCode(response);

            return await response.Content.ReadAsJsonAsync<TResult>();
        }

        protected Task PostAsync(string url, object content, Dictionary<string, string>? headers = null, CancellationToken cancellationToken = default) =>
            CallAsync(HttpMethod.Post, url, content, headers, cancellationToken);

        protected Task<TResult> PostAsync<TResult>(string url, object content, Dictionary<string, string>? headers = null, CancellationToken cancellationToken = default) =>
            CallAsync<TResult>(HttpMethod.Post, url, content, headers, cancellationToken);

        protected Task<TResult> PutAsync<TResult>(string url, object content, Dictionary<string, string>? headers = null, CancellationToken cancellationToken = default) =>
            CallAsync<TResult>(HttpMethod.Put, url, content, headers, cancellationToken);

        protected Task PatchAsync(string url, object content, Dictionary<string, string>? headers = null, CancellationToken cancellationToken = default) =>
            CallAsync(new HttpMethod("PATCH"), url, content, headers, cancellationToken);

        protected Task DeleteAsync(string url, Dictionary<string, string>? headers = null, CancellationToken cancellationToken = default) =>
            CallAsync(HttpMethod.Delete, url, null, headers, cancellationToken);

        protected Task DeleteAsync<TParams>(string url, TParams parameters, Dictionary<string, string>? headers, CancellationToken cancellationToken) =>
            CallAsync(HttpMethod.Delete, url, parameters, headers, cancellationToken);

        private string GetAbsoluteUrl(string url)
        {
            return $"{_apiSettings.ConnectionString}{url}";
        }

        private async Task<TResult> CallAsync<TResult>(HttpMethod method, string url, object? content, Dictionary<string, string>? headers, CancellationToken cancellationToken)
        {
            var requestMessage
                = new HttpRequestMessage
                  {
                      Method = method,
                      RequestUri = new Uri(GetAbsoluteUrl(url), UriKind.RelativeOrAbsolute),
                      Content = HttpContentExtensions.PrepareHttpContent(content)
                  };

            if (headers != null) 
                requestMessage.Headers.AddRange(headers);

            var response = await _client.SendAsync(requestMessage, cancellationToken);

            await EnsureSuccessStatusCode(response);

            return await response.Content.ReadAsJsonAsync<TResult>();
        }

        private async Task CallAsync(HttpMethod method, string url, object? content, Dictionary<string, string>? headers, CancellationToken cancellationToken)
        {
            var requestMessage
                = new HttpRequestMessage
                  {
                      Method = method,
                      RequestUri = new Uri(GetAbsoluteUrl(url), UriKind.RelativeOrAbsolute),
                      Content = HttpContentExtensions.PrepareHttpContent(content)
                  };

            if (headers != null)
                requestMessage.Headers.AddRange(headers);

            var response = await _client.SendAsync(requestMessage, cancellationToken).ConfigureAwait(false);

            await EnsureSuccessStatusCode(response);
        }

        private async Task EnsureSuccessStatusCode(HttpResponseMessage response)
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