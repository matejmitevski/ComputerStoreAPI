using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComputerStore.BLL.Common;
using ComputerStore.BLL.DTOs;

namespace ComputerStore.BLL.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductResponse>> GetAllAsync();

        Task<ServiceResult<ProductResponse>> GetByIdAsync(int id);

        Task<ServiceResult<ProductResponse>> CreateAsync(ProductRequest request);

        Task<ServiceResult<ProductResponse>> UpdateAsync(int id, ProductRequest request);

        Task<ServiceResult<bool>> DeleteAsync(int id);
    }
}