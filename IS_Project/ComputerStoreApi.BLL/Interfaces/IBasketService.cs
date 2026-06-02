using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComputerStore.BLL.Common;
using ComputerStore.BLL.DTOs;

namespace ComputerStore.BLL.Interfaces
{
    public interface IBasketService
    {
        Task<ServiceResult<BasketResponse>> CalculateDiscountAsync(BasketRequest request);
    }
}