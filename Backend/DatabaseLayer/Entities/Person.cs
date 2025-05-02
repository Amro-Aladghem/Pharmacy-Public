using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer.Entities
{
    public class Person
    {
        [Key]
        public int PersonId { get; set; }

        
        [MaxLength(70,ErrorMessage ="The Max length is 70 char!")]
        public string? FirstName { get; set; }

        
        [MaxLength(70, ErrorMessage = "The Max length is 70 char!")]
        public string? LastName { get; set; }

        [Required]
        [Length(15,120,ErrorMessage = "The Max length is 120 char!")]
        public string Email { get; set; }

        [Required]
        [MinLength(8,ErrorMessage ="The Min length is 8 char")]
        public string Password { get; set; }
        public string ? ProfileImageURL { get; set; }

        [MaxLength(15)]
        public string? Phone { get; set; }
        public bool IsVerified { get; set; }

    }
}
