using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.MessageDTOs
{
    public class NewMessageDTO
    {
        public int UserTypeId { get; set; }
        public int CustomerId { get; set; }
        public  int ?AdmingId { get; set; }
        public DateOnly DateOfMessage { get; set; }
        public TimeOnly Time { get; set; }
        public string Message { get; set; }
        public int PharmacyId { get; set; }
    }
}
