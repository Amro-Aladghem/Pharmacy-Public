using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.PharmacyDTOs
{
    public class PaginatedPharmacyListDTO
    {
        public int LastPharmacyId { get; set; }
        public int Limit { get; set; }
        public bool IsRowsCountCalculated { get; set; } 
    }
}
