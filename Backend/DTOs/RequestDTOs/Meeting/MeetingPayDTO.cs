using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.RequestDTOs.Meeting
{
    public class MeetingPayDTO
    {
        public int RequestId { get; set; }
        public int CustomerId { get; set; }
        public decimal PaidAmount { get;set;}
        public int PharmacyId { get; set; }
    }
}
