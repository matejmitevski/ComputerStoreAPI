using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComputerStore.BLL.Common;
using ComputerStore.BLL.DTOs;

namespace ComputerStore.BLL.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryResponse>> GetAllAsync();

        Task<ServiceResult<CategoryResponse>> GetByIdAsync(int id);

        Task<ServiceResult<CategoryResponse>> CreateAsync(CategoryRequest request);

        Task<ServiceResult<CategoryResponse>> UpdateAsync(int id, CategoryRequest request);

        Task<ServiceResult<bool>> DeleteAsync(int id);
    }
}