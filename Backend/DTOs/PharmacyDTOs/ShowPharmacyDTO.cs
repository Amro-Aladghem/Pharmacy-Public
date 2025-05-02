using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.PharmacyDTOs
{
    public class ShowPharmacyDTO
    {
        public int PharmacyId { get; set; }
        public string Name { get; set; }
        public string? ArabicName { get; set; }
        public string ? About { get; set; }
        public bool IsHasDelivery { get; set; }
        public decimal? VedioCallPrice { get; set; }
        public string CountryName { get; set; }
        public string GovernateName { get; set; }
        public string ImageURL { get; set; }
        public bool IsHasMeetingService { get; set; } //fix it to don't allow null
        public string RegionName { get; set; }

    }
}
