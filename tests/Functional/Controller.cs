namespace Byndyusoft.ApiClient.Functional;

using Microsoft.AspNetCore.Mvc;
using Models;

public class Controller
{
    [ApiController]
    [Route("protobuf-formatter")]
    public class ProtoBufFormatterController : ControllerBase
    {
        [HttpPost("post")]
        public IActionResult Post([FromBody] SimpleType model)
        {
            return Ok(model);
        }

        [HttpPut("put")]
        public IActionResult Put([FromBody] SimpleType model)
        {
            return Ok(model);
        }

        [HttpGet("get")]
        public IActionResult Get()
        {
            return Ok(SimpleType.Create());
        }
    }
}