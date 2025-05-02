using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.RequestDTOs.Meeting
{
    //For listing Req history table for Customer
    public class MeetingReqSummeryInfoDTO
    {
        public int RequsetId { get; set; }
        public string PharmacyName { get; set; }
        public string PharmacyImageURL { get; set; }
        public string StatusName { get; set; }
        public string StatusArabicName { get; set; }
        public int StatusId { get; set; }
        public DateTime Date { get; set; }
    }
}
