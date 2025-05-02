using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.PharamacyDTOs
{
    public class PhDeliveryLocationsDTO
    {
        public int PharmacyId { get; set; }
        public bool IsHasDelivery { get; set; }
        public List<DeliveryFeesDTO>? DeliveryLocations { get; set; }
    }
}
