using Micro.Shared.DTOs;
using Micro.WebUI.Models.Photostock;
using Micro.WebUI.Services.Abstract;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Micro.WebUI.Services
{
    public class PhotoStockManager : IPhotoStockService
    {
        private readonly HttpClient _httpClient;

        public PhotoStockManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> DeletePhoto(string photoUrl)
        {
            var response = await _httpClient.DeleteAsync($"photos?photoUrl={photoUrl}");
            return response.IsSuccessStatusCode;
        }

        public async Task<PhotoViewModel> UploadPhoto(IFormFile file)
        {
            if (file == null || file.Length <= 0) return null;
            
            var randonFilename = $"{Guid.NewGuid().ToString()}{Path.GetExtension(file.FileName)}";
            
            using MemoryStream ms = new();
            await file.CopyToAsync(ms);
            MultipartFormDataContent multipartContent = new();
            multipartContent.Add(new ByteArrayContent(ms.ToArray()), "file", randonFilename);

            HttpResponseMessage response = await _httpClient.PostAsync("photos", multipartContent);
            if (!response.IsSuccessStatusCode) return null;

            Response<PhotoViewModel> responseSuccess = await response.Content.ReadFromJsonAsync<Response<PhotoViewModel>>();
            return responseSuccess.Data;
        }
    }
}