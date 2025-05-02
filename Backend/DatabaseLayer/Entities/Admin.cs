using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer.Entities
{
    public class Admin 
    {
        [Key]
        public int AdminId { get; set; }

        [ForeignKey("Person")]
        public int PersonId { get; set; }
        public Person Person { get; set; }

        [ForeignKey("pharmacy")]
        public int PharamcyId { get; set; }
        public Pharmacy pharmacy { get; set; }  
        public bool IsOnline { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime LastLoggedInTime {  get; set; }
        public bool IsActive { get; set; } = true;

        public ICollection<Message> Messages { get; set; }
        public ICollection<AdminLog> AdminLogs { get; set; }
        public ICollection<Meeting> Meetings { get; set; }

    }
}
