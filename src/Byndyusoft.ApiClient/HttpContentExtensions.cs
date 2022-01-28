namespace Byndyusoft.ApiClient
{
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    public static class HttpContentExtensions
    {
        private static readonly JsonSerializerSettings SerializationSettings;

        static HttpContentExtensions()
        {
            SerializationSettings = new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.Objects};
        }

        public static HttpContent PrepareHttpContent(object? data)
        {
            var content = new StringContent(JsonConvert.SerializeObject(data, SerializationSettings));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return content;
        }

        public static async Task<T> ReadAsJsonAsync<T>(this HttpContent content)
        {
            var dataAsString = await content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(dataAsString, SerializationSettings);
        }
    }
}