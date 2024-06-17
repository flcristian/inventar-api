using inventar_api.Users.Models;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using RegisterRequest = inventar_api.Users.Models.RegisterRequest;

namespace inventar_api.Users.Controllers.Interfaces;

[ApiController]
[Route("api/v1/[controller]")]
public abstract class UsersApiController: ControllerBase
{
    [HttpGet("all")]
    [ProducesResponseType(statusCode:200,type:typeof(IEnumerable<User>))]
    public abstract Task<ActionResult<IEnumerable<User>>> GetUsers();

    [HttpGet("user/{id}")]
    [ProducesResponseType(statusCode: 200, type: typeof(User))]
    [ProducesResponseType(statusCode: 404, type: typeof(string))]
    public abstract Task<ActionResult<User>> GetUser([FromRoute]int id);
    
    [HttpPost("register")]
    [ProducesResponseType(statusCode: 200, type: typeof(User))]
    [ProducesResponseType(statusCode: 400, type: typeof(string))]
    public abstract Task<ActionResult<User>> Register([FromBody]RegisterRequest request);

    [HttpPost("login")]
    [ProducesResponseType(statusCode: 200, type: typeof(User))]
    [ProducesResponseType(statusCode: 401, type: typeof(string))]
    public abstract Task<ActionResult<User>> Login([FromBody]LoginRequest request);
}