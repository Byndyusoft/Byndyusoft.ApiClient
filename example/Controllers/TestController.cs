namespace Byndyusoft.ApiClient.Controllers
{
    using System;
    using System.Net.Http;
    using System.Net.Http.ProtoBuf;
    using System.Net.Http.ProtoBuf.Formatting;
    using System.Threading;
    using System.Threading.Tasks;
    using Byndyusoft.ApiClient;
    using Client;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using Models;

    /// <summary>
    ///     WeatherForecastController
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        /// <summary>
        ///     Get
        /// </summary>
        [HttpGet("test")]
        [FormatFilter]
        public async Task<IActionResult> Test(CancellationToken cancellationToken)
        {
            var connectionString = "http://localhost:5000/Test";
            var httpClient = new HttpClient
                {
                    BaseAddress = new Uri(connectionString)
                };
            var formatter = new ProtoBufMediaTypeFormatter();
            var options = new OptionsWrapper<ApiClientSettings>
            (
                new ApiClientSettings(){ConnectionString = connectionString}
            );

            var formatterClient = new TestFormatterClient(httpClient, formatter, options);
            var result = await formatterClient.GetAsync<SimpleProtobufType>("/get", cancellationToken);
            result = await formatterClient.PostAsync<SimpleProtobufType>("/post", result, cancellationToken);
            result = await formatterClient.PutAsync<SimpleProtobufType>("/put", result, cancellationToken);
            return Ok(result);
        }

        [HttpPost("post")]
        public async Task<IActionResult> Post([FromBody] SimpleProtobufType model, CancellationToken cancellationToken)
        {
            var result = ProtoBufContent.Create(model);
            return await FromProtoBufContent(result, cancellationToken);
        }

        [HttpPut("put")]
        public async Task<IActionResult> Put([FromBody] SimpleProtobufType model, CancellationToken cancellationToken)
        {
            var result = ProtoBufContent.Create(model);
            return await FromProtoBufContent(result, cancellationToken);
        }

        [HttpGet("get")]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            var result = ProtoBufContent.Create(SimpleProtobufType.Create());
            return await FromProtoBufContent(result, cancellationToken);
        }

        private static async Task<IActionResult> FromProtoBufContent(ProtoBufContent content, CancellationToken cancellationToken)
        {
            var resulString = await content.ReadAsStringAsync(cancellationToken);
            var contentType = content.Headers.ContentType.ToString();
            return new ContentResult() { Content = resulString, ContentType = contentType, StatusCode = 200 };
        }
    }
}