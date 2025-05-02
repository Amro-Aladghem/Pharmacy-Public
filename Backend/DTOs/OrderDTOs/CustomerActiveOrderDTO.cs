using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.OrderDTOs
{
    public class CustomerActiveOrderDTO
    {
        public int Id { get; set; }
        public OrderStatusDTO Status { get; set; }
        public int PharmacyId { get; set; }
        public string PharmacyName { get; set; }
        public string PhImageURL { get; set; }
        public DateTime Date { get; set; }
        public bool IsNotAccepted { get; set; }
        public bool IsTheSameDay { get; set; }

    }
}
