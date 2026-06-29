using FitCoach3.Models;
using FitCoach3.Models.Enums;

namespace FitCoach3.Services.Interfaces;

public interface IWorkoutService
{
    WorkoutPlan GeneratePlan(UserProfile profile);
    List<Exercise> GetExercises(SplitType splitType, TrainingLocation location);
}