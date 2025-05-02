using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.DefaultValuesDTOs
{
    public class GovernateDTO
    {
        public int GovernotId { get; set; }

        public string Name { get; set; }

        public string NameArabic { get; set; }
    }
}
