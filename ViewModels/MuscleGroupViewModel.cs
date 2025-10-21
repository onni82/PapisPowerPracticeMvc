namespace PapisPowerPracticeMvc.ViewModels
{
    public class MuscleGroupViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<ExerciseViewModel>? Exercises { get; set; }
    }
}
