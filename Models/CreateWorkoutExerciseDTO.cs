namespace PapisPowerPracticeMvc.Models
{
    public class CreateWorkoutExerciseDTO
    {
        public int ExerciseId { get; set; }
        public List<CreateWorkoutSetDTO> Sets { get; set; } = new();
    }
    public class CreateWorkoutSetDTO
    {
        public int Reps { get; set; }
        public decimal Weight { get; set; }
        public bool IsWarmup { get; set; }
    }
}
