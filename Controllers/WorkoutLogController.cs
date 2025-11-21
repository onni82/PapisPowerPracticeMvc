using Microsoft.AspNetCore.Mvc;
using PapisPowerPracticeMvc.Data.Services.IService;
using PapisPowerPracticeMvc.Models;
using PapisPowerPracticeMvc.ViewModels;
using System.Diagnostics;

namespace PapisPowerPracticeMvc.Controllers
{
    public class WorkoutLogController : Controller
    {
        private readonly ILogger<WorkoutLogController> _logger;
        private readonly IWorkoutLogServices _workoutLogServices;
        private readonly IExerciseService _exerciseService;
        private readonly IMuscleGroupService _muscleGroupService;

        public WorkoutLogController(ILogger<WorkoutLogController> logger, IWorkoutLogServices workoutLogServices, IExerciseService exerciseService, IMuscleGroupService muscleGroupService)
        {
            _logger = logger;
            _workoutLogServices = workoutLogServices;
            _exerciseService = exerciseService;
            _muscleGroupService = muscleGroupService;
        }

        public async Task<IActionResult> WorkoutLog()
        {
            var muscleGroupViewModels = await _muscleGroupService.GetAllAsync();
            var muscleGroups = muscleGroupViewModels.Select(mg => new MuscleGroup
            {
                Id = mg.Id,
                Name = mg.Name
            }).ToList();
            
            var model = new WorkoutLogViewModel
            {
                AvailableExercises = new List<Exercise>(),
                MuscleGroups = muscleGroups,
                WorkoutLogId = HttpContext.Session.GetInt32("CurrentWorkoutId")
            };
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetExercisesByMuscleGroup(int muscleGroupId)
        {
            // Get the muscle group name first
            var muscleGroups = await _muscleGroupService.GetAllAsync();
            var selectedMuscleGroup = muscleGroups.FirstOrDefault(mg => mg.Id == muscleGroupId);
            
            if (selectedMuscleGroup == null)
                return Json(new List<object>());
            
            var allExercises = await _exerciseService.GetAllAsync();
            var filteredExercises = allExercises.Where(e => e.MuscleGroups != null && e.MuscleGroups.Contains(selectedMuscleGroup.Name));
            return Json(filteredExercises.Select(e => new { id = e.Id, name = e.Name }));
        }

        [HttpPost]
        public async Task<IActionResult> AddExercise(int exerciseId)
        {
            if (HttpContext.Session.GetInt32("CurrentWorkoutId") == null)
            {
                var workoutId = new Random().Next(1000, 9999);
                HttpContext.Session.SetInt32("CurrentWorkoutId", workoutId);
                HttpContext.Session.SetString("WorkoutStartTime", DateTime.Now.ToString());
            }

            var exercises = await _exerciseService.GetAllAsync();
            var exercise = exercises.FirstOrDefault(e => e.Id == exerciseId);
            
            if (exercise != null)
            {
                var entry = new WorkoutExerciseViewModel
                {
                    ExerciseId = exercise.Id,
                    ExerciseName = exercise.Name,
                    Sets = new List<WorkoutSetViewModel> { new WorkoutSetViewModel() }
                };
                return PartialView("_WorkoutEntry", entry);
            }
            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] string workoutData)
        {


            if (string.IsNullOrEmpty(workoutData))
            {
                TempData["Error"] = "No workout data received!";
                return RedirectToAction("WorkoutLog");
            }

            _logger.LogInformation("Processing workout data...");
            _logger.LogInformation(workoutData);

            var exercises = System.Text.Json.JsonSerializer.Deserialize<
                List<CreateWorkoutExerciseDTO>>(workoutData);

            if(exercises == null || exercises.Count == 0)
            {
                TempData["Message"] = "Workout contains no exercises";
                return RedirectToAction("WorkoutLog");
            }   
                var startTimeStr = HttpContext.Session.GetString("WorkoutStartTime");
                var startTime = DateTime.TryParse(startTimeStr, out var parsedStartTime) ? parsedStartTime : DateTime.Now.AddHours(-1);
                
                var createWorkoutLogDTO = new CreateWorkoutLogDTO
                {
                    StartTime = startTime,
                    EndTime = DateTime.Now,
                    Notes = null,
                    Exercises = exercises
                };
                
                _logger.LogInformation($"About to call API with DTO: {System.Text.Json.JsonSerializer.Serialize(createWorkoutLogDTO)}");
                // Get JWT token from cookies
                var jwtToken = HttpContext.Request.Cookies["jwt"] ?? "no-token-found";
                

                
                var result = await _workoutLogServices.SaveWorkoutAsync(createWorkoutLogDTO, jwtToken);
                _logger.LogInformation($"Workout save result: {result}");
                
                HttpContext.Session.Remove("CurrentWorkoutId");
                HttpContext.Session.Remove("WorkoutStartTime");
            
            TempData["Message"] = "Workout saved successfully!";
            return RedirectToAction("WorkoutLog");
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
