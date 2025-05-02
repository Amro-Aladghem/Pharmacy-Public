using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.PersonDTOs
{
    public class PersonRegisterDTO
    {
        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? ProfileImageLink { get; set; }
        public string Phone { get; set; }
    }
}
