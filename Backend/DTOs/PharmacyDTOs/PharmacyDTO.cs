using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.PharamacyDTOs
{
    public class PharmacyDTO
    {
        public int PharmacyId { get; set; }
        public string Name { get; set; }
        public string ? ArabicName { get; set; }
        public string ? About { get; set; }
        public string? ImageURL { get; set; }
        public string? Phone { get; set; }
    }
}
