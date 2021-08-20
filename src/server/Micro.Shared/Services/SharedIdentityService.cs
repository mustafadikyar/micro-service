using Microsoft.AspNetCore.Http;

namespace Micro.Shared.Services
{
    public class SharedIdentityManager : ISharedIdentityService
    {
        private IHttpContextAccessor _httpContextAccessor;
        public SharedIdentityManager(IHttpContextAccessor httpContextAccessor) => _httpContextAccessor = httpContextAccessor;

        public string GetUserId => _httpContextAccessor.HttpContext.User.FindFirst("sub").Value;
    }
}
