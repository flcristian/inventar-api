using inventar_api.Articles.DTOs;
using inventar_api.Articles.Models;
using Microsoft.AspNetCore.Mvc;

namespace inventar_api.Articles.Controllers.Interfaces;

[ApiController]
[Route("api/v1/[controller]")]
public abstract class ArticlesApiController : ControllerBase
{
    [HttpGet("all")]
    [ProducesResponseType(statusCode:200,type:typeof(IEnumerable<Article>))]
    public abstract Task<ActionResult<IEnumerable<Article>>> GetArticles();
    
    [HttpGet("article/{code}")]
    [ProducesResponseType(statusCode:200,type:typeof(Article))]
    [ProducesResponseType(statusCode:404,type:typeof(String))]
    public abstract Task<ActionResult<Article>> GetArticleByCode([FromRoute]int code);

    [HttpPost("create")]
    [ProducesResponseType(statusCode:201,type:typeof(Article))]
    [ProducesResponseType(statusCode:400,type:typeof(String))]
    public abstract Task<ActionResult<Article>> CreateArticle([FromBody]CreateArticleRequest request);
    
    [HttpPut("update")]
    [ProducesResponseType(statusCode:202,type:typeof(Article))]
    [ProducesResponseType(statusCode:400,type:typeof(String))]
    [ProducesResponseType(statusCode:404,type:typeof(String))]
    public abstract Task<ActionResult<Article>> UpdateArticle([FromBody]UpdateArticleRequest request);
    
    [HttpDelete("delete/{code}")]
    [ProducesResponseType(statusCode:202,type:typeof(Article))]
    [ProducesResponseType(statusCode:404,type:typeof(String))]
    public abstract Task<ActionResult<Article>> DeleteArticle([FromRoute]int code);
}