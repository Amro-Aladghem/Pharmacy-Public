using DTOs.PersonDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.AdminDTOs
{
    public class ShowAdminDTO
    {
        public int AdminId { get; set; }
        public ShowPersonDTO Person { get; set; }
        public string PharmacyName { get; set; }
        public string PharmacyImage { get; set; }

    }
}
