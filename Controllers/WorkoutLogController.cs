using Microsoft.AspNetCore.Mvc;
using PapisPowerPracticeMvc.Data.Services.IService;
using PapisPowerPracticeMvc.Models;
using System.Diagnostics;

namespace PapisPowerPracticeMvc.Controllers
{
    public class WorkoutLogController : Controller
    {
        private readonly ILogger<WorkoutLogController> _logger;
        private readonly IWorkoutLogServices _workoutLogServices;

        public WorkoutLogController(ILogger<WorkoutLogController> logger, IWorkoutLogServices workoutLogServices)
        {
            _logger = logger;
            _workoutLogServices = workoutLogServices;
        }

        public async Task<IActionResult> WorkoutLog()
        {
            var exercises = await _workoutLogServices.GetExercisesAsync();
            var model = new WorkoutLogViewModel
            {
                AvailableExercises = exercises,
                WorkoutLogId = HttpContext.Session.GetInt32("CurrentWorkoutId")
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

            var entry = await _workoutLogServices.CreateWorkoutExerciseAsync(exerciseId);
            if (entry != null)
            {
                return PartialView("_WorkoutEntry", entry);
            }
            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] string workoutData)
        {
            _logger.LogInformation($"EndWorkout called with data: {workoutData}");
            
            
            if (!string.IsNullOrEmpty(workoutData))
            {
                var startTimeStr = HttpContext.Session.GetString("WorkoutStartTime");
                var startTime = DateTime.TryParse(startTimeStr, out var parsedStartTime) ? parsedStartTime : DateTime.Now.AddHours(-1);
                
                var workoutLog = new WorkoutLog
                {
                    StartTime = startTime,
                    EndTime = DateTime.Now,
                    Notes = workoutData,
                    UserId = User.Identity?.Name ?? "anonymous"
                };
                
                var result = await _workoutLogServices.SaveWorkoutAsync(workoutLog);
                _logger.LogInformation($"Workout save result: {result}");
                
                HttpContext.Session.Remove("CurrentWorkoutId");
                HttpContext.Session.Remove("WorkoutStartTime");
            }
            return RedirectToAction("WorkoutLog");
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
