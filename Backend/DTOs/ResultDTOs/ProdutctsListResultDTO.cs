using DTOs.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ResultDTOs
{
    public class ProdutctsListResultDTO
    {
        public List<ListingProductDTO> Products { get; set; }
        public string? NextPage { get; set; }
        public int Total { get; set; }
        public int LastPhProductId { get; set; }
    }
}
