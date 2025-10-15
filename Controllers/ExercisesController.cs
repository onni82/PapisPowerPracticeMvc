using Microsoft.AspNetCore.Mvc;

namespace PapisPowerPracticeMvc.Controllers
{
    public class ExercisesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
