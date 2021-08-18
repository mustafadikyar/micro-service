﻿using Micro.IdentityServer.DTOs;
using Micro.IdentityServer.Models;
using Micro.Shared.Controllers;
using Micro.Shared.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Micro.IdentityServer.Controllers
{
    [Route("api/users")]
    public class UserController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public UserController(UserManager<ApplicationUser> userManager) => _userManager = userManager;

        [HttpPost]
        public async Task<IActionResult> SignUp(SignupDTO model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Email,
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return BadRequest(Response<NoContent>.Error(result.Errors.Select(error => error.Description).ToList(), 400));

            return NoContent();
        }
    }
}
