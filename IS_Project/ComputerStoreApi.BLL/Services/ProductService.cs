using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ComputerStore.BLL.Common;
using ComputerStore.BLL.DTOs;
using ComputerStore.BLL.Interfaces;
using ComputerStore.DAL.Entities;
using ComputerStore.DAL.Interfaces;

namespace ComputerStore.BLL.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public ProductService(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IMapper mapper)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductResponse>> GetAllAsync()
        {
            var products = await _productRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ProductResponse>>(products);
        }

        public async Task<ServiceResult<ProductResponse>> GetByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product is null)
                return ServiceResult<ProductResponse>.NotFound($"Product with id '{id}' was not found.");

            return ServiceResult<ProductResponse>.Ok(_mapper.Map<ProductResponse>(product));
        }

        public async Task<ServiceResult<ProductResponse>> CreateAsync(ProductRequest request)
        {
            var validation = await ValidateProductRequestAsync(request);

            if (!validation.IsSuccess)
                return validation;

            var name = request.Name.Trim();

            var existing = await _productRepository.GetByNameAsync(name);
            if (existing is not null)
                return ServiceResult<ProductResponse>.Conflict($"Product with name '{name}' already exists.");

            var product = new Product
            {
                Name = name,
                Description = request.Description,
                Price = request.Price,
                Quantity = request.Quantity
            };

            foreach (var categoryId in request.CategoryIds.Distinct())
            {
                product.ProductCategories.Add(new ProductCategory
                {
                    CategoryId = categoryId
                });
            }

            await _productRepository.AddAsync(product);

            var createdProduct = await _productRepository.GetByIdAsync(product.Id);

            return ServiceResult<ProductResponse>.Created(_mapper.Map<ProductResponse>(createdProduct));
        }

        public async Task<ServiceResult<ProductResponse>> UpdateAsync(int id, ProductRequest request)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product is null)
                return ServiceResult<ProductResponse>.NotFound($"Product with id '{id}' was not found.");

            var validation = await ValidateProductRequestAsync(request);

            if (!validation.IsSuccess)
                return validation;

            var name = request.Name.Trim();

            var duplicate = await _productRepository.GetByNameAsync(name);
            if (duplicate is not null && duplicate.Id != id)
                return ServiceResult<ProductResponse>.Conflict($"Product with name '{name}' already exists.");

            product.Name = name;
            product.Description = request.Description;
            product.Price = request.Price;
            product.Quantity = request.Quantity;

            product.ProductCategories.Clear();

            foreach (var categoryId in request.CategoryIds.Distinct())
            {
                product.ProductCategories.Add(new ProductCategory
                {
                    ProductId = product.Id,
                    CategoryId = categoryId
                });
            }

            await _productRepository.UpdateAsync(product);

            var updatedProduct = await _productRepository.GetByIdAsync(product.Id);

            return ServiceResult<ProductResponse>.Ok(_mapper.Map<ProductResponse>(updatedProduct));
        }

        public async Task<ServiceResult<bool>> DeleteAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product is null)
                return ServiceResult<bool>.NotFound($"Product with id '{id}' was not found.");

            await _productRepository.DeleteAsync(product);

            return ServiceResult<bool>.NoContent();
        }

        private async Task<ServiceResult<ProductResponse>> ValidateProductRequestAsync(ProductRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return ServiceResult<ProductResponse>.BadRequest("Product name is required.");

            if (request.Price <= 0)
                return ServiceResult<ProductResponse>.BadRequest("Product price must be greater than 0.");

            if (request.Quantity < 0)
                return ServiceResult<ProductResponse>.BadRequest("Product quantity cannot be negative.");

            if (request.CategoryIds is null || !request.CategoryIds.Any())
                return ServiceResult<ProductResponse>.BadRequest("At least one category is required.");

            foreach (var categoryId in request.CategoryIds.Distinct())
            {
                var category = await _categoryRepository.GetByIdAsync(categoryId);

                if (category is null)
                    return ServiceResult<ProductResponse>.BadRequest($"Category with id '{categoryId}' does not exist.");
            }

            return ServiceResult<ProductResponse>.Ok(new ProductResponse());
        }
    }
}