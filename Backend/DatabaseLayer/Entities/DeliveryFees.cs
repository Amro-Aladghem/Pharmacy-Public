using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer.Entities
{
    public class DeliveryFees
    {
        public int Id { get; set; }
        public decimal MinDistanceKm { get; set; } 
        public decimal MaxDistanceKm { get; set; } 

        [Column("Fees", TypeName = "decimal(18,2)")]
        public decimal Fees { get; set; }


        public ICollection<PharmacyDeliveryLocation> PharmacyDeliveryLocations { get; set; }    
    }
}
