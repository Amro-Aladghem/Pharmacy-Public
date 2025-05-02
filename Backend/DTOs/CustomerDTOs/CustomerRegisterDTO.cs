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
    public class CustomerRegisterDTO
    {
        public PersonRegisterDTO PersonRegister { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}
