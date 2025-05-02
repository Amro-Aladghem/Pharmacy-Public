using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer.Entities
{
    public class Token
    {
        [Key]
        public int TokenId { get; set; }

        [ForeignKey("UserType")]
        public int UserTypeId { get; set; } 
        public UserType UserType { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [MaxLength(350)]
        public string GeneratedToken { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime GeneratedDateTime { get; set; }
        public DateTime ? ExpiredDate { get; set; }


    }
}
