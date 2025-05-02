using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.OrderDTOs
{
    public class NewOrderItemDTO
    {
        public int OrderId { get; set; }
        public int PhProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }  
    }
}
