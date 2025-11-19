using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PapisPowerPracticeMvc.Data.Services;
using PapisPowerPracticeMvc.Data.Services.IService;
using PapisPowerPracticeMvc.Models;
using PapisPowerPracticeMvc.ViewModels;
using System.Collections.Concurrent;
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

        public async Task<IActionResult> Edit(int id)
        {
            var exercise = await _exerciseService.GetByIdAsync(id);

            if(exercise == null)
            {
                return NotFound();
            }

            return View(exercise);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ExerciseViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _exerciseService.UpdateExercise(id, model);

            if (!result)
            {
                ModelState.AddModelError("", "Kunde inte uppdatera övningen. Försök igen.");
                return View(model);
            }

            TempData["SuccessMessage"] = "Övningen har uppdaterats!";
            return RedirectToAction(nameof(Index));
        }
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

            {
                TempData["error"] = "Övningen existerar redan";
                return RedirectToAction("Index", pageModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _exerciseService.Delete(id);

            if (success)
            {
                TempData["Message"] = "Övningen har tagits bort";

            }
            else
            {
                TempData["error"] = "Kunde inte ta bort övningen";
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
