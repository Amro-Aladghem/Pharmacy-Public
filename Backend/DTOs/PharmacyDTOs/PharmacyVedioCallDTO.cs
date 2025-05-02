using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.PharmacyDTOs
{
    public class PharmacyVedioCallDTO
    {
        public int PharamcyId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public decimal? Price { get; set; } = null;
        public bool IsHasVedioCall { get; set; } = false;
         
    }
}
