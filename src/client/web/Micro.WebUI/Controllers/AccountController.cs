using Micro.WebUI.Models;
using Micro.WebUI.Services.Abstract;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Micro.WebUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IIdentityService _identityService;
        public AccountController(IIdentityService identityService) => _identityService = identityService;

        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SigninInput model)
        {
            if (!ModelState.IsValid) return View();

            var response = await _identityService.SignIn(model);
            if (!response.IsSuccess)
            {
                response.Errors.ForEach(error => { ModelState.AddModelError(String.Empty, error); });
                return View();
            }
            return RedirectToAction(nameof(Index), "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme); //cookie temizler.
            await _identityService.RevokeRefreshToken(); //refresh token siler.
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
