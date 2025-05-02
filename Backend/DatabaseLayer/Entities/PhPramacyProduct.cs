using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer.Entities
{
    public class PhPramacyProduct
    {
        [Key]
        public int PhProductId { get; set; }

        [ForeignKey("SysProduct")]
        public int SysProductId { get; set; }
        public SysProduct SysProduct { get; set; }

        [ForeignKey("Pharmacy")]
        public int PharamcyId { get; set; }
        public Pharmacy Pharmacy { get; set; }

        [MaxLength(1000, ErrorMessage = "The Max length is 1000 char!")]
        public string ? PhDescription { get; set; }

        [Required]
        [Column("Price", TypeName = "decimal(18,3)")]
        public decimal Price { get; set; }

        [Required]
        public int Stoke { get; set; }

        [Required]
        [DataType(DataType.Date)]   
        public DateTime ProducedDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [ForeignKey("MedicalType")]
        public int MedicalTypeId { get; set; }
        public MedicalType MedicalType { get; set; }

        [ForeignKey("MedicalCategory")]
        public int CategoryId { get; set; }
        public MedicalCategory MedicalCategory { get; set; }

        [Required]
        public int MedicalQuantity { get; set; }

        public ICollection<OrderItem> orderItems { get; set; }

    }
}
