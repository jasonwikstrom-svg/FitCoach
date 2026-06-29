using FitCoach3.Models.Enums;

namespace FitCoach3.Models;

public class WorkoutPlan
{
    public string Name { get; set; } = string.Empty;
    public SplitType SplitType { get; set; }
    public List<WorkoutSession> Sessions { get; set; } = new();
}