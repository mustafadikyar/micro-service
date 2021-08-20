using Micro.WebUI.Models;
using Micro.WebUI.Services.Abstract;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Micro.WebUI.Services
{
    public class UserManager : IUserService
    {
        private readonly HttpClient _client;
        public UserManager(HttpClient client) => _client = client;

        public async Task<UserViewModel> GetUser() => await _client.GetFromJsonAsync<UserViewModel>("/api/users");
    }
}
