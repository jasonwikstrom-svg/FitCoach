using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using FitCoach3.Models;
using FitCoach3.Models.Enums;

namespace FitCoach3.Services;

public class AiCoachService
{
    private readonly HttpClient _httpClient;
    private const string FunctionUrl = "https://fitcoach-api-proxy-v2-d5d2cwgjhehhb2em.swedencentral-01.azurewebsites.net/api/coach";

    public AiCoachService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<WorkoutPlan> GenerateWorkoutPlanAsync(UserProfile profile)
    {
        var goalText = profile.Goal switch
        {
            Goal.BuildMuscle => "muskeltillväxt",
            Goal.LoseWeight => "viktnedgång",
            Goal.Cardio => "kondition",
            _ => "allmän fitness"
        };
        var locationText = profile.Location == TrainingLocation.Gym
            ? "gym med fri tillgång till maskiner och fria vikter"
            : "hemma med begränsad eller ingen utrustning";

        var prompt = $@"Skapa ett träningsschema för {profile.DaysPerWeek} dagar per vecka.
Mål: {goalText}.
Plats: {locationText}.
Passlängd: cirka {profile.SessionDurationMinutes} minuter per pass.

Svara ENDAST med giltig JSON, exakt i detta format, utan markdown-kodblock eller extra text:
{{
  ""planName"": ""kort beskrivande namn"",
  ""splitType"": ""FullBody"" | ""PushPullLegs"" | ""UpperLower"" | ""PushPullLegsUpperLower"",
  ""sessions"": [
    {{
      ""sessionName"": ""t.ex. Push Day"",
      ""exercises"": [
        {{ ""name"": ""övningsnamn"", ""sets"": 4, ""reps"": ""8-10"" }}
      ]
    }}
  ]
}}

Antalet sessioner i listan ska matcha {profile.DaysPerWeek} dagar per vecka.";

        var payload = new { prompt };
        var response = await _httpClient.PostAsJsonAsync(FunctionUrl, payload);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadFromJsonAsync<JsonElement>();
        var text = json.GetProperty("content")[0].GetProperty("text").GetString() ?? "";

        var cleaned = text.Trim();
        if (cleaned.StartsWith("```"))
        {
            cleaned = cleaned.Replace("```json", "").Replace("```", "").Trim();
        }

        var dto = JsonSerializer.Deserialize<AiWorkoutPlanDto>(cleaned, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (dto is null)
        {
            throw new InvalidOperationException("Kunde inte tolka AI-svaret som ett träningsschema.");
        }

        Enum.TryParse<SplitType>(dto.SplitType, true, out var parsedSplitType);

        var plan = new WorkoutPlan
        {
            Name = dto.PlanName,
            SplitType = parsedSplitType,
            Sessions = dto.Sessions.Select(s => new WorkoutSession
            {
                SessionName = s.SessionName,
                Exercises = s.Exercises.Select(e => new Exercise
                {
                    Name = e.Name,
                    Sets = e.Sets,
                    Reps = e.Reps
                }).ToList()
            }).ToList()
        };

        return plan;
    }

    private class AiWorkoutPlanDto
    {
        [JsonPropertyName("planName")]
        public string PlanName { get; set; } = string.Empty;

        [JsonPropertyName("splitType")]
        public string SplitType { get; set; } = string.Empty;

        [JsonPropertyName("sessions")]
        public List<AiSessionDto> Sessions { get; set; } = new();
    }

    private class AiSessionDto
    {
        [JsonPropertyName("sessionName")]
        public string SessionName { get; set; } = string.Empty;

        [JsonPropertyName("exercises")]
        public List<AiExerciseDto> Exercises { get; set; } = new();
    }

    private class AiExerciseDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("sets")]
        public int Sets { get; set; }

        [JsonPropertyName("reps")]
        public string Reps { get; set; } = string.Empty;
    }
}