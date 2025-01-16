namespace Byndyusoft.ApiClient.Functional.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Models;

    [Controller]
    [Route("formatter")]
    public class FormatterController : ControllerBase
    {
        [HttpPost]
        [Route("post")]
        [FormatFilter]
        public IActionResult Post([FromBody] SimpleModel model)
        {
            return Ok(model);
        }

        [HttpPut]
        [Route("put")]
        [FormatFilter]
        public IActionResult Put([FromBody] SimpleModel model)
        {
            return Ok(model);
        }

        [HttpGet]
        [Route("get")]
        [FormatFilter]
        public IActionResult Get()
        {
            return Ok(SimpleModel.Create());
        }
        

        [HttpGet]
        [Route("with_params")]
        [FormatFilter]
        public IActionResult WithParams(
            [FromQuery] int property,
            [FromQuery] string field,
            [FromQuery] int? nullable)
        {
            return Ok(SimpleModel.Create(property, field:field, nullable:nullable));
        }
    }
}