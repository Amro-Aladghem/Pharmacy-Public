using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer.Entities
{
    public class TempOrderRequest
    {
        [Key]
        public int TempId { get; set;}

        [ForeignKey("Cart")]
        public int OrderCartId { get; set;}
        public Cart Cart { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set;}
        public Customer Customer { get; set; }
        public DateTime DateOfSet { get; set; }

    }
}
