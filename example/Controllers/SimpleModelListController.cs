namespace Byndyusoft.ApiClient.Controllers;

using System.Linq;
using Byndyusoft.ApiClient.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class SimpleModelListController : ControllerBase
{
    [HttpPost]
    [Route("post/{id}")]
    [FormatFilter]
    public IActionResult Post([FromBody] SimpleModel model, int id)
    {
        ListModel.Data[id].Add(model);
        return Ok(model);
    }

    [HttpPut]
    [Route("put/{id}")]
    [FormatFilter]
    public IActionResult Put([FromBody] SimpleModel model, int id)
    {
        if(ListModel.Data[id].Any())
            ListModel.Data[id].RemoveAt(ListModel.Data[id].Count - 1);
        ListModel.Data[id].Add(model);
        return Ok(model);
    }

    [HttpPatch]
    [Route("patch/{id}")]
    [FormatFilter]
    public IActionResult Patch([FromBody] SimpleModel model, int id)
    {
        if (ListModel.Data.Any())
        {
            ListModel.Data[id][^1] = model;
            return Ok(model);
        }
        return NotFound();
    }

    [HttpGet]
    [Route("get/{id}")]
    [FormatFilter]
    public IActionResult Get(int id)
    {
        if(ListModel.Data[id].Any())
            return Ok(ListModel.Data[id].Last());
        return NotFound();
    }

    [HttpGet]
    [Route("getAll/{id}")]
    [FormatFilter]
    public IActionResult GetAll(int id)
    {
        return Ok(ListModel.Data[id]);
    }

    [HttpDelete]
    [Route("delete/{id}")]
    [FormatFilter]
    public IActionResult Delete(int id)
    {
        if (ListModel.Data[id].Any())
        {
            ListModel.Data[id].RemoveAt(ListModel.Data[id].Count - 1);
            return Ok();
        }

        return NotFound();
    }
}