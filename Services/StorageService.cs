using System.Text.Json;
using FitCoach3.Models;
using FitCoach3.Services.Interfaces;
using Microsoft.JSInterop;

namespace FitCoach3.Services;

public class StorageService : IStorageService
{
    private readonly IJSRuntime _js;

    public StorageService(IJSRuntime js)
    {
        _js = js;
    }

    public async Task SaveUserProfileAsync(UserProfile profile)
    {
        var json = JsonSerializer.Serialize(profile);
        await _js.InvokeVoidAsync("localStorage.setItem", "userProfile", json);
    }

    public async Task<UserProfile?> LoadUserProfileAsync()
    {
        var json = await _js.InvokeAsync<string?>("localStorage.getItem", "userProfile");
        return json == null ? null : JsonSerializer.Deserialize<UserProfile>(json);
    }

    public async Task SaveWorkoutSessionAsync(WorkoutSession session)
    {
        var history = await LoadWorkoutHistoryAsync();
        history.Add(session);
        var json = JsonSerializer.Serialize(history);
        await _js.InvokeVoidAsync("localStorage.setItem", "workoutHistory", json);
    }

    public async Task<List<WorkoutSession>> LoadWorkoutHistoryAsync()
    {
        var json = await _js.InvokeAsync<string?>("localStorage.getItem", "workoutHistory");
        return json == null ? new List<WorkoutSession>() : JsonSerializer.Deserialize<List<WorkoutSession>>(json) ?? new();
    }
    
    public async Task SaveWorkoutPlanAsync(WorkoutPlan plan)
    {
        var json = JsonSerializer.Serialize(plan);
        await _js.InvokeVoidAsync("localStorage.setItem", "currentPlan", json);
    }

    public async Task<WorkoutPlan?> LoadWorkoutPlanAsync()
    {
        var json = await _js.InvokeAsync<string?>("localStorage.getItem", "currentPlan");
        return json == null ? null : JsonSerializer.Deserialize<WorkoutPlan>(json);
    }
}