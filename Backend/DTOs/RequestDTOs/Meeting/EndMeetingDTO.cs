using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ReqestDTOs.Meeting
{
    public class EndMeetingDTO
    {
        public int PharmacyId { get; set; }
        public int UserId { get; set; }
        public string role { get; set; } = "admin";
        public int RequestId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TimeOnly Duration { get; set; }
    }
}
