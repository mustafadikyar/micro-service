using IdentityModel.Client;
using Micro.Shared.DTOs;
using Micro.WebUI.Models;
using System.Threading.Tasks;

namespace Micro.WebUI.Services.Abstract
{
    public interface IIdentityService
    {
        Task<Response<bool>> SignIn(SigninInput model);
        Task<TokenResponse> GetAccessTokenByRefreshToken();
        Task RevokeRefreshToken();
    }
}
