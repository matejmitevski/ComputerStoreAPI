using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComputerStore.BLL.DTOs;
using System.Net;
using System.Net.Http.Json;

namespace ComputerStore.IntegrationTests
{
    public class CategoriesControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public CategoriesControllerIntegrationTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task PostCategory_ShouldReturnCreated()
        {
            var request = new CategoryRequest
            {
                Name = "GPU",
                Description = "Graphics cards"
            };

            var response = await _client.PostAsJsonAsync("/api/Categories", request);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var category = await response.Content.ReadFromJsonAsync<CategoryResponse>();

            Assert.NotNull(category);
            Assert.Equal("GPU", category!.Name);
            Assert.Equal("Graphics cards", category.Description);
        }

        [Fact]
        public async Task GetCategories_ShouldReturnOk()
        {
            var response = await _client.GetAsync("/api/Categories");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task PostCategory_ShouldReturnBadRequest_WhenNameIsEmpty()
        {
            var request = new CategoryRequest
            {
                Name = "",
                Description = "Invalid category"
            };

            var response = await _client.PostAsJsonAsync("/api/Categories", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}