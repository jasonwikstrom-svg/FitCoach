using FitCoach3.Services;
using FitCoach3.Services.Interfaces;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using FitCoach3;

namespace FitCoach3;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        builder.Services.AddScoped(sp => new HttpClient 
        { 
            BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) 
        });

        // Registrera services
        builder.Services.AddSingleton<AppState>();
        builder.Services.AddSingleton<IWorkoutService, WorkoutService>();
        builder.Services.AddSingleton<IRecommendationService, RecommendationService>();
        builder.Services.AddScoped<IStorageService, StorageService>();

        await builder.Build().RunAsync();
    }
}
