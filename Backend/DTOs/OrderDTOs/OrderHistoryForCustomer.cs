using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.OrderDTOs
{

    //This class For listing history
    public class OrderHistoryForCustomer
    {
        public int Id { get; set; }
        public OrderStatusDTO Status { get; set; }
        public int PharmacyId { get; set; }
        public string PharmacyName { get; set; }
        public string PhImageURL { get; set; }
        public DateTime Date { get; set; }
    }
}
