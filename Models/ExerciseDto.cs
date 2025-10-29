namespace PapisPowerPracticeMvc.Models
{
    public class ExerciseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string VideoUrl { get; set; } = string.Empty;
        public List<MuscleGroupDto> MuscleGroups { get; set; } = new List<MuscleGroupDto>();
    }

    public class MuscleGroupDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
    }
}