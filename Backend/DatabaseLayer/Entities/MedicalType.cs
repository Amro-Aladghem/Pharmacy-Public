using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer.Entities
{
    public  class MedicalType
    {

        [Key]
        public int TypeId { get;set; }

        [Required]
        [MaxLength(20)]
        public string MedicalTypeName { get; set; }

        [Required]
        [MaxLength(20)]
        public string MedicalTypeNameArabic { get; set; }
        public ICollection<PhPramacyProduct> phPramacyProducts  { get; set; }
    }
}
