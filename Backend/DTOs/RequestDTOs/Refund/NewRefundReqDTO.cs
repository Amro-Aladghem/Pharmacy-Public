using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ReqestDTOs.Refund
{
    public class NewRefundReqDTO
    {
        public int CustomerId { get; set; }
        public string TypeName { get; set; }
        public int RefferenceId { get; set; }
        public string? AdditionalInformation { get; set; }
    }
}
