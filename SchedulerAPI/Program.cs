using Microsoft.AspNetCore;
using QuartzNETDemoLibrary;
using QuartzNETDemoLibrary.Models;

var builder = WebApplication
    .CreateBuilder();

var app = builder.Build();

var scheduler = new SchedulerService();
scheduler.Start();

// Configure the HTTP request pipeline.
app.MapPost("/configure", (SchedulerConfiguration[] configuration) =>
{
    scheduler.Stop();
    scheduler.ConfigureScheduler(configuration);
    scheduler.Start();
});

app.MapPost("/append", async (SchedulerConfiguration[] configuration) =>
{
    scheduler.Stop();
    await scheduler.AddSchedulerTaskAsync(configuration);
    scheduler.Start();
});

app.MapPost("/remove", async (SchedulerConfiguration[] configuration) =>
{
    scheduler.Stop();
    await scheduler.RemoveSchedulerTaskAsync(configuration);
    scheduler.Start();
});

app.Run("http://*:3000");

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}