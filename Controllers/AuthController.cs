using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using PapisPowerPracticeMvc.Data.Services.IService;
using PapisPowerPracticeMvc.Models.Auth.Request;
using PapisPowerPracticeMvc.Models.Auth.Response;
using System.Reflection.Metadata.Ecma335;

namespace PapisPowerPracticeMvc.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var success = await _authService.RegisterUser(model);

            if (!success)
            {
                ModelState.AddModelError("", "registeringen misslyckades");
                return View(model);
            }

            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginUserModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            var response = await _authService.LoginUser(model);

            if (response == null || string.IsNullOrEmpty(response.AccessToken))
            {
                ModelState.AddModelError("", "inloggning misslyckades");
                return View(model);
            }

            Response.Cookies.Append("jwt", response.AccessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddHours(1)
            }); 

            

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
           

        }
    }
}
