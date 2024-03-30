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
                _logger.LogInformation("���������� �������� �� ��� ������� ������ {UserName}");

                _logger.LogInformation("������ ������� ������������ {Controller}, �������� {ControllerAction}, DateTime: {DateTime}",
                 [
                     nameof(WeatherForecastController),
                    nameof(Get),
                    DateTimeOffset.Now,
                ]);

                var service = new WeatherForecastService();

                _logger.LogInformation("��������� � ������� {Service}",
                    [
                        nameof(WeatherForecastService),
                ]);

                var result = await service.ProcessFTemperature();

                sw.Stop();

                _logger.LogInformation("������ {Service} ��������� ������ �� {ElapsedTime} ����",
                    [
                        nameof(WeatherForecastService),
                    sw.ElapsedMilliseconds,
                ]);

                _logger.LogInformation($"�� �������� {result.Length} ��������� �� ������� {{Service}}",
                    [
                        nameof(WeatherForecastService),
                ]);

                _logger.LogWarning("�������� ��������������++");
                _logger.LogError(new Exception(), "�������� ������!++");

                return result;
            }
        }
    }
}
