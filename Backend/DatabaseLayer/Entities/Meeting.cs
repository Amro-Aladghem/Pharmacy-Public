using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer.Entities
{
    public class Meeting
    {

        [Key]
        public int MeetingId { get; set; }

        [ForeignKey("UserType")]
        public int UserTypeId { get; set; } 
        public UserType UserType { get; set; }
        public int RefferenceId { get; set; }

        [ForeignKey("RequestMeeting")]
        public int RequestId { get; set; }  
        public RequestMeeting RequestMeeting { get; set; }

        [Required]
        public DateTime StartDateTime { get; set; }

        [Required]
        public DateTime EndDateTime { get; set; }

        [Required]
        public TimeOnly Duration { get; set; }

    }
}
