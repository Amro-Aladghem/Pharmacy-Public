using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer.Entities
{
    public class Manager
    {
        [Key]
        public int ManagerId { get; set;}

        [ForeignKey("Person")]
        public int PersonId { get; set;}
        public Person Person { get; set;}

        [ForeignKey("Pharmacy")]
        public int? PharmacyId { get; set; }
        public Pharmacy Pharmacy { get; set; }  
        public bool IsActive { get; set; } = true;
        public DateTime? LastLoggedInTime { get; set; }

    }
}
