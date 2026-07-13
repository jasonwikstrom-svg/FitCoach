using FitCoach3.Models;

namespace FitCoach3.Services;

public class AppState
{
    public List<WorkoutSession> CompletedSessions { get; set; } = new();
    public UserProfile UserProfile { get; set; } = new() { DaysPerWeek = 3 };
    public WorkoutPlan? CurrentPlan { get; set; }

    public event Action? OnChange;

    public void AddCompletedSession(WorkoutSession session)
    {
        CompletedSessions.Add(session);
        NotifyStateChanged();
    }

    public void SetCurrentPlan(WorkoutPlan plan)
    {
        CurrentPlan = plan;
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}