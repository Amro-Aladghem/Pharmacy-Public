using DTOs.ReqestDTOs.Meeting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.RequestDTOs.Meeting
{
    public class MeetingReqInfoForAdminDTO
    {
        public int RequestId { get; set; }
        public int CustomerId { get; set; }
        public MeetingReqStatusDTO Status { get; set; }
    }
}
