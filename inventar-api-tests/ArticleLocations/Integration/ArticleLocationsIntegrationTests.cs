using System.Net;
using System.Text;
using inventar_api.ArticleLocations.DTOs;
using inventar_api.ArticleLocations.Models;
using inventar_api.ArticleLocations.Models.Comparers;
using inventar_api_tests.ArticleLocations.Helpers;
using inventar_api_tests.Articles.Helpers;
using inventar_api_tests.Infrastructure;
using inventar_api_tests.Locations.Helpers;
using inventar_api.Articles.DTOs;
using inventar_api.Locations.DTOs;
using Newtonsoft.Json;

namespace inventar_api_tests.ArticleLocations.Integration;

public class ArticleLocationsIntegrationTests : IClassFixture<ApiWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public ArticleLocationsIntegrationTests(ApiWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetArticleLocations_ArticleLocationsDoNotExist_ReturnsNotFoundStatusCode()
        {
            var request = "/api/v1/ArticleLocations/all";

            var response = await _client.GetAsync(request);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetArticleLocations_ArticleLocationsExist_ReturnsOkStatusCode_ValidArticleLocationsContentResponse()
        {
            var createArticleRequest1 = "/api/v1/Articles/create";
            CreateArticleRequest createArticleRequestDto1 = TestArticleHelper.CreateCreateArticleRequest(1);
            var createArticleRequestContent1 = new StringContent(JsonConvert.SerializeObject(createArticleRequestDto1), Encoding.UTF8, "application/json");
            await _client.PostAsync(createArticleRequest1, createArticleRequestContent1);
            var createLocationRequest1 = "/api/v1/Locations/create";
            CreateLocationRequest locationRequest1 = TestLocationHelper.CreateCreateLocationRequest(1);
            var createLocationRequestContent1 = new StringContent(JsonConvert.SerializeObject(locationRequest1), Encoding.UTF8, "application/json");
            await _client.PostAsync(createLocationRequest1, createLocationRequestContent1);
            
            var createArticleRequest2 = "/api/v1/Articles/create";
            CreateArticleRequest createArticleRequestDto2 = TestArticleHelper.CreateCreateArticleRequest(2);
            var createArticleRequestContent2 = new StringContent(JsonConvert.SerializeObject(createArticleRequestDto2), Encoding.UTF8, "application/json");
            await _client.PostAsync(createArticleRequest2, createArticleRequestContent2);
            var createLocationRequest2 = "/api/v1/Locations/create";
            CreateLocationRequest locationRequest2= TestLocationHelper.CreateCreateLocationRequest(2);
            var createLocationRequestContent2 = new StringContent(JsonConvert.SerializeObject(locationRequest2), Encoding.UTF8, "application/json");
            await _client.PostAsync(createLocationRequest2, createLocationRequestContent2);
            
            var createRequest = "/api/v1/ArticleLocations/create";
            CreateArticleLocationRequest articleLocationRequest1 = TestArticleLocationHelper.CreateCreateArticleLocationRequest(1);
            CreateArticleLocationRequest articleLocationRequest2 = TestArticleLocationHelper.CreateCreateArticleLocationRequest(2);
            var content1 = new StringContent(JsonConvert.SerializeObject(articleLocationRequest1), Encoding.UTF8, "application/json");
            var content2 = new StringContent(JsonConvert.SerializeObject(articleLocationRequest2), Encoding.UTF8, "application/json");
            await _client.PostAsync(createRequest, content1);
            await _client.PostAsync(createRequest, content2);

            var request = "/api/v1/ArticleLocations/all";
            var response = await _client.GetAsync(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<ArticleLocation>>(responseString);
            var articleLocations = TestArticleLocationHelper.CreateArticleLocations(2);
            Assert.Equal(articleLocations, result, new ArticleLocationEqualityComparer());

            var deleteRequest = "/api/v1/ArticleLocations/delete/";
            await _client.DeleteAsync(deleteRequest + "1");
            await _client.DeleteAsync(deleteRequest + "2");
        }

        [Fact]
        public async Task GetArticleLocationByCode_ArticleLocationNotFound_ReturnsNotFoundStatusCode()
        {
            var request = "/api/v1/ArticleLocations/article_location/3";

            var response = await _client.GetAsync(request);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetArticleLocationByCode_ArticleLocationFound_ReturnsOkStatusCode_ValidArticleLocationContentResponse()
        {
            var createArticleRequest = "/api/v1/Articles/create";
            CreateArticleRequest createArticleRequestDto = TestArticleHelper.CreateCreateArticleRequest(4);
            var createArticleRequestContent = new StringContent(JsonConvert.SerializeObject(createArticleRequestDto), Encoding.UTF8, "application/json");
            await _client.PostAsync(createArticleRequest, createArticleRequestContent);
            var createLocationRequest = "/api/v1/Locations/create";
            CreateLocationRequest locationRequest = TestLocationHelper.CreateCreateLocationRequest(4);
            var createLocationRequestContent = new StringContent(JsonConvert.SerializeObject(locationRequest), Encoding.UTF8, "application/json");
            await _client.PostAsync(createLocationRequest, createLocationRequestContent);
            
            var createRequest = "/api/v1/ArticleLocations/create";
            CreateArticleLocationRequest articleLocationRequest = TestArticleLocationHelper.CreateCreateArticleLocationRequest(4);
            var content = new StringContent(JsonConvert.SerializeObject(articleLocationRequest), Encoding.UTF8, "application/json");
            var r = await _client.PostAsync(createRequest, content);

            var request = "/api/v1/ArticleLocations/article_location?articleCode=4&locationCode=4";
            var response = await _client.GetAsync(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var deleteRequest = "/api/v1/ArticleLocations/delete/";
            await _client.DeleteAsync(deleteRequest + "4");
        }

        [Fact]
        public async Task CreateArticleLocation_InvalidArticleCount_ReturnsBadRequestStatusCode()
        {
            var createRequest = "/api/v1/ArticleLocations/create";
            CreateArticleLocationRequest request = TestArticleLocationHelper.CreateCreateArticleLocationRequest(5);
            request.Count = -1;
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(createRequest, content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CreateArticleLocation_ArticleDoesNotExist_ReturnsNotFoundStatusCode()
        {
            var createRequest = "/api/v1/ArticleLocations/create";
            CreateArticleLocationRequest request = TestArticleLocationHelper.CreateCreateArticleLocationRequest(6);
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(createRequest, content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CreateArticleLocation_LocationDoesNotExist_ReturnsNotFoundStatusCode()
        {
            var createRequest = "/api/v1/ArticleLocations/create";
            CreateArticleLocationRequest request = TestArticleLocationHelper.CreateCreateArticleLocationRequest(7);
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(createRequest, content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CreateArticleLocation_ValidData_ReturnsCreatedStatusCode_ValidArticleLocationContentResponse()
        {
            var createArticleRequest = "/api/v1/Articles/create";
            CreateArticleRequest createArticleRequestDto = TestArticleHelper.CreateCreateArticleRequest(8);
            var createArticleRequestContent = new StringContent(JsonConvert.SerializeObject(createArticleRequestDto), Encoding.UTF8, "application/json");
            await _client.PostAsync(createArticleRequest, createArticleRequestContent);
            var createLocationRequest = "/api/v1/Locations/create";
            CreateLocationRequest locationRequest = TestLocationHelper.CreateCreateLocationRequest(8);
            var createLocationRequestContent = new StringContent(JsonConvert.SerializeObject(locationRequest), Encoding.UTF8, "application/json");
            await _client.PostAsync(createLocationRequest, createLocationRequestContent);
            
            var createRequest = "/api/v1/ArticleLocations/create";
            CreateArticleLocationRequest request = TestArticleLocationHelper.CreateCreateArticleLocationRequest(8);
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(createRequest, content);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var deleteRequest = "/api/v1/ArticleLocations/delete/";
            await _client.DeleteAsync(deleteRequest + "8");
        }

        [Fact]
        public async Task UpdateArticleLocation_InvalidArticleCount_ReturnsBadRequestStatusCode()
        {
            var updateRequest = "/api/v1/ArticleLocations/update";
            UpdateArticleLocationRequest request = TestArticleLocationHelper.CreateUpdateArticleLocationRequest(9);
            request.Count = -1;
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            var response = await _client.PutAsync(updateRequest, content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task UpdateArticleLocation_ArticleLocationDoesNotExist_ReturnsNotFoundStatusCode()
        {
            var updateRequest = "/api/v1/ArticleLocations/update";
            UpdateArticleLocationRequest request = TestArticleLocationHelper.CreateUpdateArticleLocationRequest(10);
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            var response = await _client.PutAsync(updateRequest, content);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        
        [Fact]
        public async Task UpdateArticleLocation_ValidRequest_ReturnsUpdatedStatusCode_ValidArticleLocationContentResponse()
        {
            var createArticleRequest = "/api/v1/Articles/create";
            CreateArticleRequest createArticleRequestDto = TestArticleHelper.CreateCreateArticleRequest(11);
            var createArticleRequestContent = new StringContent(JsonConvert.SerializeObject(createArticleRequestDto), Encoding.UTF8, "application/json");
            await _client.PostAsync(createArticleRequest, createArticleRequestContent);
            var createLocationRequest = "/api/v1/Locations/create";
            CreateLocationRequest locationRequest = TestLocationHelper.CreateCreateLocationRequest(11);
            var createLocationRequestContent = new StringContent(JsonConvert.SerializeObject(locationRequest), Encoding.UTF8, "application/json");
            await _client.PostAsync(createLocationRequest, createLocationRequestContent);
            
            var createRequest = "/api/v1/ArticleLocations/create";
            CreateArticleLocationRequest createRequestDto = TestArticleLocationHelper.CreateCreateArticleLocationRequest(11);
            var createRequestContent = new StringContent(JsonConvert.SerializeObject(createRequestDto), Encoding.UTF8, "application/json");
            var createdResponse = await _client.PostAsync(createRequest, createRequestContent);

            var responseString = await createdResponse.Content.ReadAsStringAsync();
            var created = JsonConvert.DeserializeObject<ArticleLocation>(responseString);
            
            var updateRequest = "/api/v1/ArticleLocations/update";
            UpdateArticleLocationRequest request = TestArticleLocationHelper.CreateUpdateArticleLocationRequest(created.Id);
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            var response = await _client.PutAsync(updateRequest, content);

            Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);

            var deleteRequest = "/api/v1/ArticleLocations/delete?articleCode=11&locationCode=11";
            await _client.DeleteAsync(deleteRequest);
        }

        [Fact]
        public async Task DeleteArticleLocation_ArticleLocationDoesNotExist_ReturnsNotFoundStatusCode()
        {
            var deleteRequest = "/api/v1/ArticleLocations/delete?articleCode=12&locationCode=12";

            var response = await _client.DeleteAsync(deleteRequest);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DeleteArticleLocation_ArticleLocationExists_ReturnsAcceptedStatusCode_ValidArticleLocationContentResponse()
        {
            var createArticleRequest = "/api/v1/Articles/create";
            CreateArticleRequest createArticleRequestDto = TestArticleHelper.CreateCreateArticleRequest(13);
            var createArticleRequestContent = new StringContent(JsonConvert.SerializeObject(createArticleRequestDto), Encoding.UTF8, "application/json");
            await _client.PostAsync(createArticleRequest, createArticleRequestContent);
            var createLocationRequest = "/api/v1/Locations/create";
            CreateLocationRequest locationRequest = TestLocationHelper.CreateCreateLocationRequest(13);
            var createLocationRequestContent = new StringContent(JsonConvert.SerializeObject(locationRequest), Encoding.UTF8, "application/json");
            await _client.PostAsync(createLocationRequest, createLocationRequestContent);
            
            var createRequest = "/api/v1/ArticleLocations/create";
            CreateArticleLocationRequest request = TestArticleLocationHelper.CreateCreateArticleLocationRequest(13);
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            await _client.PostAsync(createRequest, content);

            var deleteRequest = "/api/v1/ArticleLocations/delete?articleCode=13&locationCode=13";
    
            var response = await _client.DeleteAsync(deleteRequest);

            Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
        }
    }