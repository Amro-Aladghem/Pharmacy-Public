using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer.Entities
{
    public class MeetingPayment
    {
        [Key]
        public int PaymentId { get; set; }

        [ForeignKey("RequestMeeting")]
        public int RequestId { get; set; }
        public RequestMeeting RequestMeeting { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        [Required]
        public DateTime DateTimeOfPaid { get; set; }

        public bool IsRefund { get; set; } = false;

        [Column("PaidAmount", TypeName = "decimal(18,3)")]
        public decimal PaidAmount { get; set; }

    }
}
