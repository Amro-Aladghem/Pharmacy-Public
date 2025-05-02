using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer.Entities
{
    public class Customer 
    {
        [Key]
        public int CutomerId {  get; set; }

        [ForeignKey("Person")]
        public int PersonId { get; set; }
        public Person Person { get; set; }
        public bool IsOnline { get; set; } = true;

        [DataType(DataType.Date)]
        public DateTime DateOfRegister { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime LastLoggedInDateTime { get; set; }

        [Required]
        [Column("Latitude", TypeName = "decimal(10,5)")]
        public decimal Latitude { get; set; }

        [Required]
        [Column("Longitude", TypeName = "decimal(10,5)")]
        public decimal Longitude { get; set; }

        public ICollection<Order> Orders { get; set; }
        public ICollection<Message> Messages  { get; set; }
        public ICollection<CustomerLog> CustomerLogs { get; set; }
        public ICollection<MeetingPayment> MeetingPayments { get; set; }
        public ICollection<RequestMeeting> RequestMeetings { get; set; }
        public ICollection<OrderPayment> OrderPayments { get; set; }
    }
}
