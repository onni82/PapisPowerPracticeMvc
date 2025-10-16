namespace PapisPowerPracticeMvc.ViewModels
{
    public class ExerciseViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? VideoUrl { get; set; }
        public List<string>? MuscleGroups { get; set; } = new();
    }
}
