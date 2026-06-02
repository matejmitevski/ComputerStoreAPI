using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerStore.BLL.DTOs
{
    public class BasketItemRequest
    {
        public int ProductId { get; set; }

        public int Quantity { get; set; }
    }
}