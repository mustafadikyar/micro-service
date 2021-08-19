using Micro.IdentityServer.DTOs;
using Micro.IdentityServer.Models;
using Micro.Shared.Controllers;
using Micro.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;

namespace Micro.IdentityServer.Controllers
{
    [Authorize(LocalApi.PolicyName)]
    [Route("api/users")]
    public class UserController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public UserController(UserManager<ApplicationUser> userManager) => _userManager = userManager;

        [HttpPost("signup")]
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

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var claim = User.Claims.FirstOrDefault(claim => claim.Type.Equals(JwtRegisteredClaimNames.Sub));
            if (claim == null) return BadRequest();

            var user = await _userManager.FindByIdAsync(claim.Value);
            if (user == null) return BadRequest();

            return Ok(new
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
            });
        }
    }
}