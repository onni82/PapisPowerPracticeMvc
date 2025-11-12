using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PapisPowerPracticeMvc.Data.Services;
using PapisPowerPracticeMvc.Data.Services.IService;
using PapisPowerPracticeMvc.Models;
using PapisPowerPracticeMvc.ViewModels;
using System.Net.Http;
using System.Threading.Tasks;

namespace PapisPowerPracticeMvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class ExerciseController : Controller
    {
        private readonly IExerciseService _exerciseService;
        private readonly IMuscleGroupService _muscleGroupService;

        public ExerciseController(IExerciseService exerciseService, IMuscleGroupService muscleGroupService)
        {
            _exerciseService = exerciseService;
            _muscleGroupService = muscleGroupService;
        }
        //public async Task<IActionResult> Index()
        //{
        //    return View();
        //}
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var exercises = (await _exerciseService.GetAllAsync()).ToList();
            var muscleGroups = (await _muscleGroupService.GetAllAsync()).ToList();

            var model = new ExercisePageViewModel
            {
                Exercises = exercises,
                NewExercise = new CreateExerciseViewModel
                {
                    AvailableMuscleGroups = muscleGroups
                }
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ExercisePageViewModel pageModel)
        {
            var model = pageModel.NewExercise;
            
            bool success = await _exerciseService.AddExercise(model);

            if (success)
            {
                TempData["message"] = "Övning skapad";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Kunde inte skapa övning");
            return View("Index", pageModel);
        }
    }
}
