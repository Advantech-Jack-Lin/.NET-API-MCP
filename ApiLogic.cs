namespace DemoApi;

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

    public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
    {
        // Use the standard conversion formula rather than an approximation to
        // avoid rounding errors when converting from Celsius to Fahrenheit.
        public int TemperatureF => 32 + (int)(TemperatureC * 9.0 / 5.0);
    }
}


