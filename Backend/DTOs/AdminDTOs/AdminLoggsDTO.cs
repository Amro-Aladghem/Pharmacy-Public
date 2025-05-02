using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.AdminDTOs
{
    public class AdminLoggsDTO
    {
        public int AdminId { get; set; }
        public DateTime DateOfLogged { get; set; }
        public bool IsLogut { get; set; }
        public DateTime? DateOfLogOut { get; set; }

    }
}
