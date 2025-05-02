using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer.Entities
{
    public class TempMeetingRequest
    {
        [Key]
        public int TempId { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        [ForeignKey("Pharmacy")]
        public int PharmacyId { get; set; }
        public Pharmacy Pharmacy { get; set; }
        public DateTime DateAndTime { get; set; }
    }
}
