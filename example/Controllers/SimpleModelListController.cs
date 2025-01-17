namespace Byndyusoft.ApiClient.Controllers;

using System.Collections.Generic;
using System.Linq;
using Byndyusoft.ApiClient.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class SimpleModelListController : ControllerBase
{
    [HttpPost]
    [Route("addList/{id}")]
    [FormatFilter]
    public IActionResult AddList(int id)
    {
        ListModel.Data.Add(id, new List<SimpleModel>());
        return Ok();
    }

    [HttpDelete]
    [Route("deleteList/{id}")]
    [FormatFilter]
    public IActionResult DeleteList(int id)
    {
        if (!ListModel.Data.ContainsKey(id))
            return NotFound();
        ListModel.Data.Remove(id);
        return Ok();
    }

    [HttpPost]
    [Route("post/{id}")]
    [FormatFilter]
    public IActionResult Post([FromBody] SimpleModel model, int id)
    {
        if(!ListModel.Data.ContainsKey(id))
            return NotFound();
        ListModel.Data[id].Add(model);
        return Ok(model);
    }

    [HttpPut]
    [Route("put/{id}")]
    [FormatFilter]
    public IActionResult Put([FromBody] SimpleModel model, int id)
    {
        if (!ListModel.Data.ContainsKey(id))
            return NotFound();
        if (ListModel.Data[id].Any())
            ListModel.Data[id].RemoveAt(ListModel.Data[id].Count - 1);
        ListModel.Data[id].Add(model);
        return Ok(model);
    }

    [HttpPatch]
    [Route("patch/{id}")]
    [FormatFilter]
    public IActionResult Patch([FromBody] SimpleModel model, int id)
    {
        if (!ListModel.Data.ContainsKey(id))
            return NotFound();
        if (ListModel.Data[id].Any())
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
        if (!ListModel.Data.ContainsKey(id))
            return NotFound();
        if (ListModel.Data[id].Any())
            return Ok(ListModel.Data[id].Last());
        return NotFound();
    }

    [HttpGet]
    [Route("getAll/{id}")]
    [FormatFilter]
    public IActionResult GetAll(int id)
    {
        if (!ListModel.Data.TryGetValue(id, out var value))
            return NotFound();
        return Ok(value);
    }

    [HttpDelete]
    [Route("delete/{id}")]
    [FormatFilter]
    public IActionResult Delete(int id)
    {
        if (!ListModel.Data.ContainsKey(id))
            return NotFound();
        if (ListModel.Data[id].Any())
        {
            ListModel.Data[id].RemoveAt(ListModel.Data[id].Count - 1);
            return Ok();
        }

        return NotFound();
    }
}