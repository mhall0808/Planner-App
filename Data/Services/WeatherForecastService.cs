using Microsoft.Extensions.Options;
using OpenWeatherMap.Standard;
using OpenWeatherMap.Standard.Models;
using PlannerApp.Data.Models;
using System;
using System.Threading.Tasks;

namespace PlannerApp.Data.Services
{
    public class WeatherForecastService
    {
        private readonly AppSettings _appSettings;

        public WeatherForecastService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public Task<WeatherForecast> GetForecastAsync(DateTime startDate)
        {
            string key = _appSettings.OpenWeatherMapApiKey;

            Current current = new Current(key);
            WeatherData data = null;
            Task getWeather = Task.Run(async () => { data = await current.GetWeatherDataByZipAsync("83401", "us"); }); // TODO - have a user input instead - loaded to the DB
            getWeather.Wait();

            return Task.FromResult(new WeatherForecast()
            {
                Date = data.AcquisitionDateTime,
                TemperatureC = data.WeatherDayInfo.Temperature,
                Summary = GetSummaryDataAsync((float)(32 + (int)data.WeatherDayInfo.Temperature / 0.5556)).Result
            });
        }

        /// <summary>
        /// Gets the summary data asynchronous.  
        /// </summary>
        /// <param name="temperature">The temperature in F.</param>
        /// <returns></returns>
        public Task<string> GetSummaryDataAsync(float temperature)
        {
            var summaryData = "It is cold.  Very cold.  Do not go out.  you will *suffer*";
            if (temperature > 0.0 && temperature < 32.0)
            {
                summaryData = "It's a little chilly outside.  Are you sure you want to go out?";
            }
            else if (temperature >= 32.0 && temperature < 50.0)
            {
                summaryData = "Cold, but bearable.  Wear your dang jacket.";
            }
            else if (temperature >= 50.0 && temperature < 70.0)
            {
                summaryData = "It's a beautiful day, my dudes!";
            }
            else if (temperature >= 70.0)
            {
                summaryData = "It's pretty dang hot outside.  Why go out when you can stay in and use your AC?";
            }

            return Task.FromResult(summaryData);
        }

    }
}
