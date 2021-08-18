using Micro.Catalog.DTOs;
using Micro.Catalog.Services;
using Micro.Shared.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Micro.Catalog.Controllers
{
    [Route("api/categories")]
    public class CategoryController : BaseController
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService) => _categoryService = categoryService;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _categoryService.GetAllAsync();
            return CreateActionResultInstance(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var response = await _categoryService.GetByIdAsync(id);
            return CreateActionResultInstance(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CategoryCreateDTO model)
        {
            var response = await _categoryService.CreateAsync(model);
            return CreateActionResultInstance(response);
        }
    }
}
