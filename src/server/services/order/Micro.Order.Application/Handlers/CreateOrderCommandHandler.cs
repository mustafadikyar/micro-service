using MediatR;
using Micro.Order.Application.Commands;
using Micro.Order.Domain.OrderAggregate;
using Micro.Order.Infrastructure;
using Micro.Services.Order.Application.DTOs;
using Micro.Shared.DTOs;
using System.Threading;
using System.Threading.Tasks;


namespace Micro.Order.Application.Handlers
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Response<OrderCreatedDTO>>
    {
        private readonly OrderDbContext _context;

        public CreateOrderCommandHandler(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<Response<OrderCreatedDTO>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            Address createdAddress = new(
                request.Address.Province, 
                request.Address.District, 
                request.Address.Street, 
                request.Address.ZipCode,
                request.Address.Line
                );

            Domain.OrderAggregate.Order createdOrder = new(request.BuyerId, createdAddress);

            request.OrderItems.ForEach(item =>
            {
                createdOrder.AddOrderItem(item.ProductId, item.ProductName, item.Price, item.PictureUrl);
            });

            await _context.Orders.AddAsync(createdOrder);
            await _context.SaveChangesAsync();

            return Response<OrderCreatedDTO>.Success(new OrderCreatedDTO { OrderId = createdOrder.Id }, 200);
        }
    }
}