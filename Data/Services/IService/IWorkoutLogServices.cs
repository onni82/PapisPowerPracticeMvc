using PapisPowerPracticeMvc.Models;

namespace PapisPowerPracticeMvc.Data.Services.IService
{
    public interface IWorkoutLogServices
    {
        Task<List<WorkoutLogVM>> WorkoutLogsForUserAsync(string jwtToken);
        Task<bool> SaveWorkoutAsync(CreateWorkoutLogDTO createWorkoutLogDTO, string jwtToken);
    }
}
