using System.ComponentModel.DataAnnotations;

namespace PlannerApp.Data.Models
{
    public class AppSettings
    {
        [Required]
        public string JwtSecret { get; set; }
                
        public string ConnectionString { get; set; }

        [Required]
        public string OpenWeatherMapApiKey { get; set; }
    }
}
