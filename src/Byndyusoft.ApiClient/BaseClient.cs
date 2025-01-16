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

        protected Task<TResult> GetAsync<TResult>(string url, CancellationToken cancellationToken) => 
            CallAsync<TResult>(HttpMethod.Get, url, null, cancellationToken);

        protected async Task<TResult> GetAsync<TParams, TResult>(string url, CancellationToken cancellationToken, TParams? dto = null)
            where TParams : class
        {
            var httpQuery = dto != null
                ? $"{url}?{HttpGetParamsBuilder.Build(dto)}"
                : url;

            return await CallAsync<TResult>(HttpMethod.Get, httpQuery, null, cancellationToken);
        }

        protected Task PostAsync(string url, object content, CancellationToken cancellationToken) =>
            CallAsync(HttpMethod.Post, url, content, cancellationToken);

        protected Task<TResult> PostAsync<TResult>(string url, object content, CancellationToken cancellationToken) =>
            CallAsync<TResult>(HttpMethod.Post, url, content, cancellationToken);

        protected Task<TResult> PutAsync<TResult>(string url, object content, CancellationToken cancellationToken) =>
            CallAsync<TResult>(HttpMethod.Put, url, content, cancellationToken);

        protected Task PutAsync(string url, object content, CancellationToken cancellationToken) =>
            CallAsync(HttpMethod.Put, url, content, cancellationToken);

        protected Task<TResult> PatchAsync<TResult>(string url, object content, CancellationToken cancellationToken) =>
            CallAsync<TResult>(new HttpMethod("PATCH"), url, content, cancellationToken);

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
            var response = await CallAsyncBase(method, url, content, cancellationToken).ConfigureAwait(false);
            return await response.Content.ReadAsJsonAsync<TResult>();
        }

        protected async Task CallAsync(HttpMethod method, string url, object? content, CancellationToken cancellationToken)
        {
            await CallAsyncBase(method, url, content, cancellationToken);
        }

        private async Task<HttpResponseMessage> CallAsyncBase(HttpMethod method, string url, object? content, CancellationToken cancellationToken)
        {
            var requestMessage
                = new HttpRequestMessage
                  {
                      Method = method,
                      RequestUri = new Uri(GetAbsoluteUrl(url), UriKind.RelativeOrAbsolute),
                  };
            if(content != null)
                requestMessage.Content = HttpContentExtensions.PrepareHttpContent(content);

            var response = await Client.SendAsync(requestMessage, cancellationToken).ConfigureAwait(false);

            await Toolkit.EnsureSuccessStatusCode(response);
            return response;
        }
    }
}