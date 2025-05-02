using DTOs.PharmacyDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ResultDTOs
{
    public  class PharmacyMeetingListResultDTO
    {
        public List<PharmacyMeetingListDTO> pharmacies { get; set; }
        public string NextPage { get; set; }
        public int RowsCount { get; set; } = 0;
        public int LastPharmacyId { get; set; }
    }
}
