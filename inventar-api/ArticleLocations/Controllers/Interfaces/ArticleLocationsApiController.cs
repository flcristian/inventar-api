using inventar_api.ArticleLocations.DTOs;
using inventar_api.ArticleLocations.Models;
using Microsoft.AspNetCore.Mvc;

namespace inventar_api.ArticleLocations.Controllers.Interfaces;

[ApiController]
[Route("api/v1/[controller]")]
public abstract class ArticleLocationsApiController : ControllerBase
{
    [HttpGet("all")]
    [ProducesResponseType(statusCode:200,type:typeof(IEnumerable<ArticleLocation>))]
    [ProducesResponseType(statusCode:404,type:typeof(String))]
    public abstract Task<ActionResult<IEnumerable<ArticleLocation>>> GetArticleLocations();
    
    [HttpGet("article_location")]
    [ProducesResponseType(statusCode:200,type:typeof(ArticleLocation))]
    [ProducesResponseType(statusCode:404,type:typeof(String))]
    public abstract Task<ActionResult<ArticleLocation>> GetArticleLocation([FromQuery]int articleCode, [FromQuery]string locationCode);

    [HttpPost("create")]
    [ProducesResponseType(statusCode:201,type:typeof(ArticleLocation))]
    [ProducesResponseType(statusCode:400,type:typeof(String))]
    [ProducesResponseType(statusCode:404,type:typeof(String))]
    public abstract Task<ActionResult<ArticleLocation>> CreateArticleLocation([FromBody]CreateArticleLocationRequest request);
    
    [HttpPut("update")]
    [ProducesResponseType(statusCode:202,type:typeof(ArticleLocation))]
    [ProducesResponseType(statusCode:400,type:typeof(String))]
    [ProducesResponseType(statusCode:404,type:typeof(String))]
    public abstract Task<ActionResult<ArticleLocation>> UpdateArticleLocation([FromBody]UpdateArticleLocationRequest request);
    
    [HttpDelete("delete")]
    [ProducesResponseType(statusCode:202,type:typeof(ArticleLocation))]
    [ProducesResponseType(statusCode:404,type:typeof(String))]
    public abstract Task<ActionResult<ArticleLocation>> DeleteArticleLocation([FromQuery]int articleCode, [FromQuery]string locationCode);
}