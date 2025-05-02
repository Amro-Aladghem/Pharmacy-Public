using DTOs.PersonDTOs;
using DTOs.PharamacyDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ManagerDTOs
{
    public class ManagerDTO
    {
        public int ManagerId { get; set; }
        public PersonDTO Person { get; set; }
        public PharmacyDTO? Pharmacy { get; set; }
        public bool IsHasPharmacy { get; set; }
    }
}
