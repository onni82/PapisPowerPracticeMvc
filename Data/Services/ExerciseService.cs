using PapisPowerPracticeMvc.Data.Services.IService;
using PapisPowerPracticeMvc.ViewModels;

namespace PapisPowerPracticeMvc.Data.Services
{
    public class ExerciseService : IExerciseService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ExerciseService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<ExerciseViewModel>> GetAllExercisesAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<ExerciseViewModel>>("exercises") ?? new List<ExerciseViewModel>();
		}

        public async Task<ExerciseViewModel?> GetExerciseByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<ExerciseViewModel>($"exercises/{id}");
        }
    }
}
