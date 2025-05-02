using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.CartDTOs
{
    public class UpdateCartItemResutlDTO
    {
        public int ItemId { get; set; } 
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal SubPriceDiff { get; set; }
        public decimal TotalPriceDiff { get; set; }
    }
}
