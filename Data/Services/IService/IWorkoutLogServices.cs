using PapisPowerPracticeMvc.Models;

namespace PapisPowerPracticeMvc.Data.Services.IService
{
    public interface IWorkoutLogServices
    {
        Task<List<Exercise>> GetExercisesAsync();
        Task<Exercise?> GetExerciseByIdAsync(int id);
        Task<WorkoutExerciseViewModel?> CreateWorkoutExerciseAsync(int exerciseId);
        Task<bool> SaveWorkoutAsync(WorkoutLog workoutLog);
    }
}
