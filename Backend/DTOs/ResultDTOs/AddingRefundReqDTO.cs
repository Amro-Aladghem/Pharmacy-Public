using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ResultDTOs
{
    public class AddingRefundReqDTO
    {
        public bool IsAccepted { get; set; }
        public string message {  get; set; } 
        public bool IsError { get; set; }
    }
}
