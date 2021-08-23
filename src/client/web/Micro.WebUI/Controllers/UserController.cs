using Micro.WebUI.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Micro.WebUI.Controllers
{

    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService) => _userService = userService;

        public async Task<IActionResult> Index()
        {
            //IdentityServer/Accounts/Get
            return View(await _userService.GetUser());
        }
    }
}
