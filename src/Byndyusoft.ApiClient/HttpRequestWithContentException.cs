namespace Byndyusoft.ApiClient
{
    using System.Net;
    using System.Net.Http;

    public class HttpRequestWithContentException : HttpRequestException
    {
        public HttpRequestWithContentException(string message, HttpStatusCode statusCode, string? content)
            : base(message, inner: null)
        {
            StatusCode = statusCode;
            Content = content;
        }

        public HttpStatusCode StatusCode { get; set; }

        public string? Content { get; set; }
    }
}