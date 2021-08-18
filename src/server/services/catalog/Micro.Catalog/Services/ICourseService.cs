using Micro.Catalog.DTOs;
using Micro.Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Micro.Catalog.Services
{
    public interface ICourseService
    {
        Task<Response<List<CourseDTO>>> GetAllAsync();
        Task<Response<CourseDTO>> GetByIdAsync(string id);
        Task<Response<List<CourseDTO>>> GetAllByUserIdAsync(string userId);
        Task<Response<CourseDTO>> CreateAsync(CourseCreateDTO model);
        Task<Response<NoContent>> UpdateAsync(CourseUpdateDTO model);
        Task<Response<NoContent>> DeleteAsync(string id);
    }
}
