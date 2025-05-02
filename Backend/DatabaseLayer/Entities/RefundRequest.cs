using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer.Entities
{
    public class RefundRequest
    {
        [Key]
        public int RefundId { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        [Required]
        public DateTime DateAndTimeOfRequest { get; set; }

        [ForeignKey("RefundType")]
        public int  RefundTypeId {get;set;}
        public RefundType RefundType { get; set; }

        [Required]
        public int RefferenceId { get; set; }

        [MaxLength(200,ErrorMessage ="The Max length is 200 char")]
        public string? AdditionalInformation { get; set; }

        [ForeignKey("RefundStatus")]
        public int RefundStatusId { get; set; }
        public RefundStatus RefundStatus { get; set; }
    }
}
