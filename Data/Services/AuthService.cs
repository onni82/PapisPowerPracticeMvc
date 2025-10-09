using PapisPowerPracticeMvc.Data.Services.IService;
using PapisPowerPracticeMvc.Models.Auth.Request;
using PapisPowerPracticeMvc.Models.Auth.Response;

namespace PapisPowerPracticeMvc.Data.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<LoginResponse> LoginUser(LoginUserModel model)
        {
            var response = await _httpClient.PostAsJsonAsync("Auth/login", model);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<LoginResponse>();
        }

        public async Task<bool> RegisterUser(RegisterUserModel model)
        {
            var response = await _httpClient.PostAsJsonAsync("Auth/register", model);
            return response.IsSuccessStatusCode;
        }
    }
}
