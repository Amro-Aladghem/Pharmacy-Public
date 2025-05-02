using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer.Entities
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Order")]
        public int OrderId { get; set; }    
        public Order Order { get;set; }

        [ForeignKey("PhPramacyProduct")]
        public int PhProductId { get; set; }
        public PhPramacyProduct PhPramacyProduct { get; set; }

        public int Quantity { get; set; }

        [Column("Price", TypeName = "decimal(18,3)")]
        public decimal Price { get; set; }


    }
}
