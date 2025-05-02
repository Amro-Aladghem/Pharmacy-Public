using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.DefaultValuesDTOs
{
    public class DeliveryTypeDTO
    {
        public int Id { get; set; }
        public decimal MinDistanceKm { get; set; }
        public decimal MaxDistanceKm { get; set; }
        public decimal Fees { get; set; }
    }
}
