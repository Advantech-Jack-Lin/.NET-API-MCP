using ModelContextProtocol.AspNetCore;
using ModelContextProtocol.Server;
using Microsoft.OpenApi.Models;
using System.ComponentModel;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// MCP: Register MCP server for agent integration with HTTP transport and tool discovery
builder.Services.AddMcpServer()
    .WithHttpTransport()
    .WithToolsFromAssembly();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

var api = app.MapGroup("/api");

api.MapGet("/weatherforecast", () => ApiLogic.GetWeatherForecast())
    .WithName("GetWeatherForecast");

api.MapGet("/hello", () => ApiLogic.GetHelloWorld());

// MCP: Expose the MCP negotiation endpoint (default is /sse for this SDK version)
app.MapMcp();

app.Run();

public static class ApiLogic
{
    public static string GetHelloWorld() => "Hello World";

    public static WeatherForecast[] GetWeatherForecast()
    {
        var summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        return Enumerable.Range(1, 5).Select(index =>
            new WeatherForecast(
                DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Random.Shared.Next(-20, 55),
                summaries[Random.Shared.Next(summaries.Length)]
            )).ToArray();
    }
}

// MCP: Tool type for agent-accessible functions
[McpServerToolType]
public static class McpApiTools
{
    [McpServerTool, Description("Returns a simple 'Hello World' string for testing API availability.")]
    public static string Hello() => ApiLogic.GetHelloWorld();

    [McpServerTool, Description("Returns a 5-day weather forecast with random data for demonstration purposes.")]
    public static WeatherForecast[] WeatherForecast() => ApiLogic.GetWeatherForecast();
}

public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
