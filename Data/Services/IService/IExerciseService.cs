using PapisPowerPracticeMvc.Models;
using PapisPowerPracticeMvc.ViewModels;

namespace PapisPowerPracticeMvc.Data.Services.IService
{
    public interface IExerciseService
    {
        Task<IEnumerable<ExerciseViewModel>> GetAllAsync();
        Task<ExerciseViewModel?> GetByIdAsync(int id);
        Task<List<Exercise>> GetExercisesForWorkoutAsync();
        Task<List<MuscleGroup>> GetMuscleGroupsAsync();
        Task<List<Exercise>> GetExercisesByMuscleGroupAsync(int muscleGroupId);
        Task<bool> AddExercise(CreateExerciseViewModel model);
        Task<bool> UpdateExercise(int id, ExerciseViewModel model);
        Task<bool> Delete(int id);
    }
}
