using System.ComponentModel.DataAnnotations;

namespace PapisPowerPracticeMvc.Models
{
    public class CalorieDataViewModel
    {
        [Required(ErrorMessage = "Kön måste anges.")]
        [RegularExpression("^(male|female)$", ErrorMessage = "Kön måste vara 'male' eller 'female'.")]
        public string Gender { get; set; } = "";

        [Required(ErrorMessage = "Vikt krävs.")]
        [Range(20, 300, ErrorMessage = "Vikt måste vara mellan 20 och 300 kg.")]
        public double Weight { get; set; }

        [Required(ErrorMessage = "Längd krävs.")]
        [Range(100, 250, ErrorMessage = "Längd måste vara mellan 100 och 250 cm.")]
        public double Height { get; set; }

        [Required(ErrorMessage = "Ålder krävs.")]
        [Range(5, 120, ErrorMessage = "Ålder måste vara mellan 5 och 120 år.")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Aktivitetsnivå måste väljas.")]
        [RegularExpression("^(sedentary|lightly_active|moderately_active|active|very_active)$", ErrorMessage = "Ogiltig aktivitetsnivå.")]
        public string ActivityLevel { get; set; } = "";
    }
}
