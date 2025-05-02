using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer.Entities
{
    public class Message
    {
        [Key]
        public int MessageId { get; set; }

        [ForeignKey("Admin")]
        public int? AdminId { get; set; }
        public Admin Admin { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }


        [ForeignKey("Pharmacy")]
        public int PharmacyId { get; set; }
        public Pharmacy Pharmacy { get; set; }

        [Required]
        [MaxLength(1000, ErrorMessage = "The Max length of message mustn't be more than 1000 char!")]
        public string message { get; set; }

        [Required]
        public DateOnly DateOfMessage { get; set; }

        [Required]
        public TimeOnly Time { get; set; }

        [ForeignKey("UserType")]
        public int UserTypeId { get; set; }
        public UserType UserType { get; set; }

    }
}
