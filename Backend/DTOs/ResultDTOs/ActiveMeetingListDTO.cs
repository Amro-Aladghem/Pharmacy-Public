using DTOs.RequestDTOs.Meeting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ResultDTOs
{
    public class ActiveMeetingListDTO
    {
        public List<MeetingReqInfoForAdminDTO> Meetings { get; set; }
        public int PageNumber { get; set; }
        public string? PrevPage { get; set; }
        public string? NextPage { get; set; }
        public int Total { get; set; }
    }
}
