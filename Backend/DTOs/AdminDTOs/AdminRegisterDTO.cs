using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs.PersonDTOs;

namespace DTOs.AdminDTOs
{
    public class AdminRegisterDTO
    {
        public PersonRegisterDTO PersonRegisterDTO { get; set; }
        public int PharmacyId { get; set; }
    }
}
