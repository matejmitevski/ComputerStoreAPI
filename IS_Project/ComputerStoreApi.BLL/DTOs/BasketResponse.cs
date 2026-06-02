using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerStore.BLL.DTOs
{
    public class BasketResponse
    {
        public decimal Subtotal { get; set; }

        public decimal Discount { get; set; }

        public decimal Total { get; set; }

        public string Message { get; set; } = string.Empty;
    }
}