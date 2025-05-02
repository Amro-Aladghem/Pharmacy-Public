using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer.Entities
{
    public  class CustomerPharmacyDistance
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Pharmacy")]
        public int PharmacyId { get; set; }
        public Pharmacy Pharmacy { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        [Required]
        public decimal distance { get; set; }

        public decimal Fees { get; set; }
    }
}
