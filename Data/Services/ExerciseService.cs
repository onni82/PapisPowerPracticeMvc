using PapisPowerPracticeMvc.Data.Services.IService;
using PapisPowerPracticeMvc.ViewModels;
using PapisPowerPracticeMvc.Models;
using System.Text.Json;

namespace PapisPowerPracticeMvc.Data.Services
{
    public class ExerciseService : IExerciseService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<ExerciseService> _logger;

        public ExerciseService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, ILogger<ExerciseService> logger)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<IEnumerable<ExerciseViewModel>> GetAllAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("exercises");
                
                if (response.IsSuccessStatusCode)
                {
                    var exercises = await response.Content.ReadFromJsonAsync<IEnumerable<ExerciseViewModel>>();
                    return exercises ?? new List<ExerciseViewModel>();
                }
                else
                {
                    _logger.LogError("Failed to get exercises. Status: {StatusCode}", response.StatusCode);
                    return new List<ExerciseViewModel>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching exercises");
                return new List<ExerciseViewModel>();
            }
		}

        public async Task<ExerciseViewModel?> GetByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"exercises/{id}");
                
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ExerciseViewModel>();
                }
                
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching exercise with ID: {Id}", id);
                return null;
            }
        }

        public async Task<List<Exercise>> GetExercisesForWorkoutAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("exercises");
                
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Failed to get exercises. Status: {StatusCode}", response.StatusCode);
                    return new List<Exercise>();
                }
                
                var responseContent = await response.Content.ReadAsStringAsync();
                var exerciseDtos = JsonSerializer.Deserialize<List<ExerciseDto>>(responseContent, new JsonSerializerOptions { 
                    PropertyNameCaseInsensitive = true
                }) ?? new List<ExerciseDto>();
                
                return exerciseDtos.Select(dto => new Exercise
                {
                    Id = dto.Id,
                    Name = dto.Name,
                    Description = dto.Description,
                    VideoUrl = dto.VideoUrl,
                    MuscleGroups = dto.MuscleGroups.Select(mg => new MuscleGroup
                    {
                        Id = mg.Id,
                        Name = mg.Name
                    }).ToList()
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting exercises for workout");
                return new List<Exercise>();
            }
        }

        public async Task<List<MuscleGroup>> GetMuscleGroupsAsync()
        {
            try
            {
                var exercises = await GetExercisesForWorkoutAsync();
                return exercises
                    .SelectMany(e => e.MuscleGroups)
                    .GroupBy(mg => mg.Id)
                    .Select(g => g.First())
                    .OrderBy(mg => mg.Name)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting muscle groups");
                return new List<MuscleGroup>();
            }
        }

        public async Task<List<Exercise>> GetExercisesByMuscleGroupAsync(int muscleGroupId)
        {
            try
            {
                var exercises = await GetExercisesForWorkoutAsync();
                return exercises
                    .Where(e => e.MuscleGroups.Any(mg => mg.Id == muscleGroupId))
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting exercises by muscle group");
                return new List<Exercise>();
            }
        }
    }
}
