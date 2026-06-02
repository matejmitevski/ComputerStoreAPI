using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerStore.BLL.DTOs
{
    public class StockImportRequest
    {
        public string Name { get; set; } = string.Empty;

        public List<string> Categories { get; set; } = new();

        public decimal Price { get; set; }

        public int Quantity { get; set; }
    }
}