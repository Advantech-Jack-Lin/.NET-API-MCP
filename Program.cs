using ModelContextProtocol.AspNetCore;
using ModelContextProtocol.Server;
using Microsoft.OpenApi.Models;
using System.ComponentModel;
using DemoApi;
using DemoApi.Services;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register services
builder.Services.AddScoped<IGreetingService, GreetingService>();

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

api.MapGet("/greeting", (IGreetingService greetingService) => greetingService.GetServiceGreeting());

// MCP: Expose the MCP negotiation endpoint (default is /sse for this SDK version)
app.MapMcp();

app.Run();

// MCP: Tool type for agent-accessible functions
[McpServerToolType]
public static class McpApiTools
{
    [McpServerTool, Description("Returns a simple 'Hello World' string for testing API availability.")]
    public static string Hello() => ApiLogic.GetHelloWorld();

    [McpServerTool, Description("Returns a 5-day weather forecast with random data for demonstration purposes.")]
    public static ApiLogic.WeatherForecast[] WeatherForecast() => ApiLogic.GetWeatherForecast();
}


