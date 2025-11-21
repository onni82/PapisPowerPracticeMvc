namespace PapisPowerPracticeMvc.Models
{
    public class CreateWorkoutLogDTO
    {
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? Notes { get; set; }

        public List<CreateWorkoutExerciseDTO> Exercises { get; set; } = new();
    }
}