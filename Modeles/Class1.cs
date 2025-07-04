namespace Modeles;
public class WeatherForecast
{
    public DateOnly Date { get; set; }

    public int TemperatureC { get; set; }

    public string Display()
    {
        return ($"date : {Date.ToString()}; Temperature : {TemperatureC}");
    }
}
