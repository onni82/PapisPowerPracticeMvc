using PapisPowerPracticeMvc.ViewModels;

namespace PapisPowerPracticeMvc.Data.Services.IService
{
    public interface ICalorieIntakeService
    {
        Task<IEnumerable<string>> GetActivityLevelsAsync();
        Task<CalorieResultViewModel?> CalculateAsync(CalorieDataViewModel model);
    }
}
