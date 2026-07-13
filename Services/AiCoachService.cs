using System.Net.Http.Json;
using System.Text.Json;

namespace FitCoach3.Services;

public class AiCoachService
{
    private readonly HttpClient _httpClient;
    private const string FunctionUrl = "https://fitcoach-api-proxy-v2-d5d2cwgjhehhb2em.swedencentral-01.azurewebsites.net/api/coach";

    public AiCoachService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GetCoachingAdviceAsync(string prompt)
    {
        var payload = new { prompt };

        var response = await _httpClient.PostAsJsonAsync(FunctionUrl, payload);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadFromJsonAsync<JsonElement>();
        var text = json
            .GetProperty("content")[0]
            .GetProperty("text")
            .GetString();

        return text ?? "Inget svar från AI-coachen.";
    }
}