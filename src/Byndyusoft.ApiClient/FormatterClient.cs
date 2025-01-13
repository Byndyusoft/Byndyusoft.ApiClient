namespace Byndyusoft.ApiClient
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;

    public class FormatterClient:BaseClient
    {
        protected readonly MediaTypeFormatter Formatter;

        protected FormatterClient
        (
            HttpClient client,
            MediaTypeFormatter formatter,
            IOptions<ApiClientSettings> apiSettings
        ):base(client, apiSettings)
        {
            Formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
        }
        
        protected async Task<TResult> GetAsync<TResult>(string url, CancellationToken cancellationToken)
        {
            var absoluteUrl = GetAbsoluteUrl(url);
            var response = await Client.GetAsync(absoluteUrl, cancellationToken).ConfigureAwait(false);
            await Toolkit.EnsureSuccessStatusCode(response);
            var result = await response
                .Content
                .ReadAsAsync<TResult>
                (
                    new[]
                    {
                        Formatter
                    },
                    cancellationToken
                )
                .ConfigureAwait(false);
            return result;
        }

        protected async Task<TResult> GetAsync<TParams, TResult>(string url, CancellationToken cancellationToken, TParams? dto = null)
            where TParams : class
        {
            var endpoint = GetAbsoluteUrl(url);
            var httpQuery = dto != null
                ? $"{endpoint}?{HttpGetParamsBuilder.Build(dto)}"
                : endpoint;

            var response = await Client.GetAsync(httpQuery, cancellationToken).ConfigureAwait(false);

            await Toolkit.EnsureSuccessStatusCode(response);
            return await response
                .Content
                .ReadAsAsync<TResult>
                (
                    new[]
                    {
                        Formatter
                    },
                    cancellationToken
                )
                .ConfigureAwait(false);
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
            var type = content.GetType();
            var absoluteUrl = GetAbsoluteUrl(url);
            var absoluteUri = new Uri(absoluteUrl, UriKind.RelativeOrAbsolute);
            var requestMessage
                = new HttpRequestMessage
                  {
                      Method = method,
                      RequestUri = absoluteUri,
                      Content = new ObjectContent(type, content, Formatter)
                  };

            var response = await Client.SendAsync(requestMessage, cancellationToken);

            await Toolkit.EnsureSuccessStatusCode(response);
            return await response
                .Content
                .ReadAsAsync<TResult>
                (
                    new[]
                    {
                        Formatter
                    },
                    cancellationToken
                )
                .ConfigureAwait(false);
        }

        protected async Task CallAsync(HttpMethod method, string url, object? content, CancellationToken cancellationToken)
        {
            var type = content.GetType();
            var requestMessage
                = new HttpRequestMessage
                  {
                      Method = method,
                      RequestUri = new Uri(GetAbsoluteUrl(url), UriKind.RelativeOrAbsolute),
                      Content = new ObjectContent(type, content, Formatter)
                  };
            var response = await Client.SendAsync(requestMessage, cancellationToken).ConfigureAwait(false);
            await Toolkit.EnsureSuccessStatusCode(response);
        }
    }
}