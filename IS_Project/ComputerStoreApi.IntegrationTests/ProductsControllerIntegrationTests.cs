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
    public class ProductsControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public ProductsControllerIntegrationTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task PostProduct_ShouldReturnCreated_WhenRequestIsValid()
        {
            var categoryRequest = new CategoryRequest
            {
                Name = "CPU",
                Description = "Computer processors"
            };

            var categoryResponse = await _client.PostAsJsonAsync("/api/Categories", categoryRequest);
            var category = await categoryResponse.Content.ReadFromJsonAsync<CategoryResponse>();

            var productRequest = new ProductRequest
            {
                Name = "Intel Core i9",
                Description = "High performance CPU",
                Price = 475.99m,
                Quantity = 5,
                CategoryIds = new List<int> { category!.Id }
            };

            var productResponse = await _client.PostAsJsonAsync("/api/Products", productRequest);

            Assert.Equal(HttpStatusCode.Created, productResponse.StatusCode);

            var product = await productResponse.Content.ReadFromJsonAsync<ProductResponse>();

            Assert.NotNull(product);
            Assert.Equal("Intel Core i9", product!.Name);
            Assert.Equal(475.99m, product.Price);
        }

        [Fact]
        public async Task PostProduct_ShouldReturnBadRequest_WhenCategoryDoesNotExist()
        {
            var productRequest = new ProductRequest
            {
                Name = "Invalid Product",
                Description = "No real category",
                Price = 100,
                Quantity = 2,
                CategoryIds = new List<int> { 999 }
            };

            var response = await _client.PostAsJsonAsync("/api/Products", productRequest);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}