using FitCoach3.Models;
using FitCoach3.Models.Enums;
using FitCoach3.Services.Interfaces;

namespace FitCoach3.Services;

public class WorkoutService : IWorkoutService
{
    public WorkoutPlan GeneratePlan(UserProfile profile)
    {
        var splitType = profile.DaysPerWeek switch
        {
            2 => SplitType.FullBody,
            3 => SplitType.PushPullLegs,
            4 => SplitType.UpperLower,
            _ => SplitType.PushPullLegsUpperLower
        };

        var plan = new WorkoutPlan
        {
            Name = $"{GoalToSwedish(profile.Goal)} – {profile.DaysPerWeek} dagar/vecka",
            SplitType = splitType,
            Sessions = GenerateSessions(splitType, profile.Location)
        };

        return plan;
    }
    
    private string GoalToSwedish(Goal goal) => goal switch
    {
        Goal.BuildMuscle => "Bygga muskler",
        Goal.LoseWeight => "Gå ner i vikt",
        Goal.Cardio => "Kondition",
        _ => goal.ToString()
    };

    public List<Exercise> GetExercises(SplitType splitType, TrainingLocation location)
    {
        return splitType switch
        {
            SplitType.FullBody => FullBodyExercises(location),
            SplitType.PushPullLegs => PushExercises(location),
            _ => FullBodyExercises(location)
        };
    }

    private List<WorkoutSession> GenerateSessions(SplitType splitType, TrainingLocation location)
    {
        return splitType switch
        {
            SplitType.FullBody => new List<WorkoutSession>
            {
                new() { SessionName = "Helkropp A", Exercises = FullBodyExercises(location) },
                new() { SessionName = "Helkropp B", Exercises = FullBodyExercises(location) }
            },
            SplitType.PushPullLegs => new List<WorkoutSession>
            {
                new() { SessionName = "Push", Exercises = PushExercises(location) },
                new() { SessionName = "Pull", Exercises = PullExercises(location) },
                new() { SessionName = "Legs", Exercises = LegExercises(location) }
            },
            _ => new List<WorkoutSession>
            {
                new() { SessionName = "Helkropp", Exercises = FullBodyExercises(location) }
            }
        };
    }

    private List<Exercise> FullBodyExercises(TrainingLocation location) => location == TrainingLocation.Gym
        ? new List<Exercise>
        {
            new() { Name = "Knäböj", Sets = 4, Reps = "6-8" },
            new() { Name = "Bänkpress", Sets = 4, Reps = "6-8" },
            new() { Name = "Marklyft", Sets = 3, Reps = "5" },
            new() { Name = "Rodd", Sets = 3, Reps = "8-10" }
        }
        : new List<Exercise>
        {
            new() { Name = "Armhävningar", Sets = 4, Reps = "10-15" },
            new() { Name = "Utfall", Sets = 3, Reps = "12" },
            new() { Name = "Plankan", Sets = 3, Reps = "60 sek" },
            new() { Name = "Rygglyft", Sets = 3, Reps = "12" }
        };
    
    private List<Exercise> PushExercises(TrainingLocation location) =>
        location == TrainingLocation.Gym
            ? new List<Exercise>
            {
                new() { Name = "Bänkpress", Sets = 4, Reps = "6-8" },
                new() { Name = "Axelpress", Sets = 3, Reps = "8-10" },
                new() { Name = "Tricepspress", Sets = 3, Reps = "10-12" }
            }
            : new List<Exercise>
            {
                new() { Name = "Armhävningar", Sets = 4, Reps = "10-15" },
                new() { Name = "Dips", Sets = 3, Reps = "10-12" },
                new() { Name = "Pike push-up", Sets = 3, Reps = "10" }
            };
    
    private List<Exercise> PullExercises(TrainingLocation location) =>
        location == TrainingLocation.Gym
            ? new List<Exercise>
            {
                new() { Name = "Latsdrag", Sets = 4, Reps = "8-10" },
                new() { Name = "Rodd", Sets = 4, Reps = "8-10" },
                new() { Name = "Bicepscurl", Sets = 3, Reps = "10-12" }
            }
            : new List<Exercise>
            {
                new() { Name = "Chinups", Sets = 4, Reps = "max" },
                new() { Name = "Inverted row", Sets = 3, Reps = "10-12" },
                new() { Name = "Bicepscurl med band", Sets = 3, Reps = "12" }
            };
    
    private List<Exercise> LegExercises(TrainingLocation location) =>
        location == TrainingLocation.Gym
            ? new List<Exercise>
            {
                new() { Name = "Knäböj", Sets = 4, Reps = "6-8" },
                new() { Name = "Benspark", Sets = 3, Reps = "10-12" },
                new() { Name = "Marklyft", Sets = 3, Reps = "6-8" }
            }
            : new List<Exercise>
            {
                new() { Name = "Utfall", Sets = 4, Reps = "12" },
                new() { Name = "Enbensknäböj", Sets = 3, Reps = "8" },
                new() { Name = "Höftlyft", Sets = 3, Reps = "15" }
            };
}