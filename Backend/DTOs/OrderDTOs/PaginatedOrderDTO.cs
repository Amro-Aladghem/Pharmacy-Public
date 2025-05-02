using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.OrderDTOs
{
    public class PaginatedOrderDTO
    {
        public int PharmacyId { get; set; }
        public int PageNumber { get; set; }
        public int Limit { get; set; }
    }
}
