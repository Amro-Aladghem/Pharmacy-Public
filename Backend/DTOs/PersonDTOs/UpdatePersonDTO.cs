using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.PersonDTOs
{
    public class UpdatePersonDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? UserName => FirstName + " " + LastName;
        public string? ProfileImageLink { get; set; }
        public string Phone { get; set; }
    }
}
