using PapisPowerPracticeMvc.Data.Services.IService;
using PapisPowerPracticeMvc.Models;
using System.Net.Http.Json;

namespace PapisPowerPracticeMvc.Data.Services
{
    public class CalorieIntakeService : ICalorieIntakeService
    {
        private readonly HttpClient _httpClient;

        public CalorieIntakeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<string>> GetActivityLevelsAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<string>>("CalorieIntake/activity-levels") ?? [];
        }

        public async Task<CalorieResultViewModel?> CalculateAsync(CalorieDataViewModel model)
        {
            var response = await _httpClient.PostAsJsonAsync("CalorieIntake/calculate", model);

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<CalorieResultViewModel>();
        }
    }
}
