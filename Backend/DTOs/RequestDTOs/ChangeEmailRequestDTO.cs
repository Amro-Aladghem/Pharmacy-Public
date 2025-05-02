using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.RequestDTOs
{
    public class ChangeEmailRequestDTO
    {
        public int UserId { get; set; }
        public string NewEmail { get; set; }
        public string VerficationCode { get; set; }
    }
}
