using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ProductDTOs
{
    public class PhProductDTO
    {
        public int PharamcyId { get; set; }
      
        public string? PhDescription { get; set; }

        public decimal Price { get; set; }

        public int Stoke { get; set; }

        public DateTime ProducedDate { get; set; }

        public DateTime EndDate { get; set; }

        public int MedicalTypeId { get; set; }

        public int MedicalQuantity { get; set; }
        public int CategoryId { get; set; }

    }
}
