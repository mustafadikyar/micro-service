using Micro.WebUI.Models.Catalog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Micro.WebUI.Services.Abstract
{
    public interface ICatalogService
    {
        Task<List<CourseViewModel>> GetAllCourseAsync();

        Task<List<CategoryViewModel>> GetAllCategoryAsync();

        Task<List<CourseViewModel>> GetAllCourseByUserIdAsync(string userId);

        Task<CourseViewModel> GetByCourseId(string courseId);

        Task<bool> CreateCourseAsync(CourseCreateInput model);

        Task<bool> UpdateCourseAsync(CourseUpdateInput model);

        Task<bool> DeleteCourseAsync(string courseId);
    }
}
