using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerStore.BLL.DTOs
{
    public class BasketRequest
    {
        public List<BasketItemRequest> Items { get; set; } = new();
    }
}