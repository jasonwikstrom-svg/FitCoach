namespace FitCoach3.Models;

public class WorkoutSession
{
    public string SessionName { get; set; } = string.Empty;
    public List<Exercise> Exercises { get; set; } = new();
    public bool IsCompleted { get; set; }
    public DateTime? CompletedAt { get; set; }
}