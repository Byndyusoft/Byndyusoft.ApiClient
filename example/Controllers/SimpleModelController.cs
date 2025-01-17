namespace Byndyusoft.ApiClient.Controllers;

using Microsoft.AspNetCore.Mvc;
using Models;

[ApiController]
[Route("[controller]")]
public class SimpleModelController : ControllerBase
    {
        [HttpPost]
        [Route("post")]
        [FormatFilter]
        public IActionResult Post([FromBody] SimpleModel model) => Ok(model);

        [HttpPut]
        [Route("put")]
        [FormatFilter]
        public IActionResult Put([FromBody] SimpleModel model) => Ok(model);

        [HttpGet]
        [Route("get")]
        [FormatFilter]
        public IActionResult Get() => Ok(SimpleModel.Create());

        [HttpGet]
        [Route("with_params")]
        [FormatFilter]
        public IActionResult WithParams(
            [FromQuery] int property,
            [FromQuery] string field,
            [FromQuery] int? nullable)
            => Ok(SimpleModel.Create(property, field: field, nullable: nullable));
}