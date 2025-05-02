using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.AdminDTOs
{
    public class AdminPreifInfoDTO
    {
        public int AdminId { get; set; }    
        public string AdminName { get; set; }   
        public bool IsOnline { get; set; }
        public bool IsActive { get; set; }
        public DateTime LastLoggedInTime { get; set; }
    }
}
