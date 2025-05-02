using DTOs.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.OrderDTOs
{
    public class OrderItemDTO
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

    }
}
