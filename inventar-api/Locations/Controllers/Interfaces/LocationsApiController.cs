using inventar_api.ArticleLocations.DTOs;
using inventar_api.Locations.DTOs;
using inventar_api.Locations.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace inventar_api.Locations.Controllers.Interfaces;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public abstract class LocationsApiController : ControllerBase
{
    [HttpGet("all")]
    [ProducesResponseType(statusCode:200,type:typeof(IEnumerable<Location>))]
    public abstract Task<ActionResult<IEnumerable<Location>>> GetLocations();
    
    [HttpGet("location/{code}")]
    [ProducesResponseType(statusCode:200,type:typeof(Location))]
    [ProducesResponseType(statusCode:404,type:typeof(String))]
    public abstract Task<ActionResult<Location>> GetLocationByCode([FromRoute]string code);

    [HttpPost("create")]
    [ProducesResponseType(statusCode:201,type:typeof(Location))]
    [ProducesResponseType(statusCode:400,type:typeof(String))]
    public abstract Task<ActionResult<Location>> CreateLocation([FromBody]CreateLocationRequest request);
    
    [HttpDelete("delete/{code}")]
    [ProducesResponseType(statusCode:202,type:typeof(Location))]
    [ProducesResponseType(statusCode:404,type:typeof(String))]
    public abstract Task<ActionResult<Location>> DeleteLocation([FromRoute]string code);
}