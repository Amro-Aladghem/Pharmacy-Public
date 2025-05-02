using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer.Entities
{
    public class PharmacyCurrency
    {
        [Key]
        public int CurrencyId { get; set; }

        [ForeignKey("Pharmacy")]
        public int PharmacyId { get; set; } 
        public Pharmacy Pharmacy { get; set; }

        [Column("CurrentCurrency", TypeName = "decimal(18,3)")]
        public decimal CurrentCurrency { get; set; }

    }
}
