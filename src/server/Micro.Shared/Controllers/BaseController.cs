using Micro.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Micro.Shared.Controllers
{
    [Route("api/courses")]
    [ApiController]
    public class BaseController
    {
        public IActionResult CreateActionResultInstance<T>(Response<T> response) => 
            new ObjectResult(response) { StatusCode = response.Status };
    }
}
