using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ReqestDTOs.Meeting
{
    public class UpdateMeetingStatus
    {
        public int RequestId { get; set; }
        public int PharmacyId { get; set; }
        public int StatusId { get; set; }
    }
}