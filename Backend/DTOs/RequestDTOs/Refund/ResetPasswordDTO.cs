using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.RequestDTOs.Refund
{
    public class ResetPasswordDTO
    {
        public int PersonId { get;set; }
        public string Password { get;set; }
        public string VerficationCode { get; set; }
    }
}
