namespace PapisPowerPracticeMvc.Models
{
    public class CalorieResultViewModel
    {
        public string Gender { get; set; } = "";
        public string ActivityLevel { get; set; } = "";
        public double Bmr { get; set; }
        public double Tdee { get; set; }
    }
}
