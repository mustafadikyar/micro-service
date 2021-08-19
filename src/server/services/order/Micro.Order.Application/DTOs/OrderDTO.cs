using System;
using System.Collections.Generic;

namespace Micro.Services.Order.Application.DTOs
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string BuyerId { get; set; }

        public AddressDTO Address { get; set; }
        public List<OrderItemDTO> Items { get; set; }
    }
}
