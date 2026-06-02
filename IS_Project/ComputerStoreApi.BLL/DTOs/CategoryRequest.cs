using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerStore.BLL.DTOs
{
    public class CategoryRequest
    {
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}