using PapisPowerPracticeMvc.Data.Services.IService;
using PapisPowerPracticeMvc.ViewModels;

namespace PapisPowerPracticeMvc.Data.Services
{
    public class MuscleGroupService : IMuscleGroupService
    {
        private readonly HttpClient _httpClient;

        public MuscleGroupService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<MuscleGroupViewModel>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<MuscleGroupViewModel>>("MuscleGroup") ?? new List<MuscleGroupViewModel>();
        }

        public async Task<MuscleGroupViewModel?> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<MuscleGroupViewModel>($"MuscleGroup/{id}");
        }

        public async Task<bool> CreateAsync(MuscleGroupViewModel muscleGroup)
        {
            var response = await _httpClient.PostAsJsonAsync("MuscleGroup", muscleGroup);
            return response.IsSuccessStatusCode;
        }
    }
}
