using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.RequestDTOs.Meeting
{
    public class StartMeetingDTO
    {
        public int RequestId { get; set; }
        public int PharmacyId { get; set; }
        public string MeetingURL { get; set; }
    }
}
