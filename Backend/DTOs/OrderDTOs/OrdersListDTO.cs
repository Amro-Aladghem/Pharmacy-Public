using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.OrderDTOs
{
    public class OrdersListDTO
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public OrderStatusDTO Status { get; set; }
    }
}
