using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer.Entities
{
    public class AdminLog
    {
        public int Id { get; set; }

        [ForeignKey("Admin")]
        public int AdminId { get; set; }
        public Admin Admin { get; set; }

        [Required]
        public DateTime DateOfLoggedIn { get; set; }

        public bool IsLogout { get; set; } = false;

        public DateTime? DateOfLoggout { get; set; }

    }
}
