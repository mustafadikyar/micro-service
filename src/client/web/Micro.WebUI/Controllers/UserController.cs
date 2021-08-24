using Micro.WebUI.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Micro.WebUI.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService) => _userService = userService;

        public async Task<IActionResult> Index()
        {
            //IdentityServer/Accounts/Get
            var user = await _userService.GetUser();
            return View(user);
        }
    }
}
