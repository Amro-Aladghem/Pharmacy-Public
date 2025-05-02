using DTOs.ReqestDTOs.Meeting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ResultDTOs
{
    public class AddingMeetingResultDTO
    {
        public bool IsSuccess { get; set; }
        public MeetingReqStatusDTO Status { get; set; }
    }
}
