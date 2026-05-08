using Microsoft.AspNetCore.Mvc;
using WebApi1.Models;

namespace WebApi1.Controllers;

[ApiController]
[Route("api-v1/[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    [HttpGet("All")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            Summaries[Random.Shared.Next(Summaries.Length)]
        ))
        .ToArray();
    }

    // Метод 2: Доступен по GET api/WeatherForecast/today
    [HttpGet("today")]
    public WeatherForecast GetToday()
    {
        /* логика получения данных только на сегодня */
        return new WeatherForecast(DateOnly.FromDateTime(DateTime.Now), 25, "Hot");
    }
}