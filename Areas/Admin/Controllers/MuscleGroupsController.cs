using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PapisPowerPracticeMvc.Data.Services.IService;
using PapisPowerPracticeMvc.ViewModels;

namespace PapisPowerPracticeMvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class MuscleGroupsController : Controller
    {
        private readonly IMuscleGroupService _muscleGroupService;

        public MuscleGroupsController(IMuscleGroupService muscleGroupService)
        {
            _muscleGroupService = muscleGroupService;
        }

        public async Task<IActionResult> Index()
        {
            var groups = await _muscleGroupService.GetAllAsync();
            return View(groups);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var group = await _muscleGroupService.GetByIdAsync(id);
            
            if (group == null)
            {
                return NotFound();
			}

			return View(group);
		}

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(MuscleGroupViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var success = await _muscleGroupService.CreateAsync(model);
            if (success)
            {
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "Kunde inte skapa muskelgruppen. Försök igen.");
            return View(model);
        }
    }
}
