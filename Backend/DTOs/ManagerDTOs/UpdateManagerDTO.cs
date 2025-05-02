using DTOs.PersonDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ManagerDTOs
{
    public class UpdateManagerDTO
    {
        public int ManagerId { get;set; }
        public UpdatePersonDTO Person { get; set; }
        public int PharmacyId { get; set; }

    }
}
