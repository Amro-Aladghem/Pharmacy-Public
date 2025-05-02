using DTOs.DefaultValuesDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.PharamacyDTOs
{
    public class PharmacySummaryDTO
    {
        public int PharmacyId { get; set; }
        public string Name { get; set; }
        public string? ImageURL { get;set; }
        public bool IsHasDelivery { get; set; }
    }
}
