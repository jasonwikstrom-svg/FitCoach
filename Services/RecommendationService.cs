using FitCoach3.Models;
using FitCoach3.Models.Enums;
using FitCoach3.Services.Interfaces;

namespace FitCoach3.Services;

public class RecommendationService : IRecommendationService
{
    public SplitType RecommendNextSession(List<WorkoutSession> history)
    {
        var pushCount = history.Count(s => s.SessionName == "Push");
        var pullCount = history.Count(s => s.SessionName == "Pull");
        var legsCount = history.Count(s => s.SessionName == "Legs");

        if (legsCount <= pushCount && legsCount <= pullCount)
            return SplitType.PushPullLegs;
        if (pullCount <= pushCount)
            return SplitType.PushPullLegs;

        return SplitType.PushPullLegs;
    }

    public string GetRecommendationMessage(List<WorkoutSession> history)
    {
        var pushCount = history.Count(s => s.SessionName == "Push");
        var pullCount = history.Count(s => s.SessionName == "Pull");
        var legsCount = history.Count(s => s.SessionName == "Legs");

        if (legsCount < pushCount && legsCount < pullCount)
            return "Du borde köra ben idag!";
        if (pullCount < pushCount)
            return "Du borde köra pull idag!";
        if (pushCount < pullCount)
            return "Du borde köra push idag!";

        return "Bra balans! Kör vilket pass du känner för.";
    }
}