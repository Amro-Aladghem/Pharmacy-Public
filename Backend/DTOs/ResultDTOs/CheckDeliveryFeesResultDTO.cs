using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ResultDTOs
{
    public class CheckDeliveryFeesResultDTO
    {
        public bool IsDelivered { get; set; }
        public decimal? Fees { get; set; } = 0;
    }
}
