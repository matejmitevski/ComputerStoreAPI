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
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryResponse>> GetAllAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<CategoryResponse>>(categories);
        }

        public async Task<ServiceResult<CategoryResponse>> GetByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category is null)
                return ServiceResult<CategoryResponse>.NotFound($"Category with id '{id}' was not found.");

            return ServiceResult<CategoryResponse>.Ok(_mapper.Map<CategoryResponse>(category));
        }

        public async Task<ServiceResult<CategoryResponse>> CreateAsync(CategoryRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return ServiceResult<CategoryResponse>.BadRequest("Category name is required.");

            var name = request.Name.Trim();

            var existing = await _categoryRepository.GetByNameAsync(name);
            if (existing is not null)
                return ServiceResult<CategoryResponse>.Conflict($"Category with name '{name}' already exists.");

            var category = new Category
            {
                Name = name,
                Description = request.Description
            };

            await _categoryRepository.AddAsync(category);

            return ServiceResult<CategoryResponse>.Created(_mapper.Map<CategoryResponse>(category));
        }

        public async Task<ServiceResult<CategoryResponse>> UpdateAsync(int id, CategoryRequest request)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category is null)
                return ServiceResult<CategoryResponse>.NotFound($"Category with id '{id}' was not found.");

            if (string.IsNullOrWhiteSpace(request.Name))
                return ServiceResult<CategoryResponse>.BadRequest("Category name is required.");

            var name = request.Name.Trim();

            var duplicate = await _categoryRepository.GetByNameAsync(name);
            if (duplicate is not null && duplicate.Id != id)
                return ServiceResult<CategoryResponse>.Conflict($"Category with name '{name}' already exists.");

            category.Name = name;
            category.Description = request.Description;

            await _categoryRepository.UpdateAsync(category);

            return ServiceResult<CategoryResponse>.Ok(_mapper.Map<CategoryResponse>(category));
        }

        public async Task<ServiceResult<bool>> DeleteAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category is null)
                return ServiceResult<bool>.NotFound($"Category with id '{id}' was not found.");

            if (category.ProductCategories.Any())
                return ServiceResult<bool>.Conflict("Category cannot be deleted because it is used by products.");

            await _categoryRepository.DeleteAsync(category);

            return ServiceResult<bool>.NoContent();
        }
    }
}