using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace DTOs.PharmacyDTOs
{
    public class PharmacyMeetingListDTO
    {
        public int PharmacyId { get; set; }
        public string PharmacyName { get; set; }
        public string ImageURL { get; set; }
        public string GovernateName { get; set; }   
        public decimal Price { get; set; }
    }
}
