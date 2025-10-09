using Microsoft.AspNetCore.Mvc;
using PapisPowerPracticeMvc.Models;
using System.Diagnostics;

namespace PapisPowerPracticeMvc.Controllers
{
    public class WorkoutLogController : Controller
    {
        private readonly ILogger<WorkoutLogController> _logger;

        public WorkoutLogController(ILogger<WorkoutLogController> logger)
        {
            _logger = logger;
        }

        public IActionResult WorkoutLog()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
