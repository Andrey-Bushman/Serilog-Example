using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication1.Services;

namespace WebApplication1.Controllers {

    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase {

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IHostEnvironment _hostEnv;

        /// <summary>
        /// Сформировать дополнительный набор свойств, который хотим
        /// добавлять ко всем сообщениям лога в рамках Scope, в котором
        /// этот набор будет использоваться.
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, object> CreateScopeInformation() {

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

            // Формируем область, в рамках которой каждому отправляемому сообщению лога будут присваиваться
            // дополнительные свойства из объекта, переданного в качестве параметра методу BeginScope().
            using (_logger.BeginScope(CreateScopeInformation())) {

                // Пример №1: в тексте сообщения используем параметр UserName, определённый в рамках Scope.
                _logger.LogInformation("Приложение работает из под учётной записи {UserName}");

                // Пример №2: вторым параметром можно передавать массив объектов, значения которых можно использовать в тексте, указывая их в формате {AnyName}.
                // Порядок следования объектов в массиве должен совпадать с порядком следования используемых значений в тексте сообщения.
                // Добавляемые вами таким образом свойства автоматически будут добавлены и как структурированные свойства ДЛЯ КОНКРЕТНОЙ ДАННОЙ ЗАПИСИ ЛОГА
                // (вы их увидите в Seq).
                _logger.LogInformation("Запрос получен контроллером {Controller}, действие {ControllerAction}, DateTime: {DateTime}",
                    new object[]{
                        nameof(WeatherForecastController),
                        nameof(Get),
                        DateTimeOffset.Now,
                    });

                var service = new WeatherForecastService();

                _logger.LogInformation("Обращение к сервису {Service}",
                    new object[]{
                        nameof(WeatherForecastService),
                    });

                var result = await service.ProcessFTemperature();

                sw.Stop();

                _logger.LogInformation("Сервис {Service} обработал запрос за {ElapsedTime} мсек",
                    new object[]{
                        nameof(WeatherForecastService),
                        sw.ElapsedMilliseconds,
                    });

                _logger.LogInformation($"Мы получили {result.Length} прогнозов от сервиса {{Service}}",
                    new object[]{
                        nameof(WeatherForecastService),
                    });

                _logger.LogWarning("Тестовое предупреждение++");
                _logger.LogError(new Exception(), "Тестовая ошибка!++");

                return result;
            }
        }
    }
}
