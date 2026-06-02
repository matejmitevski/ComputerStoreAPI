using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComputerStore.BLL.Common;
using ComputerStore.BLL.DTOs;
using ComputerStore.BLL.Interfaces;
using ComputerStore.DAL.Entities;
using ComputerStore.DAL.Interfaces;

namespace ComputerStore.BLL.Services
{
    public class StockImportService : IStockImportService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public StockImportService(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<ServiceResult<string>> ImportAsync(List<StockImportRequest> requests)
        {
            if (requests is null || !requests.Any())
                return ServiceResult<string>.BadRequest("Import list cannot be empty.");

            foreach (var item in requests)
            {
                if (string.IsNullOrWhiteSpace(item.Name))
                    return ServiceResult<string>.BadRequest("Product name is required in import data.");

                if (item.Price <= 0)
                    return ServiceResult<string>.BadRequest($"Price for product '{item.Name}' must be greater than 0.");

                if (item.Quantity < 0)
                    return ServiceResult<string>.BadRequest($"Quantity for product '{item.Name}' cannot be negative.");

                if (item.Categories is null || !item.Categories.Any())
                    return ServiceResult<string>.BadRequest($"Product '{item.Name}' must have at least one category.");

                var categoryIds = new List<int>();

                foreach (var categoryNameRaw in item.Categories)
                {
                    var categoryName = categoryNameRaw.Trim();

                    if (string.IsNullOrWhiteSpace(categoryName))
                        continue;

                    var category = await _categoryRepository.GetByNameAsync(categoryName);

                    if (category is null)
                    {
                        category = new Category
                        {
                            Name = categoryName,
                            Description = "Created automatically during stock import."
                        };

                        await _categoryRepository.AddAsync(category);
                    }

                    categoryIds.Add(category.Id);
                }

                var productName = item.Name.Trim();

                var product = await _productRepository.GetByNameAsync(productName);

                if (product is null)
                {
                    product = new Product
                    {
                        Name = productName,
                        Price = item.Price,
                        Quantity = item.Quantity,
                        ProductCategories = categoryIds.Distinct()
                            .Select(id => new ProductCategory
                            {
                                CategoryId = id
                            })
                            .ToList()
                    };

                    await _productRepository.AddAsync(product);
                }
                else
                {
                    product.Price = item.Price;
                    product.Quantity += item.Quantity;

                    product.ProductCategories.Clear();

                    foreach (var categoryId in categoryIds.Distinct())
                    {
                        product.ProductCategories.Add(new ProductCategory
                        {
                            ProductId = product.Id,
                            CategoryId = categoryId
                        });
                    }

                    await _productRepository.UpdateAsync(product);
                }
            }

            return ServiceResult<string>.Ok("Stock information imported successfully.");
        }
    }
}