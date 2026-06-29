using FitCoach3.Models.Enums;

namespace FitCoach3.Models;

public class UserProfile
{
    public string Name { get; set; } = string.Empty;
    public Goal Goal { get; set; }
    public TrainingLocation Location { get; set; }
    public SplitType SplitType { get; set; }
    public int DaysPerWeek { get; set; }
    public int Age { get; set; }
    public double Weight { get; set; }
    public double Height { get; set; }
    public int SessionDurationMinutes { get; set; } = 60;
}