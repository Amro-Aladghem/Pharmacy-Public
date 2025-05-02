using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer.Entities
{
    public class SystemSetting
    {
        [Key]
        public int ServiceId { get;set; }

        [Required]
        [MaxLength(150)]
        public string ServiceName { get; set; } 

        [Required]
        public decimal Fees { get; set; }
    }
}
