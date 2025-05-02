using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.RequestDTOs.Meeting
{
    public class PagenaitedReqHistoryDTO
    {
        public int PharmacyId { get; set; } 
        public int PageNumber { get; set; }
        public int Limit { get; set; }
        public int LastReqMeetingId { get; set; }
       
    }
}
