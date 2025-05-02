using DTOs.PersonDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.AdminDTOs
{
    public class UpdateAdminDTO
    {
        public int AdminId { get; set; }
        public UpdatePersonDTO Person { get; set; }
        public string? PharmacyName { get; set; }
        public string? PhImageURL { get; set; }
    }
}
