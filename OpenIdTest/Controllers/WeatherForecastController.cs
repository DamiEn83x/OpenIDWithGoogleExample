using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace OpenIdTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        }; 
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }



        [HttpGet("GetLoggedUser")]
        [Authorize] // Requires authentication for this endpoint
        public string GetLoggedUser()
        {
            var identity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            var nameClaim = identity?.FindFirst(ClaimTypes.Name);
            var id = identity?.FindFirst(ClaimTypes.NameIdentifier);

            if (nameClaim != null)
            {
                return id.Value+" "+ nameClaim.Value;
            }

            return string.Empty;
        }

        [HttpGet("GetWeatherForecast")]
        [Authorize] // Requires authentication for this endpoint
        public IEnumerable<WeatherForecast> Get()
        {



            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}