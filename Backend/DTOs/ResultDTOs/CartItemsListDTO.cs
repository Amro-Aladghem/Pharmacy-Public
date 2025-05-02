using DTOs.CartDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ResultDTOs
{
    public class CartItemsListDTO
    {
        public int CartId { get; set; }
        public List<ListingCartItemDTO> CartItmes { get; set; } 
        public decimal TotalPrice { get; set; }
        public decimal DeliveryFees { get; set; } = 0;
        public decimal SubPrice { get; set; }
        public decimal ServiceFees { get; set; }
    }
}
