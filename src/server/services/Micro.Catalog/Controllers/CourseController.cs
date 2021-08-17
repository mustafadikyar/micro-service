using Micro.Catalog.DTOs;
using Micro.Catalog.Services;
using Micro.Shared.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Micro.Catalog.Controllers
{
    [Route("api/courses")]
    public class CourseController : BaseController
    {
        private readonly ICourseService _courseService;
        public CourseController(ICourseService courseService) => _courseService = courseService;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _courseService.GetAllAsync();
            return CreateActionResultInstance(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var response = await _courseService.GetByIdAsync(id);
            return CreateActionResultInstance(response);
        }

        [Route("{userId}")]
        public async Task<IActionResult> GetAllByUserId(string userId)
        {
            var response = await _courseService.GetAllByUserIdAsync(userId);
            return CreateActionResultInstance(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CourseCreateDTO model)
        {
            var response = await _courseService.CreateAsync(model);
            return CreateActionResultInstance(response);
        }

        [HttpPut]
        public async Task<IActionResult> Put(CourseUpdateDTO model)
        {
            var response = await _courseService.UpdateAsync(model);
            return CreateActionResultInstance(response);
        }

        [HttpDelete("{courceId}")]
        public async Task<IActionResult> Delete(string courceId)
        {
            var response = await _courseService.DeleteAsync(courceId);
            return CreateActionResultInstance(response);
        }
    }
}
