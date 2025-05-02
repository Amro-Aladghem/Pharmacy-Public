using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ReqestDTOs.Refund
{
    public class UpdateRefundReqStatusDTO
    {
        public int AdminId { get; set; }
        public int StatusId { get; set; }
        public int RefundReqId { get; set; }
    }
}
