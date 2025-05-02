using DTOs.PharamacyDTOs;
using DTOs.ReqestDTOs.Meeting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.RequestDTOs.Meeting
{
    public class RequestInfoDTO
    {
        public int RequestId { get; set; }
        public PharmacyDTO Pharmacy { get; set; }
        public MeetingReqStatusDTO MeetingReqStatus { get; set; }
        public DateTime DateOfRequest { get; set; }
        public decimal Price { get; set; }
    }
}
