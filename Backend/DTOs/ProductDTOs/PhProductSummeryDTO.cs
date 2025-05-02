using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ProductDTOs
{
    public class PhProductSummeryDTO
    {
        public int PhProductId { get; set; }
        public string PhProductName { get; set; }
        public string ?ImageURL { get; set; }
        public decimal? Stoke { get; set; }
        public decimal? Price { get; set; }
    }
}
