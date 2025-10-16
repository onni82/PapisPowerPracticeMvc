using PapisPowerPracticeMvc.Data.Services.IServices;
using PapisPowerPracticeMvc.Models;
using System.Text.Json;

namespace PapisPowerPracticeMvc.Data.Services
{
    public class WorkoutLogServices : IWorkoutLogServices
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<WorkoutLogServices> _logger;

        public WorkoutLogServices(HttpClient httpClient, ILogger<WorkoutLogServices> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<List<Exercise>> GetExercisesAsync()
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

        public async Task<Exercise?> GetExerciseByIdAsync(int id)
        {
            var exercises = await GetExercisesAsync();
            return exercises.FirstOrDefault(e => e.Id == id);
        }

        public async Task<WorkoutExerciseViewModel?> CreateWorkoutExerciseAsync(int exerciseId)
        {
            var exercise = await GetExerciseByIdAsync(exerciseId);
            if (exercise != null)
            {
                return new WorkoutExerciseViewModel
                {
                    ExerciseId = exercise.Id,
                    ExerciseName = exercise.Name,
                    Sets = new List<WorkoutSetViewModel> { new WorkoutSetViewModel() }
                };
            }
            return null;
        }

        public async Task<bool> SaveWorkoutAsync(WorkoutLog workoutLog)
        {
            try
            {
                var json = JsonSerializer.Serialize(workoutLog);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("https://localhost:7202/api/WorkoutLogs", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error saving workout: {ex.Message}");
                return false;
            }
        }
    }
}
