using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ReqestDTOs.Refund
{
    public class RefundReqDetailDTO
    {
        public int Id { get; set; }
        public DateTime DateAndTimeOfRequest { get; set; }
        public int RefferenceId { get; set; }
        public string TypeName { get; set; }
        public RefundStatusDTO Status { get; set; } 
    }
}
