namespace Modeles;
public class WeatherForecast
{
    public DateOnly Date { get; set; }
    public int TemperatureC { get; set; }

    public string Display()
    {
        return $"Celsius Date : {Date.ToString()}; temperature : {TemperatureC}";
    }
}
