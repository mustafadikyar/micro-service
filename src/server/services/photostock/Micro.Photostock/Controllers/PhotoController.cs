using Micro.Photostock.DTOs;
using Micro.Shared.Controllers;
using Micro.Shared.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Micro.Photostock.Controllers
{
    [Route("api/photos")]
    public class PhotoController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> Post(IFormFile file, CancellationToken cancellationToken)
        {
            if (file != null && file.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos", file.FileName);

                using FileStream stream = new(path, FileMode.Create);
                await file.CopyToAsync(stream, cancellationToken);
                var returnPath = file.FileName;
                PhotoDTO photo = new() { Url = returnPath };

                return CreateActionResultInstance(Response<PhotoDTO>.Success(photo, 200));
            }
            return CreateActionResultInstance(Response<PhotoDTO>.Error("Photo is empty", 400));
        }

        [HttpDelete]
        public IActionResult Delete(string photoUrl)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos", photoUrl);

            if (!System.IO.File.Exists(path))
                return CreateActionResultInstance(Response<NoContent>.Error("Photo not found", 404));

            System.IO.File.Delete(path);
            return CreateActionResultInstance(Response<NoContent>.Success(204));
        }
    }
}
