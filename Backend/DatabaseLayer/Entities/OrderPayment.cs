using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer.Entities
{
    public class OrderPayment
    {
        [Key]
        public int PaymentId { get; set; }

        [ForeignKey("Order")]
        public int OrderId { get; set; }    
        public Order Order { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }  

        [Required]
        public DateTime DateTimeOfPaid { get; set; }

        [ForeignKey("PaymentMethode")]
        public int PaymentMethodeId { get; set; }
        public PaymentMethode PaymentMethode { get; set; }
        public bool IsRefund { get; set; } = false;

        [Column("PaidAmount", TypeName = "decimal(18,3)")]
        public decimal PaidAmount { get; set; }
    }
}
