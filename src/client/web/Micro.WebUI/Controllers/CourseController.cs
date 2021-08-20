using Micro.Shared.Services;
using Micro.WebUI.Models.Catalog;
using Micro.WebUI.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace Micro.WebUI.Controllers
{
    [Authorize]
    public class CourseController : Controller
    {
        private readonly ICatalogService _catalogService;
        private readonly ISharedIdentityService _sharedIdentityService;

        public CourseController(ICatalogService catalogService, ISharedIdentityService sharedIdentityService)
        {
            _catalogService = catalogService;
            _sharedIdentityService = sharedIdentityService;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _catalogService.GetAllCourseByUserIdAsync(_sharedIdentityService.GetUserId);
            return View(response);
        }

        public async Task<IActionResult> Create()
        {
            var categories = await _catalogService.GetAllCategoryAsync();
            ViewBag.categoryList = new SelectList(categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CourseCreateInput CourceCreateInput)
        {
            var categories = await _catalogService.GetAllCategoryAsync();
            ViewBag.categoryList = new SelectList(categories, "Id", "Name");

            if (!ModelState.IsValid)
                return View();

            CourceCreateInput.UserId = _sharedIdentityService.GetUserId;
            await _catalogService.CreateCourseAsync(CourceCreateInput);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(string id)
        {
            var Cource = await _catalogService.GetByCourseId(id);
            var categories = await _catalogService.GetAllCategoryAsync();

            if (Cource == null)
            {
                //mesaj göster
                RedirectToAction(nameof(Index));
            }
            ViewBag.categoryList = new SelectList(categories, "Id", "Name", Cource.Id);
            CourseUpdateInput CourceUpdateInput = new()
            {
                Id = Cource.Id,
                Name = Cource.Name,
                Description = Cource.Description,
                Price = Cource.Price,
                Feature = Cource.Feature,
                CategoryId = Cource.CategoryId,
                UserId = Cource.UserId,
                Image = Cource.Image
            };

            return View(CourceUpdateInput);
        }

        [HttpPost]
        public async Task<IActionResult> Update(CourseUpdateInput CourceUpdateInput)
        {
            var categories = await _catalogService.GetAllCategoryAsync();
            ViewBag.categoryList = new SelectList(categories, "Id", "Name", CourceUpdateInput.Id);

            if (!ModelState.IsValid)
                return View();

            await _catalogService.UpdateCourseAsync(CourceUpdateInput);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string id)
        {
            await _catalogService.DeleteCourseAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}