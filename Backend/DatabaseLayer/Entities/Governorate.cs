using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer.Entities
{
    public class Governorate
    {
        [Key]
        public int GovernorId { get; set; }

        [MaxLength(40)]
        public string Name { get; set; }

        [MaxLength(40)]
        public string NameArabic { get; set; }


        [ForeignKey("country")]
        public int CountryId { get; set; }
        public Country country { get; set; }

        public ICollection<Pharmacy> Pharmacies { get; set; }
    }
}
