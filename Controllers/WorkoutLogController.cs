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

        private async Task<List<Exercise>> GetExercisesAsync()
        {
            var response = await _httpClient.GetAsync("https://localhost:7202/api/exercises");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Exercise>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<Exercise>();
            }
            return new List<Exercise>();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
