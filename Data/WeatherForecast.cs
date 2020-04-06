using System;

namespace PlannerApp.Data
{
    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public float TemperatureC { get; set; }

        public float TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }
    }
}
