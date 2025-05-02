using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.CartDTOs
{
    public class UpdateCartItemDTO
    {
        public int ItemId { get; set; }
        public int Quantity { get; set; }
    }
}
