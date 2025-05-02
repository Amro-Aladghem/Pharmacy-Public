using DTOs.PersonDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.CustomerDTOs
{
    public class ShowCustomerDTO
    {
        public int CustomerId { get; set; }
        public ShowPersonDTO Person { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}
