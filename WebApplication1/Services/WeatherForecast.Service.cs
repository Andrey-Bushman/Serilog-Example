namespace WebApplication1.Services {
    public class WeatherForecastService {

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public async Task<WeatherForecast[]> ProcessFTemperature() {

            await Task.Delay(10);

            var result = Enumerable.Range(1, 5).Select(index => new WeatherForecast {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();

            await Task.Delay(10);

            return result;
        }
    }
}
