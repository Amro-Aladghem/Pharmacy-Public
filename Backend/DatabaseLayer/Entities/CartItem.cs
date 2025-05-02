using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer.Entities
{
    public class CartItem
    {
        [Key]
        public int ItemId { get; set; }

        [ForeignKey("Cart")]
        public int CartId { get; set; }
        public Cart Cart { get; set; }

        [ForeignKey("PhPramacyProduct")]
        public int PhProductId { get; set; }
        public PhPramacyProduct PhPramacyProduct { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }

    }
}
