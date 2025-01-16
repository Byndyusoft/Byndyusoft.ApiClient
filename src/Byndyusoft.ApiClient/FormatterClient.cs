namespace Byndyusoft.ApiClient
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;

    public class FormatterClient: BaseClient
    {
        private readonly MediaTypeFormatter _formatter;

        protected FormatterClient
        (
            HttpClient client,
            MediaTypeFormatter formatter,
            IOptions<ApiClientSettings> apiSettings
        ):base(client, apiSettings)
        {
            _formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
            foreach (var mediaTypeHeaderValue in _formatter.SupportedMediaTypes)
                Client.DefaultRequestHeaders.Accept.Add
                (
                    new MediaTypeWithQualityHeaderValue
                    (
                        mediaTypeHeaderValue.MediaType
                    )
                );
        }
        
        protected new async Task<TResult> GetAsync<TResult>(string url, CancellationToken cancellationToken) =>
            await CallAsync<TResult>(HttpMethod.Get, url, null, cancellationToken);

        protected new async Task<TResult> GetAsync<TParams, TResult>(string url, CancellationToken cancellationToken, TParams? dto = null)
            where TParams : class
        {
            var httpQuery = dto != null
                ? $"{url}?{HttpGetParamsBuilder.Build(dto)}"
                : url;
            var result = await CallAsync<TResult>(HttpMethod.Get, httpQuery, null, cancellationToken).ConfigureAwait(false);
            return result;
        }

        protected new Task PostAsync(string url, object content, CancellationToken cancellationToken) =>
            CallAsync(HttpMethod.Post, url, content, cancellationToken);

        protected new Task<TResult> PostAsync<TResult>(string url, object content, CancellationToken cancellationToken) =>
            CallAsync<TResult>(HttpMethod.Post, url, content, cancellationToken);

        protected new Task<TResult> PutAsync<TResult>(string url, object content, CancellationToken cancellationToken) =>
            CallAsync<TResult>(HttpMethod.Put, url, content, cancellationToken);

        protected new Task PutAsync(string url, object content, CancellationToken cancellationToken) =>
            CallAsync(HttpMethod.Put, url, content, cancellationToken);

        protected new Task<TResult> PatchAsync<TResult>(string url, object content, CancellationToken cancellationToken) =>
            CallAsync<TResult>(new HttpMethod("PATCH"), url, content, cancellationToken);

        protected new Task PatchAsync(string url, object content, CancellationToken cancellationToken) =>
            CallAsync(new HttpMethod("PATCH"), url, content, cancellationToken);

        protected new Task DeleteAsync(string url, CancellationToken cancellationToken) =>
            CallAsync(HttpMethod.Delete, url, null, cancellationToken);

        protected new Task DeleteAsync<TParams>(string url, TParams parameters, CancellationToken cancellationToken) =>
            CallAsync(HttpMethod.Delete, url, parameters, cancellationToken);

        protected new async Task<TResult> CallAsync<TResult>(HttpMethod method, string url, object? content, CancellationToken cancellationToken)
        {
            var response = await CallAsyncBase(method, url, content, cancellationToken).ConfigureAwait(false);
            var result = await response!
                .Content
                .ReadAsAsync<TResult>
                (
                    new[]
                    {
                        _formatter
                    },
                    cancellationToken
                )
                .ConfigureAwait(false);
            return result;
        }

        protected new async Task CallAsync(HttpMethod method, string url, object? content, CancellationToken cancellationToken)
        {
            await CallAsyncBase(method, url, content, cancellationToken).ConfigureAwait(false);
        }

        private async Task<HttpResponseMessage?> CallAsyncBase
        (
            HttpMethod method,
            string url,
            object? content,
            CancellationToken cancellationToken)
        {
            var isProtobuf = _formatter.GetType().Name.ToLower().Contains("protobuf");
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
                var objContent = new ObjectContent(type, content, _formatter);
                requestMessage.Content = objContent;
                if (isProtobuf)
                {
                    var len = await objContent.ReadAsByteArrayAsync();
                    requestMessage.Content.Headers.ContentLength = len.LongLength;
                    requestMessage.Headers.TransferEncodingChunked = false;
                }
            }

            if (isProtobuf)
                requestMessage.Version = new Version(1, 0);

            var response = await Client.SendAsync(requestMessage, cancellationToken).ConfigureAwait(false);
            await Toolkit.EnsureSuccessStatusCode(response).ConfigureAwait(false);
            return response;
        }
    }
}