using PapisPowerPracticeMvc.ViewModels;

namespace PapisPowerPracticeMvc.Data.Services.IService
{
    public interface IMuscleGroupService
    {
        Task<IEnumerable<MuscleGroupViewModel>> GetAllAsync();
        Task<MuscleGroupViewModel?> GetByIdAsync(int id);
    }
}
