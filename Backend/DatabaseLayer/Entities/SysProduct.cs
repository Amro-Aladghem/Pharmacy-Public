using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer.Entities
{
    public class SysProduct
    {
        [Key]
        public int SysProductId { get; set; }

        [Required]
        [MaxLength(500,ErrorMessage ="The max length is 500 char")]
        public string ProductName { get;set; }

        [Required]
        [MaxLength(1000,ErrorMessage ="The Max length is 1000 char!")]
        public string Description { get; set; }

        [Required]
        [MaxLength(1000)]
        public string ImageURL { get; set; }

        public ICollection<PhPramacyProduct> phPramacyProducts { get; set; }

    }
}
