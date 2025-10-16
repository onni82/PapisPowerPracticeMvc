namespace PapisPowerPracticeMvc.Models.Auth.Response
{
    public class LoginResponse
    {
        public string AccessToken { get; set; }
        public DateTime Expiration { get; set; }
    }
}
