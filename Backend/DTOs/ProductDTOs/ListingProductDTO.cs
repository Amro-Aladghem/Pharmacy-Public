using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ProductDTOs
{
    public class ListingProductDTO
    {
        public int PhProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ProductImageURL { get; set; }
        public string PhName { get; set; }
        public int PharmacyId { get; set; }

    }
}