using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer.Entities
{
    public class OrderStatus
    {
        [Key]
        public int StatusId { get; set; }

        [MaxLength(50)]
        public string StatusName { get; set; }

        [MaxLength(50)]
        public string StatusNameArabic { get; set; }
        public ICollection<Order> Orders { get; }

    }
}
