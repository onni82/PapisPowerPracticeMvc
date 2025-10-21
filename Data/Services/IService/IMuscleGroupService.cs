using PapisPowerPracticeMvc.ViewModels;

namespace PapisPowerPracticeMvc.Data.Services.IService
{
    public interface IMuscleGroupService
    {
        Task<IEnumerable<MuscleGroupViewModel>> GetAllMuscleGroupsAsync();
        Task<MuscleGroupViewModel?> GetMuscleGroupByIdAsync(int id);
    }
}
