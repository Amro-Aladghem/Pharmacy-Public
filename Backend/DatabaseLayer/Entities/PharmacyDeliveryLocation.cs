using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer.Entities
{
    public class PharmacyDeliveryLocation
    {

        [Key]
        public int Id { get; set; }


        [ForeignKey("Pharmacy")]
        public int PharmacyId { get; set; }
        public Pharmacy Pharmacy { get; set; }

        [ForeignKey("DeliveryFees")]
        public int DeliveryId { get; set; }
        public DeliveryFees DeliveryFees { get; set; }

        [Required]
        public bool IsDeliveriedToHere { get; set; }

    }
}
