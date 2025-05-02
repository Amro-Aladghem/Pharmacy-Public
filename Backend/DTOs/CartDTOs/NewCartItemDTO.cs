using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.CartDTOs
{
    public class NewCartItemDTO
    {
        public int CartId { get; set; }
        public int CustomerId { get; set; }
        public CartItemDTO CartItem { get; set; }

    }
}
