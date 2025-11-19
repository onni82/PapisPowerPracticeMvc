using Microsoft.AspNetCore.Mvc;
using PapisPowerPracticeMvc.Data.Services.IService;
using PapisPowerPracticeMvc.Models;

namespace PapisPowerPracticeMvc.Controllers
{
    public class ExercisesController : Controller
    {
        private readonly IExerciseService _exerciseService;
        private readonly ILogger<ExercisesController> _logger;

        public ExercisesController(IExerciseService exerciseService, ILogger<ExercisesController> logger)
        {
            _exerciseService = exerciseService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var exercises = await _exerciseService.GetAllAsync();
            return View(exercises);
        }

        public async Task<IActionResult> Details(int id)
        {
            var exercise = await _exerciseService.GetByIdAsync(id);
            if (exercise == null) return NotFound();

            return View(exercise);
        }
    }
}
