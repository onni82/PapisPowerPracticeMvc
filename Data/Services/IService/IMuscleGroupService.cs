using PapisPowerPracticeMvc.ViewModels;

namespace PapisPowerPracticeMvc.Data.Services.IService
{
    public interface IMuscleGroupService
    {
        Task<IEnumerable<MuscleGroupViewModel>> GetAllAsync();
        Task<MuscleGroupViewModel?> GetByIdAsync(int id);
        Task<bool> CreateAsync(MuscleGroupViewModel muscleGroup);
        Task<bool> UpdateAsync(int id, MuscleGroupViewModel muscleGroup);
        Task<bool> DeleteAsync(int id);
    }
}
