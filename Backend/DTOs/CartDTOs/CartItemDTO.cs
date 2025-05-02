using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.CartDTOs
{
    public class CartItemDTO
    {
        public int PhProductId { get; set; }
        public int PharmacyId { get; set; }
        public int Quantity { get; set; }
    }
}
