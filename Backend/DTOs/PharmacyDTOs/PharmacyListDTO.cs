using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.PharmacyDTOs
{
    public class PharmacyListDTO
    {
        public int PharmacyId { get; set; }
        public string Name { get; set; }
        public string? ArabicName { get; set; }
        public string? ImageURL { get; set; }
        public string GovernateName { get; set; }
        public string RegionName { get; set; }
    }
}
