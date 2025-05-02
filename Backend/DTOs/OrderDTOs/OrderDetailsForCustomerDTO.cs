using DTOs.PharamacyDTOs;
using DTOs.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.OrderDTOs
{
    public class OrderDetailsForCustomerDTO
    {
        public int Id { get; set; }
        public PharmacyDTO Pharmacy { get; set; }
        public OrderStatusDTO Status { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal SubPrice { get; set; }
        public decimal DeliveryFees { get; set; }
        public decimal ServiceFees { get; set; }
        public int PaymentMethodeId { get; set; }
        public bool IsAbleToCanceled { get; set; }
        public List<OrderItemDTO> Products { get; set; }
        public DateTime DateOfOrder { get; set; }

    }
}
