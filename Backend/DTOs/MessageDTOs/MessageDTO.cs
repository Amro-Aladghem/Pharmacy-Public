using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.MessageDTOs
{
    public class MessageDTO
    {
        public int MessageId { get; set; }
        public string Message { get; set; } 
        public int UserTypeId { get; set; }
    }
}
