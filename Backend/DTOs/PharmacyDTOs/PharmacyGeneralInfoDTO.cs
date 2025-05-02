using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.PharmacyDTOs
{
    public class PharmacyGeneralInfoDTO
    {
        public int PharamcyId { get; set; }
        public int CountryId { get; set; }
        public int GovernateId { get; set; }
        public string Phone { get; set; }
        public string ? PhEmail { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string? StreetName { get; set; }
    }
}
