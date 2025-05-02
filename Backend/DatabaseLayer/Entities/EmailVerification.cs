using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer.Entities
{
    public class EmailVerification
    {
        [Key]
        public int VerificationId { get; set; }

        [ForeignKey("UserType")]
        public int? UserTypeId { get; set; } 
        public UserType UserType { get; set; }

        [Required]
        public int RefferenceId { get; set; }

        [Required]
        [MaxLength(20)]
        public string Code { get; set; }

        [Required]
        public DateOnly DateOfCreated { get; set; }

        [Required]
        public TimeOnly TimeOfCreated { get; set; }

        [Required]
        public TimeOnly TimeOfExpired { get; set; }

        [Required]
        public int AttemptCount { get; set; } = 0;

        [Required]
        public bool IsActive { get; set; } = true;
    }
}
