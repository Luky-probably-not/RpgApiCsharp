using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modeles;
public class WeatherFarenheit
{
    public int TemperatureF;

    public DateOnly Date { get; set; }

    public string Display()
    {
        return $"Farenheit Date : {Date.ToString()}; temperature : {TemperatureF}";
    }

}
