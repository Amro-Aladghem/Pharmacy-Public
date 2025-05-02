using DTOs.OrderDTOs;
using DTOs.ReqestDTOs.Meeting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.RequestDTOs.Meeting
{
    public class CustomerActiveRequestDTO
    {
        public int Id { get; set; }
        public MeetingReqStatusDTO Status { get; set; }
        public int PharmacyId { get; set; }
        public string PharmacyName { get; set; }
        public string PhImageURL { get; set; }
        public DateTime Date { get; set; }
        public bool IsNotAccepted { get; set; }
        public bool IsTheSameDay { get; set; }

    }
}
