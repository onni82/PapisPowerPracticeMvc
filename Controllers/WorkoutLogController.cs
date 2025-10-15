using Microsoft.AspNetCore.Mvc;
using PapisPowerPracticeMvc.Models;
using System.Diagnostics;
using System.Text.Json;

namespace PapisPowerPracticeMvc.Controllers
{
    public class WorkoutLogController : Controller
    {
        private readonly ILogger<WorkoutLogController> _logger;
        private readonly HttpClient _httpClient;

        public WorkoutLogController(ILogger<WorkoutLogController> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7202/api/");
        }

        public async Task<IActionResult> WorkoutLog()
        {

            var exercises = await GetExercisesAsync();
            var model = new WorkoutLogViewModel
            {
                AvailableExercises = exercises
            };
            return View(model);
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

            var exercises = await GetExercisesAsync();
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
        public IActionResult EndWorkout()
        {
            var workoutId = HttpContext.Session.GetInt32("CurrentWorkoutId");
            if (workoutId.HasValue)
            {
                HttpContext.Session.SetString("WorkoutEndTime", DateTime.Now.ToString());
                HttpContext.Session.Remove("CurrentWorkoutId");
            }
            return RedirectToAction("WorkoutLog");
        }

        private async Task<List<Exercise>> GetExercisesAsync()
        {
            try
            {
                var response = await _httpClient.GetStringAsync("https://localhost:7202/api/Exercises");
                return JsonSerializer.Deserialize<List<Exercise>>(response, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<Exercise>();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                return new List<Exercise>();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
