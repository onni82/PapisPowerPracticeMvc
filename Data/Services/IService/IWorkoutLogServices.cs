using PapisPowerPracticeMvc.Models;

namespace PapisPowerPracticeMvc.Data.Services.IService
{
    public interface IWorkoutLogServices
    {
        Task<bool> SaveWorkoutAsync(CreateWorkoutLogDTO createWorkoutLogDTO, string jwtToken);
    }
}
