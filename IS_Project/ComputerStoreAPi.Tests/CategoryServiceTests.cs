using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ComputerStore.BLL.DTOs;
using ComputerStore.BLL.Profiles;
using ComputerStore.BLL.Services;
using ComputerStore.DAL.Entities;
using ComputerStore.DAL.Interfaces;
using Moq;

namespace ComputerStore.Tests
{
    public class CategoryServiceTests
    {
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
        private readonly IMapper _mapper;
        private readonly CategoryService _categoryService;

        public CategoryServiceTests()
        {
            _categoryRepositoryMock = new Mock<ICategoryRepository>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = config.CreateMapper();

            _categoryService = new CategoryService(
                _categoryRepositoryMock.Object,
                _mapper
            );
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnBadRequest_WhenNameIsEmpty()
        {
            var request = new CategoryRequest
            {
                Name = "",
                Description = "Test description"
            };

            var result = await _categoryService.CreateAsync(request);

            Assert.False(result.IsSuccess);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Category name is required.", result.Error);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnConflict_WhenCategoryAlreadyExists()
        {
            var request = new CategoryRequest
            {
                Name = "CPU",
                Description = "Computer processors"
            };

            _categoryRepositoryMock
                .Setup(repo => repo.GetByNameAsync("CPU"))
                .ReturnsAsync(new Category
                {
                    Id = 1,
                    Name = "CPU",
                    Description = "Existing category"
                });

            var result = await _categoryService.CreateAsync(request);

            Assert.False(result.IsSuccess);
            Assert.Equal(409, result.StatusCode);
            Assert.Equal("Category with name 'CPU' already exists.", result.Error);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnCreated_WhenCategoryIsValid()
        {
            var request = new CategoryRequest
            {
                Name = "GPU",
                Description = "Graphics cards"
            };

            _categoryRepositoryMock
                .Setup(repo => repo.GetByNameAsync("GPU"))
                .ReturnsAsync((Category?)null);

            _categoryRepositoryMock
                .Setup(repo => repo.AddAsync(It.IsAny<Category>()))
                .Returns(Task.CompletedTask);

            var result = await _categoryService.CreateAsync(request);

            Assert.True(result.IsSuccess);
            Assert.Equal(201, result.StatusCode);
            Assert.NotNull(result.Data);
            Assert.Equal("GPU", result.Data!.Name);
            Assert.Equal("Graphics cards", result.Data.Description);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnConflict_WhenCategoryIsUsedByProducts()
        {
            var category = new Category
            {
                Id = 1,
                Name = "CPU",
                Description = "Computer processors",
                ProductCategories = new List<ProductCategory>
                {
                    new ProductCategory
                    {
                        ProductId = 1,
                        CategoryId = 1
                    }
                }
            };

            _categoryRepositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(category);

            var result = await _categoryService.DeleteAsync(1);

            Assert.False(result.IsSuccess);
            Assert.Equal(409, result.StatusCode);
            Assert.Equal("Category cannot be deleted because it is used by products.", result.Error);
        }
    }
}