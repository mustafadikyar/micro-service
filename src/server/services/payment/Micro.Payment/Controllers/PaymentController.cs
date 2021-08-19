using Micro.Payment.DTOs;
using Micro.Shared.Controllers;
using Micro.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Micro.Payment.Controllers
{
    [Route("api/payments")]
    public class PaymentController : BaseController
    {
        [HttpPost]
        public IActionResult ReceivePayment(/*PaymentDTO paymentDto*/) => //payment operations
            CreateActionResultInstance(Response<NoContent>.Success(200));
    }
}
