using PapisPowerPracticeMvc.Models.Auth.Request;
using PapisPowerPracticeMvc.Models.Auth.Response;

namespace PapisPowerPracticeMvc.Data.Services.IService
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginUser(LoginUserModel model);
        Task<bool> LogoutUser(int id);
        Task<bool> RegisterUser(RegisterUserModel model);
    }
}
