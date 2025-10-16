using PapisPowerPracticeMvc.Models;
using PapisPowerPracticeMvc.ViewModels;

namespace PapisPowerPracticeMvc.Data.Services.IService
{
    public interface IExerciseService
    {
        Task<IEnumerable<ExerciseViewModel>> GetAllExercisesAsync();>
        Task<ExerciseViewModel?> GetExerciseByIdAsync(int id);
    }
}
