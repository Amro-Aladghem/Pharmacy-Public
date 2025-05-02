using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.RequestDTOs
{
    public class LocationCheckRequest
    {
        public int CustomerId { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
    }
}
