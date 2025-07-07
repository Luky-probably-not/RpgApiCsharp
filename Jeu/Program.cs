using Modeles;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

HttpClient client = new HttpClient();

async Task<WeatherForecast> GetWeather(string path)
{
     HttpResponseMessage response = await client.GetAsync(path);
    var weather = await response.Content.ReadFromJsonAsync < WeatherForecast> ();
    return weather;
}

async Task RunAsync()
{
    client.BaseAddress = new Uri("http://localhost:5019");
    client.DefaultRequestHeaders.Accept.Clear();
    client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/json"));

    try
    {
        var weather = await GetWeather("/WeatherForecast/");

        Console.WriteLine(weather.Display());
        Console.WriteLine(weather.GetType().Name);
    }
    catch ( Exception e)
    {
        Console.WriteLine(e);
        throw;
    }
}

RunAsync().GetAwaiter().GetResult();