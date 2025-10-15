using PapisPowerPracticeMvc.Models;
using PapisPowerPracticeMvc.ViewModels;

namespace PapisPowerPracticeMvc.Data.Services.IService
{
    public interface IExerciseService
    {
        Task<ExerciseViewModel?> GetExerciseByIdAsync(int id);
    }
}
