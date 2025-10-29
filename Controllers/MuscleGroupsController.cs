using Microsoft.AspNetCore.Mvc;
using PapisPowerPracticeMvc.Data.Services.IService;
using PapisPowerPracticeMvc.ViewModels;

namespace PapisPowerPracticeMvc.Controllers
{
    public class MuscleGroupsController : Controller
    {
        private readonly IMuscleGroupService _muscleGroupService;
        private readonly IExerciseService _exerciseService;

        public MuscleGroupsController(IMuscleGroupService muscleGroupService, IExerciseService exerciseService)
        {
            _muscleGroupService = muscleGroupService;
            _exerciseService = exerciseService;
        }

        public async Task<IActionResult> Index()
        {
            var groups = await _muscleGroupService.GetAllAsync();
            return View(groups);
        }

        public async Task<IActionResult> Details(int id)
        {
            var group = await _muscleGroupService.GetByIdAsync(id);
            if (group == null) return NotFound();

            // Hämta övningar och filtrera övningar som matchar gruppen
            var exercises = await _exerciseService.GetAllAsync();
            var relatedExercises = exercises
                .Where(e => e.MuscleGroups != null &&
                        e.MuscleGroups.Any(m => string.Equals(m, group.Name, StringComparison.OrdinalIgnoreCase)))
                .ToList();

            ViewBag.MuscleGroup = group;
            return View(relatedExercises);
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
