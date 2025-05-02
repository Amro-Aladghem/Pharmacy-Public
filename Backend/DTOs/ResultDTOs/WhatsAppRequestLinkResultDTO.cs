using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ResultDTOs
{
    public class WhatsAppRequestLinkResultDTO
    {
        public string? url { get; set; } = "";
        public bool IsDone { get; set; }
        public string? ErrorMessage { get; set; } = "";
    }
}
