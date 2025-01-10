namespace Byndyusoft.ApiClient
{
    using System.Net.Http;
    using System.Threading.Tasks;

    public static class Toolkit
    {
        public static async Task EnsureSuccessStatusCode(HttpResponseMessage response)
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