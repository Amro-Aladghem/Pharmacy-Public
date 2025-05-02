using DTOs.PharamacyDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ProductDTOs
{
    public class ShowProductDTO
    {
        public int PhProductId { get; set; }
        public string Name { get; set; }
        public string ImageURL { get; set; }
        public DateTime ProducedDate { get; set; }  
        public DateTime EndDate { get; set; }
        public decimal Price { get; set; }
        public int MedicalQuantity { get; set; }
        public int Stoke { get; set; }
        public MedicalTypeDTO MedicalType { get; set; }
        public PharmacySummaryDTO Pharmacy { get; set; }
    }
}
