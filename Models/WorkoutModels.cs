using Microsoft.AspNetCore.Identity;

namespace PapisPowerPracticeMvc.Models
{
    public class Exercise
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string VideoUrl { get; set; } = string.Empty;
        public ICollection<MuscleGroup> MuscleGroups { get; set; } = new List<MuscleGroup>();
    }

    public class MuscleGroup
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<Exercise> Exercises { get; set; } = new List<Exercise>();
    }

    public class WorkoutLog
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? Notes { get; set; }
        public string UserId { get; set; } = string.Empty;
        public IdentityUser User { get; set; } = null!;
        public ICollection<WorkoutExercise> WorkoutExercises { get; set; } = new List<WorkoutExercise>();
    }

    public class WorkoutExercise
    {
        public int Id { get; set; }
        public int WorkoutLogId { get; set; }
        public WorkoutLog WorkoutLog { get; set; } = null!;
        public int ExerciseId { get; set; }
        public Exercise Exercise { get; set; } = null!;
        public ICollection<WorkoutSet> Sets { get; set; } = new List<WorkoutSet>();
    }

    public class WorkoutSet
    {
        public int Id { get; set; }
        public int WorkoutExerciseId { get; set; }
        public WorkoutExercise WorkoutExercise { get; set; } = null!;
        public int Reps { get; set; }
        public decimal Weight { get; set; }
    }

    public class WorkoutLogViewModel
    {
        public int? WorkoutLogId { get; set; }
        public List<Exercise> AvailableExercises { get; set; } = new List<Exercise>();
        public List<WorkoutExerciseViewModel> WorkoutExercises { get; set; } = new List<WorkoutExerciseViewModel>();
        public string? Notes { get; set; }
    }

    public class WorkoutExerciseViewModel
    {
        public int ExerciseId { get; set; }
        public string ExerciseName { get; set; } = string.Empty;
        public List<WorkoutSetViewModel> Sets { get; set; } = new List<WorkoutSetViewModel>();
    }

    public class WorkoutSetViewModel
    {
        public int Reps { get; set; }
        public decimal Weight { get; set; }
    }
}