using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication1.Services;

namespace WebApplication1.Controllers {

    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase {

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IHostEnvironment _hostEnv;

        private Dictionary<string, object> GetScopeInformation() {

            var scopeInfo = new Dictionary<string, object> {
                { "MachineName", Environment.MachineName },
                { "UserName", Environment.UserName },
                { "Environment", _hostEnv.EnvironmentName },
                { "AppName", "Logging Scopes" },
            };

            return scopeInfo;
        }

        public WeatherForecastController(IHostEnvironment hostEnv, ILogger<WeatherForecastController> logger) {
            _logger = logger;
            _hostEnv = hostEnv;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IEnumerable<WeatherForecast>> Get() {

            var sw = new Stopwatch();
            sw.Start();

            using (_logger.BeginScope(GetScopeInformation())) {
                _logger.LogInformation("Приложение работает из под учётной записи {UserName}");

                _logger.LogInformation("Запрос получен контроллером {Controller}, действие {ControllerAction}, DateTime: {DateTime}",
                 [
                     nameof(WeatherForecastController),
                    nameof(Get),
                    DateTimeOffset.Now,
                ]);

                var service = new WeatherForecastService();

                _logger.LogInformation("Обращение к сервису {Service}",
                    [
                        nameof(WeatherForecastService),
                ]);

                var result = await service.ProcessFTemperature();

                sw.Stop();

                _logger.LogInformation("Сервис {Service} обработал запрос за {ElapsedTime} мсек",
                    [
                        nameof(WeatherForecastService),
                    sw.ElapsedMilliseconds,
                ]);

                _logger.LogInformation($"Мы получили {result.Length} прогнозов от сервиса {{Service}}",
                    [
                        nameof(WeatherForecastService),
                ]);

                _logger.LogWarning("Тестовое предупреждение++");
                _logger.LogError(new Exception(), "Тестовая ошибка!++");

                return result;
            }
        }
    }
}
