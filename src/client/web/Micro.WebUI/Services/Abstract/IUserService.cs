using Micro.WebUI.Models;
using System.Threading.Tasks;

namespace Micro.WebUI.Services.Abstract
{
    public interface IUserService
    {
        Task<UserViewModel> GetUser();
    }
}
