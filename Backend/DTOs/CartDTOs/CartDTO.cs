using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.CartDTOs
{
    public class CartDTO
    {
        public int CartId { get; set; }
        public int? PharmacyId { get; set; }
        public int NumberOfItems { get; set; }
    }
}
