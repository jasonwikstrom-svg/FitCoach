using FitCoach3.Models;

namespace FitCoach3.Services.Interfaces;

public interface IStorageService
{
    Task SaveUserProfileAsync(UserProfile profile);
    Task<UserProfile?> LoadUserProfileAsync();
    Task SaveWorkoutSessionAsync(WorkoutSession session);
    Task<List<WorkoutSession>> LoadWorkoutHistoryAsync();
}