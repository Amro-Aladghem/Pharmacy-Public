using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ReqestDTOs
{
    public class EmailVerficationResultDTO
    {
        public bool IsRightCode { get; set; }
        public string message { get; set; }
        public bool Isblocked { get; set; }
    }
}
