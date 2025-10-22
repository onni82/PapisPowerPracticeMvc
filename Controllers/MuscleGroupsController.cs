using Microsoft.AspNetCore.Mvc;
using PapisPowerPracticeMvc.Data.Services.IService;

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
            var muscleGroups = await _muscleGroupService.GetAllAsync();
            return View(muscleGroups);
        }

        public async Task<IActionResult> Details(int id)
        {
            var group = await _muscleGroupService.GetByIdAsync(id);
            if (group == null) return NotFound();

            // Hämta övningar och filtrera övningar som matchar gruppen
            var allExercises = await _exerciseService.GetAllAsync();
            var filteredExercises = allExercises
                .Where(e => e.MuscleGroups != null && e.MuscleGroups.Contains(group.Name, StringComparer.OrdinalIgnoreCase))
                .ToList();

            ViewBag.MuscleGroup = group;
            return View(filteredExercises);
        }
    }
}
