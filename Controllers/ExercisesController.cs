using Microsoft.AspNetCore.Mvc;
using PapisPowerPracticeMvc.Models;

namespace PapisPowerPracticeMvc.Controllers
{
    public class ExercisesController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ExercisesController(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> Details(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<ExerciseViewModel>($"api/exercises/{id}");
            if (response == null)
                return NotFound();

            return View(response);
        }
    }
}
