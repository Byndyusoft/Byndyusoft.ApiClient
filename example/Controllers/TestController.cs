namespace Byndyusoft.ApiClient.Controllers
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Json.Formatting;
    using System.Net.Http.MessagePack.Formatting;
    using System.Net.Http.ProtoBuf.Formatting;
    using System.Threading;
    using System.Threading.Tasks;
    using Byndyusoft.ApiClient;
    using Client;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using Models;

    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet("test_protobuf")]
        [FormatFilter]
        public async Task<IActionResult> TestProtoBuf(CancellationToken cancellationToken)
        {
            var connectionString = "http://localhost:5000/Test";
            var httpClient = new HttpClient
                {
                    BaseAddress = new Uri(connectionString)
                };
            var formatter = new ProtoBufMediaTypeFormatter(){TypeModel = { BufferSize = 4 * 1024 * 1024 },BufferSize = 4 * 1024 * 1024};
            var options = new OptionsWrapper<ApiClientSettings>
            (
                new ApiClientSettings(){ConnectionString = connectionString}
            );

            var formatterClient = new TestFormatterClient(httpClient, formatter, options);
            var result = await formatterClient.GetAsync<SimpleModel>("/get", cancellationToken);
            result = await formatterClient.GetAsync<ParamsTestModel,SimpleModel>(
                "/with_params",
                cancellationToken,
                new ParamsTestModel(
                    10101,
                    "tuple",
                    null));
            result = await formatterClient.PostAsync<SimpleModel>("/post", result, cancellationToken);
            result = await formatterClient.PutAsync<SimpleModel>("/put", result, cancellationToken);
            return Ok(result);
        }
        
        [HttpGet("test_message_pack")]
        [FormatFilter]
        public async Task<IActionResult> TestMessagePack(CancellationToken cancellationToken)
        {
            var connectionString = "http://localhost:5000/Test";
            var httpClient = new HttpClient
                             {
                                 BaseAddress = new Uri(connectionString)
                             };
            var formatter = new MessagePackMediaTypeFormatter();
            var options = new OptionsWrapper<ApiClientSettings>
            (
                new ApiClientSettings(){ConnectionString = connectionString}
            );

            var formatterClient = new TestFormatterClient(httpClient, formatter, options);
            var result = await formatterClient.GetAsync<SimpleModel>("/get", cancellationToken);
            result = await formatterClient.GetAsync<ParamsTestModel,SimpleModel>(
                "/with_params",
                cancellationToken,
                new ParamsTestModel(
                    10101,
                    "tuple",
                    null));
            result = await formatterClient.PostAsync<SimpleModel>("/post", result, cancellationToken);
            result = await formatterClient.PutAsync<SimpleModel>("/put", result, cancellationToken);
            return Ok(result);
        }
        
        [HttpGet("test_json")]
        [FormatFilter]
        public async Task<IActionResult> TestJson(CancellationToken cancellationToken)
        {
            var connectionString = "http://localhost:5000/Test";
            var httpClient = new HttpClient
                             {
                                 BaseAddress = new Uri(connectionString)
                             };
            var formatter = new JsonMediaTypeFormatter();
            var options = new OptionsWrapper<ApiClientSettings>
            (
                new ApiClientSettings(){ConnectionString = connectionString}
            );

            var formatterClient = new TestFormatterClient(httpClient, formatter, options);
            var result = await formatterClient.GetAsync<SimpleModel>("/get", cancellationToken);
            result = await formatterClient.GetAsync<ParamsTestModel,SimpleModel>(
                "/with_params",
                cancellationToken,
                new ParamsTestModel(
                    10101,
                    "tuple",
                    null));
            result = await formatterClient.PostAsync<SimpleModel>("/post", result, cancellationToken);
            result = await formatterClient.PutAsync<SimpleModel>("/put", result, cancellationToken);
            return Ok(result);
        }

        [HttpPost("post")]
        public IActionResult Post([FromBody] SimpleModel model)
        {
            return Ok(model);
        }

        [HttpPut("put")]
        public IActionResult Put([FromBody] SimpleModel model)
        {
            return Ok(model);
        }

        [HttpGet("get")]
        public IActionResult Get()
        {
            return Ok(SimpleModel.Create());
        }

        [HttpGet("with_params")]
        public IActionResult WithParams(
            [FromQuery] int property,
            [FromQuery] string field,
            [FromQuery] int? nullable)
        {
            return Ok(SimpleModel.Create(property, field, nullable));
        }
    }
}