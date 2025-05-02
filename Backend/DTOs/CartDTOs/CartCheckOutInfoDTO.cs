using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.CartDTOs
{
    public class CartCheckOutInfoDTO
    {
        public int CartId { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal DeliveryFees { get; set; } = 0;
        public decimal SubPrice { get; set; }
        public decimal ServiceFees { get; set; }
    }
}
