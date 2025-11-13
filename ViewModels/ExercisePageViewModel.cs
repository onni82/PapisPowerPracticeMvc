namespace PapisPowerPracticeMvc.ViewModels
{
    public class ExercisePageViewModel
    {
        public List<ExerciseViewModel> Exercises { get; set; } = new();
        public CreateExerciseViewModel NewExercise { get; set; } = new();
    }
}
