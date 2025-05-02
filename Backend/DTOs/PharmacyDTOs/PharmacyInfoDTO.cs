using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.PharamacyDTOs
{
    public class PharmacyInfoDTO
    {
        public string Name { get; set; }
        public string? ArabicName { get; set; }
        public string? About { get; set; }
        public int CountryId { get; set;}
        public int GovernateId { get; set; }
        public string Phone { get; set; }
        public string PhEmail { get; set; }
        public decimal Longitude { get; set; }  
        public decimal Latitude { get; set; }
        public string? StreetName { get; set; }
        public decimal? VedioCallPrice { get; set; } = 0;
        public bool IsHasDelivery { get; set; }
        public string? ImageURL { get; set; }
    }
}
