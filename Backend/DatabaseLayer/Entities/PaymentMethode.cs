using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer.Entities
{
    public class PaymentMethode
    {
        [Key]
        public int MethodeId { get; set; }

        [Required]
        [MaxLength(30)]
        public string MethodeName { get;set; }

        public ICollection<OrderPayment> OrderPayments { get; set; }
    }
}
