using FitCoach3.Models;

namespace FitCoach3.Services;

public class AppState
{
    public List<WorkoutSession> CompletedSessions { get; set; } = new();
    public UserProfile UserProfile { get; set; } = new() { DaysPerWeek = 3 };

    public event Action? OnChange;

    public void AddCompletedSession(WorkoutSession session)
    {
        CompletedSessions.Add(session);
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}