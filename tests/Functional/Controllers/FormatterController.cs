namespace Byndyusoft.ApiClient.Functional.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Models;

    [Controller]
    [Route("formatter")]
    public class FormatterController : ControllerBase
    {
        [HttpPost("post")]
        [Route("post")]
        [FormatFilter]
        public IActionResult Post([FromBody] SimpleModel model)
        {
            return Ok(model);
        }

        [HttpPut("put")]
        [Route("put")]
        [FormatFilter]
        public IActionResult Put([FromBody] SimpleModel model)
        {
            return Ok(model);
        }

        [HttpGet("get")]
        [Route("get")]
        [FormatFilter]
        public IActionResult Get()
        {
            return Ok(SimpleModel.Create());
        }
    }
}