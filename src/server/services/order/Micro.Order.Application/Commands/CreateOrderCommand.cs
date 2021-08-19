using MediatR;
using Micro.Services.Order.Application.DTOs;
using Micro.Shared.DTOs;
using System.Collections.Generic;

namespace Micro.Order.Application.Commands
{
    public class CreateOrderCommand : IRequest<Response<OrderCreatedDTO>>
    {
        public string BuyerId { get; set; }
        public List<OrderItemDTO> OrderItems { get; set; }
        public AddressDTO Address { get; set; }
    }
}