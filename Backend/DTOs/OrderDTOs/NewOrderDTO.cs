using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.OrderDTOs
{
    public class NewOrderDTO
    {
        public int CustomerId { get; set; }
        public int PharmacyId { get; set; }
        public decimal TotalPrice { get; set; }

    }
}
