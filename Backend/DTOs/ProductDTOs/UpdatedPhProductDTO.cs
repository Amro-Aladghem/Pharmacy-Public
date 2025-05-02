using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ProductDTOs
{
    public class UpdatedPhProductDTO
    {
        public int PhProductId { get; set; }
        public int PharmacyId { get; set; }
        public PhProductDTO PhProduct { get; set; }
    }
}
