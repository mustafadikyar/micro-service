using Micro.Discount.Services;
using Micro.Shared.Controllers;
using Micro.Shared.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Micro.Discount.Controllers
{
    [Route("api/discounts")]    
    public class DiscountController : BaseController
    {
        private readonly IDiscountService _discountService;
        private readonly ISharedIdentityService _sharedIdentityService;

        public DiscountController(IDiscountService discountService, ISharedIdentityService sharedIdentityService)
        {
            _discountService = discountService;
            _sharedIdentityService = sharedIdentityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return CreateActionResultInstance(await _discountService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var discount = await _discountService.GetById(id);

            return CreateActionResultInstance(discount);
        }

        [HttpGet("{code}")]
        public async Task<IActionResult> GetByCode(string code)

        {
            var userId = _sharedIdentityService.GetUserId;

            var discount = await _discountService.GetByCodeAndUserId(code, userId);

            return CreateActionResultInstance(discount);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Models.Discount model)
        {
            return CreateActionResultInstance(await _discountService.Save(model));
        }

        [HttpPut]
        public async Task<IActionResult> Put(Models.Discount model)
        {
            return CreateActionResultInstance(await _discountService.Update(model));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return CreateActionResultInstance(await _discountService.Delete(id));
        }
    }
}
