using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ProductDTOs
{
    public class PaginatedPhProductDTO
    {
        public int Limit { get; set; }
        public int LastPhProductId { get; set; }
        public bool IsRowsCountCalculated { get; set; } = false;
    }
}
