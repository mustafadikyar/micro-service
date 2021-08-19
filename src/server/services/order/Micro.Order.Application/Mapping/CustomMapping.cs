using AutoMapper;
using Micro.Order.Domain.OrderAggregate;
using Micro.Services.Order.Application.DTOs;

namespace Micro.Services.Order.Application.Mapping
{
    internal class CustomMapping : Profile
    {
        public CustomMapping()
        {
            CreateMap<Micro.Order.Domain.OrderAggregate.Order, OrderDTO>().ReverseMap();
            CreateMap<OrderItem, OrderItemDTO>().ReverseMap();
            CreateMap<Address, AddressDTO>().ReverseMap();
        }
    }
}