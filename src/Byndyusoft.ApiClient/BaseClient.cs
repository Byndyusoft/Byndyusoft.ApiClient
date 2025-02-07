namespace Byndyusoft.ApiClient
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Net.Http.Headers;
    using System.Net.Http.Json;
    using System.Net.Http.Json.Formatting;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;

    public class BaseClient
    {
        protected readonly MediaTypeFormatter Formatter;
        protected readonly HttpClient Client;
        protected readonly ApiClientSettings ApiSettings;

        protected BaseClient(
            HttpClient client,
            IOptions<ApiClientSettings> apiSettings,
            MediaTypeFormatter? formatter = null
        )
        {
            Client = client ?? throw new ArgumentNullException(nameof(client));
            ApiSettings = apiSettings.Value ?? throw new ArgumentNullException(nameof(apiSettings));
            Formatter = formatter ?? new JsonMediaTypeFormatter(JsonDefaults.SerializerOptions);
            
            foreach (var mediaTypeHeaderValue in Formatter.SupportedMediaTypes)
                Client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue(mediaTypeHeaderValue.MediaType)
                );
        }
        
        protected async Task<TResult> GetAsync<TResult>(string url, CancellationToken cancellationToken) =>
            await CallAsync<TResult>(HttpMethod.Get, url, null, cancellationToken);

        protected async Task<TResult> GetAsync<TParams, TResult>(string url, TParams? parameters, CancellationToken cancellationToken)
            where TParams : class
        {
            var httpQuery = parameters != null
                ? $"{url}?{HttpGetParamsBuilder.Build(parameters)}"
                : url;
            var result = await CallAsync<TResult>(HttpMethod.Get, httpQuery, null, cancellationToken).ConfigureAwait(false);
            return result;
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

        protected async Task DeleteAsync<TParams>(string url, TParams? parameters, CancellationToken cancellationToken)
            where TParams : class
        {
            var httpQuery = parameters != null
                ? $"{url}?{HttpGetParamsBuilder.Build(parameters)}"
                : url;
            await CallAsync(HttpMethod.Delete, httpQuery, null, cancellationToken).ConfigureAwait(false);
        }

        protected string GetAbsoluteUrl(string url) => $"{ApiSettings.ConnectionString}{url}";

        protected async Task<TResult> CallAsync<TResult>(HttpMethod method, string url, object? content, CancellationToken cancellationToken)
        {
            var response = await CallAsyncBase(method, url, content, cancellationToken).ConfigureAwait(false);
            
            var result = await response
                .Content
                .ReadAsAsync<TResult>(
                    new[] { Formatter },
                    cancellationToken
                )
                .ConfigureAwait(false);
            return result;
        }

        protected Task CallAsync(HttpMethod method, string url, object? content, CancellationToken cancellationToken) =>
            CallAsyncBase(method, url, content, cancellationToken);

        private async Task<HttpResponseMessage> CallAsyncBase(HttpMethod method, string url, object? content, CancellationToken cancellationToken)
        {
            var absoluteUrl = GetAbsoluteUrl(url);
            var absoluteUri = new Uri(absoluteUrl, UriKind.RelativeOrAbsolute);
            var requestMessage
                = new HttpRequestMessage
                  {
                      Method = method,
                      RequestUri = absoluteUri,
                  };
            if (content != null)
            {
                var type = content.GetType();
                var objContent = new ObjectContent(type, content, Formatter);
                requestMessage.Content = objContent;
            }

            var response = await Client.SendAsync(requestMessage, cancellationToken);

            response.EnsureSuccessStatusCode();
            return response;
        }
    }
}