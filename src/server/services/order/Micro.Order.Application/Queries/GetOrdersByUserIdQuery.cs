using MediatR;
using Micro.Services.Order.Application.DTOs;
using Micro.Shared.DTOs;
using System.Collections.Generic;

namespace Micro.Order.Application.Queries
{
    public class GetOrdersByUserIdQuery : IRequest<Response<List<OrderDTO>>>
    {
        public string UserId { get; set; }
    }
}