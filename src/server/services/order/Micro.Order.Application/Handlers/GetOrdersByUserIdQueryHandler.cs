using MediatR;
using Micro.Order.Application.Queries;
using Micro.Order.Infrastructure;
using Micro.Services.Order.Application.DTOs;
using Micro.Services.Order.Application.Mapping;
using Micro.Shared.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Micro.Order.Application.Handlers
{
    internal class GetOrdersByUserIdQueryHandler : IRequestHandler<GetOrdersByUserIdQuery, Response<List<OrderDTO>>>
    {
        private readonly OrderDbContext _context;

        public GetOrdersByUserIdQueryHandler(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<Response<List<OrderDTO>>> Handle(GetOrdersByUserIdQuery request, CancellationToken cancellationToken)
        {
            var orders = await _context.Orders.Include(x => x.OrderItems).Where(x => x.BuyerId == request.UserId).ToListAsync();

            if (!orders.Any())
            {
                return Response<List<OrderDTO>>.Success(new List<OrderDTO>(), 200);
            }

            var ordersDto = ObjectMapper.Mapper.Map<List<OrderDTO>>(orders);

            return Response<List<OrderDTO>>.Success(ordersDto, 200);
        }
    }
}