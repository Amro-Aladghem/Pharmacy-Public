using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs.PersonDTOs;
using DTOs.PharamacyDTOs;

namespace DTOs.AdminDTOs
{
    public class AdminDTO
    {
        public int AdminId { get; set; }
        public PersonDTO Person { get; set; }
        public PharmacyDTO Pharmacy { get; set; }
        public bool IsOnline { get; set; }
    }
}
