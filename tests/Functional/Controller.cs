namespace Byndyusoft.ApiClient.Functional;

using Microsoft.AspNetCore.Mvc;
using Models;

public class Controller
{
    [Controller]
    [Route("protobuf-formatter")]
    public class ProtoBufFormatterController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post([FromBody] SimpleType model)
        {
            return Ok(model);
        }

        [HttpPut]
        public IActionResult Put([FromBody] SimpleType model)
        {
            return Ok(model);
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(SimpleType.Create());
        }
    }
}