using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.OrderDTOs
{
    public class InvoiceDTO
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public bool IsPaid { get; set; }

    }
}
