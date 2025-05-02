using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.OrderDTOs
{
    public class CheckOutDTO
    {
        public int CartId { get; set; }
        public int CustomerId { get; set; }
        public int PaymentMethodId { get; set; }
    }
}
