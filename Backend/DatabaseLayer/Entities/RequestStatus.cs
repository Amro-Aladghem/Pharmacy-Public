using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer.Entities
{
    public class RequestStatus
    {
        [Key]
        public int ReqStatusId { get; set; }

        [Required]
        [MaxLength(50)]
        public string ReqStatusName { get; set; }


        [Required]
        [MaxLength(50)]
        public string ReqStatusArabic { get; set; }

        public ICollection<RequestMeeting> RequestMeetings { get; set; }

    }
}
