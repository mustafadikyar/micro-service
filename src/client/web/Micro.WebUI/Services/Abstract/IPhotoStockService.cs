using Micro.WebUI.Models.Photostock;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Micro.WebUI.Services.Abstract
{
    public interface IPhotoStockService
    {
        Task<PhotoViewModel> UploadPhoto(IFormFile photo);
        Task<bool> DeletePhoto(string photoUrl);
    }
}
