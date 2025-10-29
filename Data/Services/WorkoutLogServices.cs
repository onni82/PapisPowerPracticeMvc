using PapisPowerPracticeMvc.Data.Services.IService;
using PapisPowerPracticeMvc.Models;
using System.Text.Json;

namespace PapisPowerPracticeMvc.Data.Services
{
    public class WorkoutLogServices : IWorkoutLogServices
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<WorkoutLogServices> _logger;
        private readonly IConfiguration _configuration;

        public WorkoutLogServices(HttpClient httpClient, ILogger<WorkoutLogServices> logger, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _logger = logger;
            _configuration = configuration;
        }



        public async Task<bool> SaveWorkoutAsync(CreateWorkoutLogDTO createWorkoutLogDTO, string jwtToken)
        {
            try
            {
                var json = JsonSerializer.Serialize(createWorkoutLogDTO);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwtToken);
                
                var baseUrl = _configuration["ApiSettings:BaseUrl"];
                var response = await _httpClient.PostAsync($"{baseUrl}/api/WorkoutLog", content);
                

                
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
