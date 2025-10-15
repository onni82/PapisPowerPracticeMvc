using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace PapisPowerPracticeMvc.Models.Auth.Request
{
    public class LoginUserModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(6, ErrorMessage = "Lösenordet måste innehålla minst 6 karaktärer")]
        public string Password { get; set; }
    }
}
