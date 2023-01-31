using Microsoft.AspNetCore;
using QuartzNETDemoLibrary;
using QuartzNETDemoLibrary.Models;

var builder = WebApplication
    .CreateBuilder();

var app = builder.Build();

var scheduler = new SchedulerService();
await scheduler.StartAsync();

// Configure the HTTP request pipeline.
app.MapGet("/configure", async (SchedulerConfiguration[] configuration) =>
{
    await scheduler.StopAsync();
    await scheduler.ConfigureSchedulerAsync(configuration);
    await scheduler.StartAsync();
});

app.Run("http://*:3000");

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}