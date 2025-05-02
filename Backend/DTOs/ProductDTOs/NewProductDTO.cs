using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ProductDTOs
{
    public class NewProductDTO
    {
        public int PharmacyId { get; set; }
        public SysProductDTO SysProduct { get; set; }
        public PhProductDTO PhProduct { get; set; }
    }
}
