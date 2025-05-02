using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer.Entities
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get;set; }
        public Customer Customer { get; set; }

        [ForeignKey("Pharmacy")]
        public int ? PharmacyId { get; set; }
        public Pharmacy Pharmacy { get; set; }
        public DateTime? LastUpdatedTime { get; set; }

        public ICollection<CartItem> cartItems { get; set; }

    }
}
