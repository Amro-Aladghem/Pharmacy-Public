using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer.Entities
{
    public class Region
    {
        [Key]
        public int RegionId { get; set; }

        [ForeignKey("Governorate")]
        public int GovernorateId { get; set;}
        public Governorate Governorate { get; set;}
        public string RegionName { get; set; }
    }
}
