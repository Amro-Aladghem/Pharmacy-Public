using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ReqestDTOs.Meeting
{
    public class MeetingReqStatusDTO
    {
        public int Id { get; set; }
        public int RequestId { get; set; }
        public string Name { get; set; }
        public string ArabicName { get; set; }
        public string? MeetingURL { get; set; }
    }
}
