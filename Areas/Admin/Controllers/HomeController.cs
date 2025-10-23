using Microsoft.AspNetCore.Mvc;

namespace PapisPowerPracticeMvc.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
