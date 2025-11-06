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
            var response = await _httpClient.GetAsync($"MuscleGroup/{id}");

            if (!response.IsSuccessStatusCode)
            {
                // T.ex. 404- eller 500-fel
                return null;
            }

			// Vid 204 No content, returnera null
            if (response.Content.Headers.ContentLength == 0)
            {
                return null;
            }

			// Deserialisera svaret till MuscleGroupViewModel
            return await response.Content.ReadFromJsonAsync<MuscleGroupViewModel>();
		}

		public async Task<bool> CreateAsync(MuscleGroupViewModel muscleGroup)
        {
            var response = await _httpClient.PostAsJsonAsync("MuscleGroup", muscleGroup);
            return response.IsSuccessStatusCode;
        }
        public async Task<bool> UpdateAsync(int id, MuscleGroupViewModel muscleGroup)
        {
            var response = await _httpClient.PutAsJsonAsync($"MuscleGroup/{id}", muscleGroup);
            return response.IsSuccessStatusCode;
        }
    }
}
