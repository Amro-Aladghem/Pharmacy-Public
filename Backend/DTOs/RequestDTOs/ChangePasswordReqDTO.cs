using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.RequestDTOs
{
    public class ChangePasswordReqDTO
    {
        public int UserId { get; set; }
        public string currentPassowrd { get; set; }
        public string NewPassword { get; set; }
    }
}
