using PapisPowerPracticeMvc.Data.Services.IService;
using PapisPowerPracticeMvc.ViewModels;

namespace PapisPowerPracticeMvc.Data.Services
{
    public class ExerciseService : IExerciseService
    {
        private readonly HttpClient _httpClient;

        public ExerciseService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ExerciseViewModel?> GetExerciseByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<ExerciseViewModel>($"exercises/{id}");
        }
    }
}
