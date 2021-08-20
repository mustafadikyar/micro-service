using System;
using System.Threading.Tasks;

namespace Micro.WebUI.Services.Abstract
{
    public interface IClientCredentialTokenService
    {
        Task<string> GetToken();
    }
}
