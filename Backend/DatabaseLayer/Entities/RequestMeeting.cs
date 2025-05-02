using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer.Entities
{
    public class RequestMeeting
    {
        [Key]
        public int RequestId { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }


        [ForeignKey("Pharmacy")]  
        public int PharmacyId { get; set; }
        public Pharmacy Pharmacy { get; set; }

        [Required]
        public DateTime RequestDateTime { get; set; }=DateTime.Now;

        [ForeignKey("RequestStatus")]
        public int RequestStatusId { get; set; }
        public RequestStatus RequestStatus { get; set; }    

        [MaxLength(300)]
        public string ? MeetingURL { get; set; }

        public ICollection<Meeting> Meetings { get; set; }

    }
}
