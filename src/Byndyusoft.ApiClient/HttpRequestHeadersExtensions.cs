namespace Byndyusoft.ApiClient
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http.Headers;

    public static class HttpRequestHeadersExtensions
    {
        public static HttpRequestHeaders AddRange(this HttpRequestHeaders headers, Dictionary<string, string> newHeaders)
        {
            if (newHeaders.Any() == false)
                return headers;

            foreach (var newHeaderKey in newHeaders.Keys)
            {
                headers.Add(newHeaderKey, newHeaders[newHeaderKey]);
            }

            return headers;
        }
    }
}