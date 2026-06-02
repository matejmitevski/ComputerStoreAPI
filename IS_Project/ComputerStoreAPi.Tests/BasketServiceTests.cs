using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComputerStore.BLL.DTOs;
using ComputerStore.BLL.Services;
using ComputerStore.DAL.Entities;
using ComputerStore.DAL.Interfaces;
using Moq;

namespace ComputerStore.Tests
{
    public class BasketServiceTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly BasketService _basketService;

        public BasketServiceTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _basketService = new BasketService(_productRepositoryMock.Object);
        }

        [Fact]
        public async Task CalculateDiscountAsync_ShouldReturnBadRequest_WhenBasketIsEmpty()
        {
            var request = new BasketRequest
            {
                Items = new List<BasketItemRequest>()
            };

            var result = await _basketService.CalculateDiscountAsync(request);

            Assert.False(result.IsSuccess);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Basket cannot be empty.", result.Error);
        }

        [Fact]
        public async Task CalculateDiscountAsync_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            var request = new BasketRequest
            {
                Items = new List<BasketItemRequest>
                {
                    new BasketItemRequest
                    {
                        ProductId = 99,
                        Quantity = 1
                    }
                }
            };

            _productRepositoryMock
                .Setup(repo => repo.GetByIdAsync(99))
                .ReturnsAsync((Product?)null);

            var result = await _basketService.CalculateDiscountAsync(request);

            Assert.False(result.IsSuccess);
            Assert.Equal(404, result.StatusCode);
            Assert.Equal("Product with id '99' was not found.", result.Error);
        }

        [Fact]
        public async Task CalculateDiscountAsync_ShouldReturnBadRequest_WhenStockIsNotEnough()
        {
            var product = new Product
            {
                Id = 1,
                Name = "Intel CPU",
                Price = 475.99m,
                Quantity = 2
            };

            var request = new BasketRequest
            {
                Items = new List<BasketItemRequest>
                {
                    new BasketItemRequest
                    {
                        ProductId = 1,
                        Quantity = 999
                    }
                }
            };

            _productRepositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(product);

            var result = await _basketService.CalculateDiscountAsync(request);

            Assert.False(result.IsSuccess);
            Assert.Equal(400, result.StatusCode);
            Assert.Contains("Not enough stock", result.Error);
        }

        [Fact]
        public async Task CalculateDiscountAsync_ShouldApplyDiscount_WhenMultipleProductsSameCategory()
        {
            var cpuCategory = new Category
            {
                Id = 1,
                Name = "CPU"
            };

            var product = new Product
            {
                Id = 1,
                Name = "Intel CPU",
                Price = 475.99m,
                Quantity = 10,
                ProductCategories = new List<ProductCategory>
                {
                    new ProductCategory
                    {
                        ProductId = 1,
                        CategoryId = 1,
                        Category = cpuCategory
                    }
                }
            };

            var request = new BasketRequest
            {
                Items = new List<BasketItemRequest>
                {
                    new BasketItemRequest
                    {
                        ProductId = 1,
                        Quantity = 2
                    }
                }
            };

            _productRepositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(product);

            var result = await _basketService.CalculateDiscountAsync(request);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(951.98m, result.Data!.Subtotal);
            Assert.Equal(23.80m, result.Data.Discount);
            Assert.Equal(928.18m, result.Data.Total);
        }
    }
}