using Microsoft.AspNetCore.Mvc;

namespace PapisPowerPracticeMvc.Areas.Admin.Controllers
{
    public class MuscleGroupsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
