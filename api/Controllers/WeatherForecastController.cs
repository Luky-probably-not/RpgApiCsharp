using Microsoft.AspNetCore.Mvc;
using Modeles;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<Dictionary<string, string>> Get()
        {
            var wf = new Dictionary<string, string>
            {
                { Resource.TemperatureCelsius , "27"},
                { Resource.Date , DateOnly.FromDateTime(DateTime.Now).ToString()}
            }

            return wf;
        }
    }
}
