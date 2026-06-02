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
    public class BasketService : IBasketService
    {
        private readonly IProductRepository _productRepository;

        public BasketService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ServiceResult<BasketResponse>> CalculateDiscountAsync(BasketRequest request)
        {
            if (request.Items is null || !request.Items.Any())
                return ServiceResult<BasketResponse>.BadRequest("Basket cannot be empty.");

            var basketProducts = new List<(Product Product, int Quantity)>();

            foreach (var item in request.Items)
            {
                if (item.Quantity <= 0)
                    return ServiceResult<BasketResponse>.BadRequest("Product quantity must be greater than 0.");

                var product = await _productRepository.GetByIdAsync(item.ProductId);

                if (product is null)
                    return ServiceResult<BasketResponse>.NotFound($"Product with id '{item.ProductId}' was not found.");

                if (item.Quantity > product.Quantity)
                    return ServiceResult<BasketResponse>.BadRequest(
                        $"Not enough stock for product '{product.Name}'. Available: {product.Quantity}, requested: {item.Quantity}.");

                basketProducts.Add((product, item.Quantity));
            }

            var subtotal = basketProducts.Sum(x => x.Product.Price * x.Quantity);
            decimal discount = 0;

            var totalItems = basketProducts.Sum(x => x.Quantity);

            if (totalItems > 1)
            {
                var categoryGroups = new Dictionary<int, List<Product>>();

                foreach (var basketProduct in basketProducts)
                {
                    for (int i = 0; i < basketProduct.Quantity; i++)
                    {
                        foreach (var productCategory in basketProduct.Product.ProductCategories)
                        {
                            var categoryId = productCategory.CategoryId;

                            if (!categoryGroups.ContainsKey(categoryId))
                                categoryGroups[categoryId] = new List<Product>();

                            categoryGroups[categoryId].Add(basketProduct.Product);
                        }
                    }
                }

                var discountedProductIds = new HashSet<int>();

                foreach (var group in categoryGroups)
                {
                    if (group.Value.Count > 1)
                    {
                        var firstProduct = group.Value.First();

                        if (!discountedProductIds.Contains(firstProduct.Id))
                        {
                            discount += firstProduct.Price * 0.05m;
                            discountedProductIds.Add(firstProduct.Id);
                        }
                    }
                }
            }

            var response = new BasketResponse
            {
                Subtotal = Math.Round(subtotal, 2),
                Discount = Math.Round(discount, 2),
                Total = Math.Round(subtotal - discount, 2),
                Message = discount > 0
                    ? "Discount calculated successfully."
                    : "No discount applied."
            };

            return ServiceResult<BasketResponse>.Ok(response);
        }
    }
}