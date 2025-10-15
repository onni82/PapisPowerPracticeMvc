using System.ComponentModel.DataAnnotations;

namespace PapisPowerPracticeMvc.Models.Auth.Request
{
    public class RegisterUserModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(6, ErrorMessage = "Lösenordet måste innehålla minst 6 karaktärer")]
        public string Password { get; set; }
    }
}
