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

		// Lista alla muskelgrupper
		public async Task<IActionResult> Index()
        {
            var groups = await _muscleGroupService.GetAllAsync();
            return View(groups);
        }

		// GET: Admin/MuscleGroups/Edit/5
		public async Task<IActionResult> Edit(int id)
        {
            var group = await _muscleGroupService.GetByIdAsync(id);
            
            if (group == null)
            {
                return NotFound();
            }

            return View(group);
        }

		// POST: Admin/MuscleGroups/Edit/5
		[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MuscleGroupViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _muscleGroupService.UpdateAsync(id, model);
            
            if (!result)
            {
                ModelState.AddModelError("", "Kunde inte uppdatera muskelgruppen. Försök igen.");
                return View(model);
            }

            TempData["SuccessMessage"] = "Muskelgruppen har uppdaterats!";
            return RedirectToAction(nameof(Index));
        }

		// GET: Admin/MuscleGroups/Create
		[HttpGet]
        public IActionResult Create()
        {
            return View();
        }

		// POST: Admin/MuscleGroups/Create
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _muscleGroupService.DeleteAsync(id);

            if (success)
            {
                TempData["SuccessMessage"] = "Muskelgruppen har tagits bort.";
            }
            else
            {
                TempData["Error"] = "Kunde inte ta bort muskelgruppen.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
