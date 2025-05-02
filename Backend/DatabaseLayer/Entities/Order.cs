using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer.Entities
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [ForeignKey("Pharmacy")]
        public int PharmacyId { get; set; }
        public Pharmacy Pharmacy { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        [Required]
        public DateTime OrderDateAndTime { get; set; }

        [Column("TotalPrice", TypeName = "decimal(18,3)")]
        public decimal TotalPrice { get; set; }

        [ForeignKey("OrderStatus")]
        public int OrderStatusId { get; set; }
        public OrderStatus OrderStatus { get; set; }

        [Required]
        public decimal DeliveryFees { get; set; }

        [Required]
        public decimal SubPrice { get; set; }

        [Required]
        public decimal ServiceFees { get; set; }

        [ForeignKey("PaymentMethode")]
        public int PaymentMethodeId { get; set; }
        public PaymentMethode PaymentMethode { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
        public ICollection<Invoice> Invoices { get; set; }

    }
}
