using DTOs.CustomerDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.OrderDTOs
{
    //This class For listing history
    public class OrderDetailForAdmin
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public OrderStatusDTO Status { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalPrice { get; set; }

    }
}
