using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ProductDTOs
{
    public class MedicalTypeDTO
    {
        public int TypeId { get; set; }

        public string MedicalTypeName { get; set; }
        
        public string MedicalTypeNameArabic { get; set; }
    }
}
