using MediatR;
using Micro.Order.Application.Commands;
using Micro.Order.Application.Queries;
using Micro.Shared.Controllers;
using Micro.Shared.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Micro.Order.Controllers
{
    [Route("api/[controller]")]
    public class OrderController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly ISharedIdentityService _sharedIdentityService;

        public OrderController(IMediator mediator, ISharedIdentityService sharedIdentityService)
        {
            _mediator = mediator;
            _sharedIdentityService = sharedIdentityService;
        }

        [HttpGet]//query
        public async Task<IActionResult> Get()
        {
            var response = await _mediator.Send(new GetOrdersByUserIdQuery { UserId = _sharedIdentityService.GetUserId });
            return CreateActionResultInstance(response);
        }

        [HttpPost]//command
        public async Task<IActionResult> Post(CreateOrderCommand model)
        {
            var response = await _mediator.Send(model);
            return CreateActionResultInstance(response);
        }
    }
}
