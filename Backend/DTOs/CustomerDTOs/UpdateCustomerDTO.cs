using DTOs.PersonDTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.CustomerDTOs
{
    public class UpdateCustomerDTO
    {
        public int CustomerId { get; set; }
        public UpdatePersonDTO Person { get; set; }
        
    }
}
