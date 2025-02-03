namespace Byndyusoft.ApiClient.Example.Server.Controllers;

using System.Collections.Generic;
using System.Linq;
using Contracts;
using Microsoft.AspNetCore.Mvc;
using Models;

[ApiController]
[Route("[controller]")]
public class PersonController : ControllerBase
{
    [HttpPost]
    [Route(PersonModelListRoutes.AddListCmd + "/{id}")]
    [FormatFilter]
    public IActionResult AddList(int id)
    {
        PersonData.Data.TryAdd(id, new Dictionary<ulong, PersonModel>());
        return Ok();
    }

    [HttpGet]
    [Route(PersonModelListRoutes.GetEveryPersonCmd + "/{id}")]
    [FormatFilter]
    public IActionResult GetEveryPerson(int id)
    {
        if (PersonData.Data.TryGetValue(id, out var value)==false)
            return NotFound();
        
        return Ok(value);
    }

    [HttpDelete]
    [Route(PersonModelListRoutes.DeleteListCmd + "/{id}")]
    [FormatFilter]
    public IActionResult DeleteList(int id)
    {
        if (PersonData.Data.ContainsKey(id)==false)
            return NotFound();
        
        PersonData.Data.Remove(id, out _);
        return Ok();
    }

    [HttpPost]
    [Route(PersonModelListRoutes.AddPersonCmd + "/{id}")]
    [FormatFilter]
    public IActionResult AddPerson([FromBody] PersonModel model, int id)
    {
        if(PersonData.Data.ContainsKey(id)==false)
            return NotFound();
        
        if (PersonData.Data[id].TryAdd(model.Id, model)==false)
            return new ConflictResult();
        
        return Ok(model);
    }

    [HttpPut]
    [Route(PersonModelListRoutes.ReplacePersonCmd + "/{id}")]
    [FormatFilter]
    public IActionResult ReplacePerson([FromBody] PersonModel model, int id)
    {
        if (PersonData.Data.ContainsKey(id)==false)
            return NotFound($"No data found by id: {id}");
        
        var personId = model.Id;
        PersonData.Data[id][personId] = model;
        return Ok(model);
    }

    [HttpPatch]
    [Route(PersonModelListRoutes.UpdatePersonCmd + "/{id}")]
    [FormatFilter]
    public IActionResult UpdatePerson([FromBody] PersonModel model, int id)
    {
        if (PersonData.Data.ContainsKey(id)==false || PersonData.Data[id].Any()==false)
            return NotFound($"No data found by id: {id}");
        
        var personList = PersonData.Data[id];
        var personId = model.Id;
        if (personList.TryGetValue(personId, out var person)==false)
            return NotFound($"No person found by id: {personId}");
        
        if (string.IsNullOrEmpty(model.FirstName)==false)
            person.FirstName = model.FirstName;
        if (string.IsNullOrEmpty(model.LastName)==false)
            person.LastName = model.LastName;
        if (model.DateOfBirth.HasValue)
            person.DateOfBirth = model.DateOfBirth;
        if (model.DriverLicenseId.HasValue)
            person.DriverLicenseId = model.DriverLicenseId;
        if (model.IsMarried.HasValue)
            person.IsMarried = model.IsMarried;
        if (model.ChildrenNames != null)
            person.ChildrenNames = model.ChildrenNames;
        return Ok(person);
    }

    [HttpGet]
    [Route(PersonModelListRoutes.GetPersonCmd)]
    [FormatFilter]
    public IActionResult GetPerson(
        [FromQuery] int listId,
        [FromQuery] ulong id
    )
    {
        if (PersonData.Data.ContainsKey(listId)==false || PersonData.Data[listId].Any()==false)
            return NotFound($"No data found by id: {id}");
        
        var personList = PersonData.Data[listId];
        if (personList.TryGetValue(id, out var person)==false)
            return NotFound($"No person found by id: {id}");
        
        return Ok(person);
    }

    [HttpDelete]
    [Route(PersonModelListRoutes.DeletePersonCmd)]
    [FormatFilter]
    public IActionResult RemovePerson(
        [FromQuery] int listId,
        [FromQuery] ulong id
    )
    {
        if (PersonData.Data.ContainsKey(listId)==false || PersonData.Data[listId].Any()==false)
            return NotFound($"No data found by id: {id}");
        
        var personList = PersonData.Data[listId];
        if (personList.ContainsKey(id)==false)
            return NotFound($"No person found by id: {id}");
        
        personList.Remove(id);
        return Ok();
    }
}