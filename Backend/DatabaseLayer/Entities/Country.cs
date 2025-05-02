using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer.Entities
{
    public class Country
    {
        [Key]
        public int CountryId { get; set; }

        [MaxLength(30)]
        public string Name { get; set; }

        public ICollection<Governorate> Governorates { get; set; }
        public ICollection<Pharmacy> Pharmacies { get; set; }
    }
}
