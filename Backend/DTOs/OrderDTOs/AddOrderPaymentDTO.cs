using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.OrderDTOs
{
    public class AddOrderPaymentDTO
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public int PaymentMethodeId { get; set; }
        public decimal TotalPrice { get; set; } 

    }
}
