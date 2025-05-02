using DTOs.OrderDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ResultDTOs
{
    public class OrderListResultDTO
    {
        public List<OrdersListDTO>? Orders { get; set; }
        public int CurrentPage { get; set; }
        public string? PrevPage { get;set; }
        public string? NextPage { get; set; }    
        public int TotalPages { get; set; }
        public int RowsCount { get; set; }   

    }
}
