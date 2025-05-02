using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs.PersonDTOs;

namespace DTOs.CustomerDTOs
{
    public class CustomerDTO
    {
        public int CustomerId { get; set; }
        public PersonDTO Person { get; set; }
        public decimal Latitude { get; set; } = 0;
        public decimal Longitude { get; set; } = 0;

    }
}
