using PapisPowerPracticeMvc.Data.Services.IService;
using PapisPowerPracticeMvc.Models.Auth.Request;
using PapisPowerPracticeMvc.Models.Auth.Response;

namespace PapisPowerPracticeMvc.Data.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
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

        public async Task<bool> LogoutUser(int id)
        {
            var response = await _httpClient.PostAsync($"Auth/logout/{id}", null);

            _httpContextAccessor.HttpContext.Response.Cookies.Delete("jwt");

            return response.IsSuccessStatusCode;
        }
    }
}
