using Microsoft.AspNetCore.Mvc;

namespace Micro.WebUI.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult SignIn()
        {
            return View();
        }
    }
}
