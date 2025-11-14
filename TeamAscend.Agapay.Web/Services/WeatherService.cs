using Newtonsoft.Json;
using TeamAscend.Agapay.Web.Models.OpenWeatherMap;

namespace TeamAscend.Agapay.Web.Services
{
    public class WeatherService
    {
        public static WeatherService Instance { get; private set; }

        private const string API_KEY = "5238980f6760db928cd80bce921243bf"; // Replace with your actual OpenWeatherMap API key
        //Currently hard coded sa QC NCR
        private const string BASE_URL = "https://api.openweathermap.org/data/2.5/weather?lat=14.6510546&lon=121.0486254";

        public WeatherService()
        {
            Instance = this;
        }

        public async Task<WeatherResponse?> GetTodays()
        {
            WeatherResponse? response = null;

            using (var httpClient = new HttpClient())
            {
                try
                {
                    var url = $"{BASE_URL}&appid={API_KEY}&units=metric";
                    var rawResponse = await httpClient.GetStringAsync(url);
                    
                    response = JsonConvert.DeserializeObject<WeatherResponse>(rawResponse);
                }
                catch (Exception ex)
                {
                    // Handle exception (log it, return null, etc.)
                    Console.WriteLine($"Error fetching weather data: {ex.Message}");
                }
            }

            return response;
        }

        // Alternative method using latitude and longitude
        public static async Task<WeatherResponse?> GetTodays(double latitude, double longitude)
        {
            WeatherResponse? response = null;

            using (var httpClient = new HttpClient())
            {
                try
                {
                    var url = $"{BASE_URL}?lat={latitude}&lon={longitude}&appid={API_KEY}&units=metric";
                    var rawResponse = await httpClient.GetStringAsync(url);
                    
                    response = JsonConvert.DeserializeObject<WeatherResponse>(rawResponse);
                }
                catch (Exception ex)
                {
                    // Handle exception (log it, return null, etc.)
                    Console.WriteLine($"Error fetching weather data: {ex.Message}");
                }
            }

            return response;
        }
    }
}