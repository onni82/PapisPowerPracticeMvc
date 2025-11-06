using System.ComponentModel.DataAnnotations;

namespace PapisPowerPracticeMvc.ViewModels
{
    public class MuscleGroupViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Namn är obligatoriskt")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "URL är obligatorisk")]
        [Url(ErrorMessage = "Ange en giltig URL")]
        public string ImageUrl { get; set; } = string.Empty;
    }
}
