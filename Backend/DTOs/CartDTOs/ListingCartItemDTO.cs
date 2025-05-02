using DTOs.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.CartDTOs
{
    public class ListingCartItemDTO
    {
        public int ItemId { get; set; }
        public PhProductSummeryDTO Product { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }  
    }
}
