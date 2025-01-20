namespace Byndyusoft.ApiClient
{
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Net.Http.Json;
    using System.Threading.Tasks;

    public static class HttpContentExtensions
    {
        public static HttpContent PrepareHttpContent(object? data)
        {
            var content = JsonContent.Create(data);
            content.Headers.ContentType = new MediaTypeHeaderValue(JsonDefaults.MediaTypeHeader.MediaType);
            return content;
        }

        public static Task<T> ReadAsJsonAsync<T>(this HttpContent content)
            => content.ReadAsAsync<T>();
    }
}