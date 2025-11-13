namespace PapisPowerPracticeMvc.ViewModels
{
    public class CreateExerciseViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? VideoUrl { get; set; }
        public List<int> SelectedMuscleGroupIds { get; set; } = new();

        public List<MuscleGroupViewModel> AvailableMuscleGroups { get; set; } = new();
    }
}
