using System.Net;
using System.Text;
using inventar_api_tests.Articles.Helpers;
using inventar_api_tests.Infrastructure;
using inventar_api.Articles.DTOs;
using inventar_api.Articles.Models;
using inventar_api.Articles.Models.Comparers;
using Newtonsoft.Json;

namespace inventar_api_tests.Articles.Integration;

public class ArticlesIntegrationTests : IClassFixture<ApiWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public ArticlesIntegrationTests(ApiWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Get_GetArticles_ArticlesDoNotExist_ReturnsNotFoundStatusCode()
        {
            var request = "/api/v1/Articles/all";

            var response = await _client.GetAsync(request);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Get_GetArticles_ArticlesExist_ReturnsOkStatusCode_ValidArticlesContentResponse()
        {
            var createArticleRequest = "/api/v1/Articles/create";
            CreateArticleRequest articleRequest1 = TestArticleHelper.CreateCreateArticleRequest(1);
            CreateArticleRequest articleRequest2 = TestArticleHelper.CreateCreateArticleRequest(2);
            var content1 = new StringContent(JsonConvert.SerializeObject(articleRequest1), Encoding.UTF8, "application/json");
            var content2 = new StringContent(JsonConvert.SerializeObject(articleRequest2), Encoding.UTF8, "application/json");
            await _client.PostAsync(createArticleRequest, content1);
            await _client.PostAsync(createArticleRequest, content2);

            var request = "/api/v1/Articles/all";
            var response = await _client.GetAsync(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<Article>>(responseString);
            var articles = TestArticleHelper.CreateArticles(2);
            Assert.Equal(articles, result, new ArticleEqualityComparer());

            var deleteRequest = "/api/v1/Articles/";
            await _client.DeleteAsync(deleteRequest + "1");
            await _client.DeleteAsync(deleteRequest + "2");
        }

        [Fact]
        public async Task Get_GetArticleByCode_ArticleNotFound_ReturnsNotFoundStatusCode()
        {
            var request = "/api/v1/Articles/article/3";

            var response = await _client.GetAsync(request);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Get_GetArticleByCode_ArticleFound_ReturnsOkStatusCode_ValidArticleContentResponse()
        {
            var createArticleRequest = "/api/v1/Articles/create";
            CreateArticleRequest articleRequest = TestArticleHelper.CreateCreateArticleRequest(4);
            var content = new StringContent(JsonConvert.SerializeObject(articleRequest), Encoding.UTF8, "application/json");
            await _client.PostAsync(createArticleRequest, content);

            var request = "/api/v1/Articles/article/4";
            var response = await _client.GetAsync(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Article>(responseString);
            var article = TestArticleHelper.CreateArticle(4);
            Assert.Equal(article.Code, result.Code);

            var deleteRequest = "/api/v1/Articles/delete/";
            await _client.DeleteAsync(deleteRequest + "4");
        }

        [Fact]
        public async Task Post_CreateArticle_InvalidArticleCode_ReturnsBadRequestStatusCode()
        {
            var createRequest = "/api/v1/Articles/create";
            CreateArticleRequest request = TestArticleHelper.CreateCreateArticleRequest(-1);
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(createRequest, content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        
        [Fact]
        public async Task Post_CreateArticle_CodeAlreadyUsed_ReturnsBadRequestStatusCode()
        {
            var createRequest = "/api/v1/Articles/create";
            CreateArticleRequest request = TestArticleHelper.CreateCreateArticleRequest(5);
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            await _client.PostAsync(createRequest, content);

            var response = await _client.PostAsync(createRequest, content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var deleteRequest = "/api/v1/Articles/delete/";
            await _client.DeleteAsync(deleteRequest + "5");
        }

        [Fact]
        public async Task Post_CreateArticle_ValidRequest_ReturnsCreatedStatusCode_ValidArticleContentResponse()
        {
            var createRequest = "/api/v1/Articles/create";
            CreateArticleRequest request = TestArticleHelper.CreateCreateArticleRequest(6);
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(createRequest, content);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Article>(responseString);
            var article = TestArticleHelper.CreateArticle(6);
            Assert.Equal(article.Code, result.Code);

            var deleteRequest = "/api/v1/Articles/delete/";
            await _client.DeleteAsync(deleteRequest + "6");
        }
        
        [Fact]
        public async Task Put_UpdateArticle_InvalidArticleCode_ReturnsBadRequestStatusCode()
        {
            var updateRequest = "/api/v1/Articles/update";
            UpdateArticleRequest request = TestArticleHelper.CreateUpdateArticleRequest(-1);
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            var response = await _client.PutAsync(updateRequest, content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        
        [Fact]
        public async Task Put_UpdateArticle_CodeAlreadyUsed_ReturnsBadRequestStatusCode()
        {
            var updateRequest = "/api/v1/Articles/update";
            UpdateArticleRequest request = TestArticleHelper.CreateUpdateArticleRequest(7);
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            
            var response = await _client.PutAsync(updateRequest, content);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Put_UpdateArticle_ValidRequest_ReturnsUpdatedStatusCode_ValidArticleContentResponse()
        {
            var createRequest = "/api/v1/Articles/create";
            CreateArticleRequest createRequestDto = TestArticleHelper.CreateCreateArticleRequest(8);
            var createRequestContent = new StringContent(JsonConvert.SerializeObject(createRequestDto), Encoding.UTF8, "application/json");
            await _client.PostAsync(createRequest, createRequestContent);
            
            var updateRequest = "/api/v1/Articles/update";
            UpdateArticleRequest request = TestArticleHelper.CreateUpdateArticleRequest(8);
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            var response = await _client.PutAsync(updateRequest, content);

            Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Article>(responseString);
            var article = TestArticleHelper.CreateArticle(8);
            Assert.Equal(article.Code, result.Code);
            
            var deleteRequest = "/api/v1/Articles/delete/";
            await _client.DeleteAsync(deleteRequest + "8");
        }

        [Fact]
        public async Task Delete_DeleteArticle_ArticleNotFound_ReturnsNotFoundStatusCode()
        {
            var deleteRequest = "/api/v1/Articles/delete";

            var response = await _client.DeleteAsync(deleteRequest + "9");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        
        [Fact]
        public async Task Delete_DeleteArticle_ArticleFound_ReturnsAcceptedStatusCode()
        {
            var createRequest = "/api/v1/Articles/create";
            CreateArticleRequest request = TestArticleHelper.CreateCreateArticleRequest(10);
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            await _client.PostAsync(createRequest, content);
        
            var deleteRequest = "/api/v1/Articles/delete/";
        
            var response = await _client.DeleteAsync(deleteRequest + "10");
        
            Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Article>(responseString);
            var article = TestArticleHelper.CreateArticle(10);
            Assert.Equal(article.Code, result.Code);
        }
    }