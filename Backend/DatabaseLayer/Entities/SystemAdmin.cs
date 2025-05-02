using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer.Entities
{
    public class SystemAdmin
    {
        [Key]
        public int AdminId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [MaxLength(100)]
        public string Password { get; set; }

        [Required]
        [MaxLength(10)]
        public string PINcode { get; set; }

        [Required]
        public DateTime LastLoggedIn { get; set; }
    }

}
