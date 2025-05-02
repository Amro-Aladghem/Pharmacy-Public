using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.PharamacyDTOs
{
    public class PhCurrencyDTO
    {
        public int CurrencyId { get; set; } 
        public int PharamacyId { get; set; }
        public decimal CurrentCurrency { get; set; }    

    }
}
