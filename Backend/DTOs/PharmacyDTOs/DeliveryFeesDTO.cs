using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.PharamacyDTOs
{
    public class DeliveryFeesDTO
    {
        public int id { get; set; }
        public decimal MinDistanceKm { get; set; }
        public decimal MaxDistanceKm { get; set; }
        public decimal Fees { get; set; }
        public bool IsDeliverdToHere { get; set; }
    }
}
