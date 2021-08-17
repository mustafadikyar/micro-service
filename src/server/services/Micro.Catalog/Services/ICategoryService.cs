using Micro.Catalog.DTOs;
using Micro.Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Micro.Catalog.Services
{
    public interface ICategoryService
    {
        Task<Response<List<CategoryDTO>>> GetAllAsync();
        Task<Response<CategoryDTO>> CreateAsync(CategoryCreateDTO model);
        Task<Response<CategoryDTO>> GetByIdAsync(string id);
    }
}
