using Microsoft.AspNetCore.Mvc;
using PapisPowerPracticeMvc.Data.Services.IService;
using PapisPowerPracticeMvc.ViewModels;

namespace PapisPowerPracticeMvc.Controllers
{
    public class CalorieIntakeController : Controller
    {
        private readonly ICalorieIntakeService _service;

        public CalorieIntakeController(ICalorieIntakeService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewBag.ActivityLevels = await _service.GetActivityLevelsAsync();
            return View(new CalorieDataViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Index(CalorieDataViewModel model)
        {
            ViewBag.ActivityLevels = await _service.GetActivityLevelsAsync();

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _service.CalculateAsync(model);

            if (result == null)
            {
                ViewBag.Error = "Kunde inte beräkna kalorier. Kontrollera dina värden.";
                return View(model);
            }

            ViewBag.Result = result;
            return View(model);
        }
    }
}
