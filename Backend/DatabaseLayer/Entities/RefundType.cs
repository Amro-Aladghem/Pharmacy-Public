using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer.Entities
{
    public class RefundType
    {
        [Key]
        public int TypeId { get; set; }

        [MaxLength(20)]
        public string Name { get; set; }    

    }
}
