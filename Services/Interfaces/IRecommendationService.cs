using FitCoach3.Models;
using FitCoach3.Models.Enums;

namespace FitCoach3.Services.Interfaces;

public interface IRecommendationService
{
    SplitType RecommendNextSession(List<WorkoutSession> history);
    string GetRecommendationMessage(List<WorkoutSession> history);
}